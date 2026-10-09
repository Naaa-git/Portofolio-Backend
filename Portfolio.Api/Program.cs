// v1.0.1 — rollback mechanism test, no behavior change.
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Portfolio.Api.Data;
using Portfolio.Api.Repositories;
using Portfolio.Api.Services;
using Prometheus;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

// Structured logging: every log line is a JSON object with named properties
// (not free-form text), which is what makes it filterable/queryable once it
// lands in Loki instead of just being grep-able. Also quiets EF Core's
// per-query SQL noise down to Warning, which was drowning out everything else.
builder.Host.UseSerilog((context, services, loggerConfig) =>
{
    var lokiUrl = context.Configuration["Loki:Url"];

    loggerConfig
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "Portfolio.Api")
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
        .WriteTo.Console();

    if (!string.IsNullOrEmpty(lokiUrl))
    {
        loggerConfig.WriteTo.GrafanaLoki(lokiUrl, labels:
        [
            new LokiLabel { Key = "app", Value = "portfolio-api" },
            new LokiLabel { Key = "env", Value = context.HostingEnvironment.EnvironmentName },
        ]);
    }
});

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Portfolio API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Description = "Paste the JWT returned by /api/auth/login (without the \"Bearer \" prefix).",
    });

    options.AddSecurityRequirement(_ => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", null),
            new List<string>()
        },
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuditSaveChangesInterceptor>();
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
        .AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>()));

// Hit by infra (load balancer / uptime monitor), not by the frontend — reports
// whether the app can actually reach Postgres, not just "process is running".
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Default")!, name: "postgres");

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection(GoogleOptions.SectionName));
builder.Services.Configure<MicrosoftOptions>(builder.Configuration.GetSection(MicrosoftOptions.SectionName));
builder.Services.Configure<BrevoOptions>(builder.Configuration.GetSection(BrevoOptions.SectionName));
builder.Services.Configure<SeedAdminOptions>(builder.Configuration.GetSection(SeedAdminOptions.SectionName));
builder.Services.Configure<OpenSearchOptions>(builder.Configuration.GetSection(OpenSearchOptions.SectionName));
builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection(KafkaOptions.SectionName));
builder.Services.AddSingleton<IOtpEmailProducer, KafkaOtpEmailProducer>();
builder.Services.AddHostedService<OtpEmailConsumer>();

builder.Services.AddSingleton(new ConfigurationManager<OpenIdConnectConfiguration>(
    "https://login.microsoftonline.com/consumers/v2.0/.well-known/openid-configuration",
    new OpenIdConnectConfigurationRetriever()));

builder.Services.AddHttpClient<IEmailSender, BrevoEmailSender>();

builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<ISocialLinkRepository, SocialLinkRepository>();
builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
builder.Services.AddScoped<IOutsideCodeRepository, OutsideCodeRepository>();

builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ISocialLinkService, SocialLinkService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddSingleton<IProjectSearchService, ProjectSearchService>();
builder.Services.AddScoped<IOutsideCodeService, OutsideCodeService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<ITotpService, TotpService>();

// Dev-only mock; swap to an Azure Blob Storage implementation when ready (see LocalDiskImageStorageService).
builder.Services.AddScoped<IImageStorageService, LocalDiskImageStorageService>();

var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
var signingKey = jwtSection["SigningKey"]
    ?? throw new InvalidOperationException("Jwt:SigningKey is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
        };
    });

builder.Services.AddAuthorization();

// Auth endpoints (login, OTP/TOTP verification) get a strict per-IP limit —
// this is the actual defense against brute-forcing a 6-digit code, not the
// hashing. 5 attempts/minute is generous for a real human, harsh for a script.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("auth", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
            }));
});

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    var seedAdmin = scope.ServiceProvider.GetRequiredService<IOptions<SeedAdminOptions>>().Value;
    await DbSeeder.SeedAsync(db, seedAdmin);

    // Full reindex on every startup keeps OpenSearch from ever drifting out of
    // sync with Postgres (e.g. after editing seed data directly, or restoring
    // a DB backup) — cheap enough at this scale to just redo it every boot.
    var searchService = scope.ServiceProvider.GetRequiredService<IProjectSearchService>();
    await searchService.EnsureIndexAsync();
    await searchService.ReindexAllAsync(await db.Projects.ToListAsync());
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// One structured log line per request (method, path, status code, elapsed ms) —
// this is what actually gets searched/filtered in Loki day-to-day, far more
// than the individual EF Core query logs.
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("Frontend");
app.UseRateLimiter();

// Must wrap routing so every request (including 401s) gets counted —
// placed after auth middleware would miss rejected requests entirely.
app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapMetrics(); // GET /metrics — scraped by Prometheus, not meant for browsers
app.MapHealthChecks("/health");

AppMetrics.EnsureRegistered();

app.Run();
