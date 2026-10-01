using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Data;

/// <summary>
/// Ports the hardcoded content from the Nuxt frontend's data/*.ts files into the database
/// on first run, so the site keeps working immediately after switching to the API.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!db.AdminUsers.Any())
        {
            db.AdminUsers.Add(new AdminUser
            {
                Username = "admin",
                // Default password: "ChangeMe123!" — change it via the admin UI after first login.
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("ChangeMe123!"),
                Email = "aldimusthofa02@gmail.com",
            });
        }

        if (!db.Profiles.Any())
        {
            db.Profiles.Add(new Profile
            {
                Name = "Pradana Aldi Musthofa",
                ShortName = "Pradana",
                Role = "Fullstack Developer",
                RoleAlternatives = [".NET Specialist", "Vue.js Developer", "React Developer"],
                Tagline = "Building clean, scalable web applications from backend to browser.",
                Bio = "Fullstack Developer with 2+ years of experience specializing in .NET and Vue.js. " +
                      "Proficient in C#, TypeScript, JavaScript, React, and other modern frameworks, with hands-on " +
                      "experience in backend development, API integration, and full lifecycle application development.",
                BioExtended = "I'm passionate about writing clean, maintainable, and scalable code. I enjoy " +
                              "understanding software architecture deeply to provide informed technical suggestions and contribute " +
                              "effectively in team environments. Currently open to new opportunities where I can continue growing " +
                              "and building impactful products.",
                AvailableForWork = true,
                Email = "aldimusthofa02@gmail.com",
                Location = "Jakarta, Indonesia",
                AvatarUrl = "https://picsum.photos/seed/pradana/400/400",
                CvUrl = "#",
            });
        }

        if (!db.Skills.Any())
        {
            db.Skills.AddRange(
                new Skill { Category = "Languages", Items = ["C#", "TypeScript", "JavaScript", "SQL"], SortOrder = 1 },
                new Skill { Category = "Frontend", Items = ["Vue.js", "React", "Nuxt 3", "Next.js", "HTML5", "CSS3", "Tailwind CSS"], SortOrder = 2 },
                new Skill { Category = "Backend", Items = [".NET", "ASP.NET Core", "RESTful API", "JWT", "Background Jobs"], SortOrder = 3 },
                new Skill { Category = "Database & Cache", Items = ["PostgreSQL", "SQL Server", "Redis", "OpenSearch"], SortOrder = 4 },
                new Skill { Category = "Tools & DevOps", Items = ["Git", "GitLab", "Postman", "Docker", "Linux"], SortOrder = 5 }
            );
        }

        if (!db.SocialLinks.Any())
        {
            db.SocialLinks.AddRange(
                new SocialLink { Name = "GitHub", Url = "https://github.com/danana", Icon = "github" },
                new SocialLink { Name = "LinkedIn", Url = "https://linkedin.com/in/pradana-aldi", Icon = "linkedin" },
                new SocialLink { Name = "Email", Url = "mailto:aldimusthofa02@gmail.com", Icon = "mail" }
            );
        }

        if (!db.Experiences.Any())
        {
            db.Experiences.AddRange(
                new Experience
                {
                    Company = "Kodehive (PT Kode Gama Teknologi)",
                    Role = "Full Stack Developer",
                    Type = "Full-time",
                    Period = "Jul 2024 – Present",
                    Location = "Jakarta Selatan, Indonesia · On-site",
                    Current = true,
                    Description =
                    [
                        "Improved application performance, reducing response times from minutes to seconds through query optimization, indexing, resolving N+1 queries, and implementing CTEs.",
                        "Migrated and adapted data from legacy systems to new applications, ensuring data consistency, integrity, and smooth transition for end-users.",
                        "Transformed UI designs into interactive web interfaces using Vue.js, HTML5, and TypeScript, enhancing user experience and interface consistency.",
                        "Collaborated with users and stakeholders to clarify requirements and align implementation with business logic.",
                        "Implemented authentication and authorization using JWT and role-based access control, securing sensitive application data.",
                        "Integrated caching and search solutions with Redis and OpenSearch, significantly improving data retrieval speed.",
                        "Developed and consumed RESTful APIs for frontend-backend communication.",
                        "Created background jobs and scheduled tasks for batch processing and data synchronization.",
                    ],
                    Skills = [".NET", "Vue.js", "TypeScript", "Redis", "OpenSearch", "PostgreSQL", "JWT"],
                },
                new Experience
                {
                    Company = "PT Javan Cipta Solusi",
                    Role = "Fullstack Web Developer",
                    Type = "Part-time",
                    Period = "Oct 2023 – Apr 2024",
                    Location = "Yogyakarta, Indonesia · On-site",
                    Current = false,
                    Description =
                    [
                        "Executed slicing tasks to transform mockup designs into functional code with pixel-perfect accuracy using React and TypeScript.",
                        "Modified backend infrastructure to prepare data for frontend utilization and tested integrations with Postman.",
                        "Integrated digital document management using OnlyOffice to enhance efficiency and accessibility within the system.",
                        "Interacted with backend APIs for retrieving and storing data using RESTful APIs.",
                        "Utilized GitLab for version control, collaboration, and change tracking.",
                    ],
                    Skills = ["React", "TypeScript", "OnlyOffice", "RESTful API", "GitLab"],
                }
            );
        }

        if (!db.Projects.Any())
        {
            db.Projects.AddRange(
                new Project
                {
                    Title = "iTalent — HR & Talent Management System",
                    Slug = "italent-hr-system",
                    ShortDescription = "Enterprise HR platform for talent management, payroll, and employee lifecycle.",
                    LongDescription = "A comprehensive HR management system built for enterprise scale. Features include employee lifecycle management, payroll processing, performance reviews, leave management, and detailed analytics dashboards. Optimized for high-volume data operations with Redis caching and OpenSearch integration.",
                    TechStack = [".NET", "Vue.js", "TypeScript", "PostgreSQL", "Redis", "OpenSearch"],
                    ImageUrl = "https://picsum.photos/seed/italent/800/450",
                    GithubUrl = "https://github.com/danana",
                    DemoUrl = "#",
                    Featured = true,
                    Category = "Enterprise",
                },
                new Project
                {
                    Title = "DocFlow — Digital Document Management",
                    Slug = "docflow-document-management",
                    ShortDescription = "Document management system with OnlyOffice integration for real-time collaboration.",
                    LongDescription = "A digital document management platform that integrates OnlyOffice for collaborative editing. Supports document versioning, role-based access control, workflow approvals, and audit trails. Built with React and a .NET backend, with real-time collaboration features.",
                    TechStack = ["React", "TypeScript", ".NET", "PostgreSQL", "OnlyOffice"],
                    ImageUrl = "https://picsum.photos/seed/docflow/800/450",
                    GithubUrl = "https://github.com/danana",
                    DemoUrl = "#",
                    Featured = true,
                    Category = "Productivity",
                },
                new Project
                {
                    Title = "Analytics Dashboard",
                    Slug = "analytics-dashboard",
                    ShortDescription = "Real-time analytics dashboard with interactive charts and data visualization.",
                    LongDescription = "A modern analytics dashboard providing real-time insights through interactive charts and visualizations. Features include customizable widgets, data export, date range filtering, and multi-tenant support. Optimized for performance with lazy loading and efficient API design.",
                    TechStack = ["Vue.js", "TypeScript", ".NET", "Chart.js", "Redis"],
                    ImageUrl = "https://picsum.photos/seed/analytics/800/450",
                    GithubUrl = "https://github.com/danana",
                    DemoUrl = "#",
                    Featured = true,
                    Category = "Analytics",
                },
                new Project
                {
                    Title = "AuthGuard — JWT Auth Boilerplate",
                    Slug = "authguard-jwt-boilerplate",
                    ShortDescription = "Production-ready authentication boilerplate with JWT and role-based access control.",
                    LongDescription = "A reusable authentication and authorization system built with .NET and Vue.js. Includes JWT token management, refresh token rotation, role-based access control (RBAC), rate limiting, and comprehensive audit logging. Designed as a starter template for enterprise applications.",
                    TechStack = [".NET", "Vue.js", "TypeScript", "JWT", "PostgreSQL"],
                    ImageUrl = "https://picsum.photos/seed/authguard/800/450",
                    GithubUrl = "https://github.com/danana",
                    DemoUrl = "#",
                    Featured = false,
                    Category = "Security",
                },
                new Project
                {
                    Title = "TaskFlow — Project Management App",
                    Slug = "taskflow-project-management",
                    ShortDescription = "Kanban-style project management tool with team collaboration features.",
                    LongDescription = "A lightweight project management application with Kanban boards, task assignments, deadline tracking, and team collaboration features. Built with React and a RESTful .NET API, with real-time updates via SignalR.",
                    TechStack = ["React", "TypeScript", ".NET", "SignalR", "PostgreSQL"],
                    ImageUrl = "https://picsum.photos/seed/taskflow/800/450",
                    GithubUrl = "https://github.com/danana",
                    DemoUrl = "#",
                    Featured = false,
                    Category = "Productivity",
                }
            );
        }

        await db.SaveChangesAsync();
    }
}
