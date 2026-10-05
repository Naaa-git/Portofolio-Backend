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

        if (!db.OutsideCodeIntros.Any())
        {
            db.OutsideCodeIntros.Add(new OutsideCodeIntro
            {
                Paragraph1 = "Halaman ini nggak ada project, skill, atau achievement — isinya sisi gw di luar kerjaan.",
                Paragraph2 = "Di luar kerjaan, perhatian gw suka pindah-pindah. Minggu ini mikirin software architecture, " +
                             "minggu depan udah kepikiran hal yang nggak ada hubungannya sama sekali, terus abis itu sibuk " +
                             "belajar gitar. Nggak ada benang merah yang rapi — emang gitu aja.",
            });
        }

        if (!db.AwayFromKeyboardItems.Any())
        {
            db.AwayFromKeyboardItems.AddRange(
                new AwayFromKeyboardItem
                {
                    Title = "Guitar",
                    Note = "Mulai dari nol — belum pernah pegang alat musik sebelumnya. Lagi latihan chord, perpindahan chord, strumming.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/45/GuitareClassique5.png",
                    SortOrder = 1,
                },
                new AwayFromKeyboardItem
                {
                    Title = "Running",
                    Note = "Easy running, belum fokus ngejar pace. Lebih ke suka aja habis lari.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/0/0a/Speedsuit.jpg",
                    SortOrder = 2,
                },
                new AwayFromKeyboardItem
                {
                    Title = "Workout",
                    Note = "Fokus hypertrophy, progres jangka panjang daripada latihan random.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/d/dc/Small_group_fitness_sessions_bundall.jpg",
                    SortOrder = 3,
                },
                new AwayFromKeyboardItem
                {
                    Title = "Reading",
                    Note = "Suka baca soal psychology dan self-development. Kadang kelar, kadang mandek di tengah terus pindah buku lain.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/b6/Gutenberg_Bible%2C_Lenox_Copy%2C_New_York_Public_Library%2C_2009._Pic_01.jpg",
                    SortOrder = 4,
                },
                new AwayFromKeyboardItem
                {
                    Title = "Games",
                    Note = "Suka main, tapi juga suka mikirin kenapa suatu game bikin pengen dimainin lagi dan lagi. Lagi iseng nyoba bikin game sendiri.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/cf/SNES-Controller-in-Hand.jpg",
                    SortOrder = 5,
                }
            );
        }

        if (!db.MovieTakes.Any())
        {
            db.MovieTakes.AddRange(
                new MovieTake
                {
                    Title = "Game of Thrones",
                    Take = "Serunya luar biasa dari awal sampai beberapa season terakhir. Endingnya menurut gw nggak sebagus itu.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/d/d8/Game_of_Thrones_title_card.jpg",
                    SortOrder = 1,
                },
                new MovieTake
                {
                    Title = "Attack on Titan",
                    Take = "Gw di kubu Eren. Ngerti kalau ini pendapat yang nggak semua orang setuju.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/d/d6/Shingeki_no_Kyojin_manga_volume_1.jpg",
                    SortOrder = 2,
                }
            );
        }

        if (!db.MusicArtists.Any())
        {
            db.MusicArtists.AddRange(
                new MusicArtist
                {
                    Name = "Rex Orange County",
                    Url = "https://open.spotify.com/artist/7pbDxGE6nQSZVfiFdq9lOL",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/cf/Rex_Orange_County_-_Photo_by_Skyler_Pradhan_%28cropped%29.jpg",
                    SortOrder = 1,
                },
                new MusicArtist
                {
                    Name = "Jeremy Zucker",
                    Url = "https://open.spotify.com/intl-id/artist/3gIRvgZssIb9aiirIg0nI3",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/ee/Jeremy_Zucker_Is_Nothing_Sacred_Tour.jpg",
                    SortOrder = 2,
                },
                new MusicArtist
                {
                    Name = "Denny Caknan",
                    Url = "https://open.spotify.com/intl-id/artist/3Gr3opnAGpJiTowsTyJFWG",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/0/09/Denny_Caknan_Boshe_Jogja.png",
                    SortOrder = 3,
                }
            );
        }

        if (!db.PodcastChannels.Any())
        {
            db.PodcastChannels.AddRange(
                new PodcastChannel
                {
                    Name = "Raditya Dika",
                    Url = "https://www.youtube.com/@radityadika",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/25/Raditya_Dika_on_Interview_GoGirl_TV.jpg",
                    SortOrder = 1,
                },
                new PodcastChannel
                {
                    Name = "dr. Tirta",
                    Url = "https://www.youtube.com/@TirtaPengPengPeng",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/7e/Tirta_Mandira_Hudhi_%28cropped%29.jpg",
                    SortOrder = 2,
                },
                new PodcastChannel
                {
                    Name = "PZN (Programmer Zaman Now)",
                    Url = "https://www.youtube.com/@ProgrammerZamanNow",
                    ImageUrl = null,
                    SortOrder = 3,
                }
            );
        }

        if (!db.OutsideCodeBooks.Any())
        {
            db.OutsideCodeBooks.AddRange(
                new OutsideCodeBook
                {
                    Title = "Man's Search for Meaning",
                    Author = "Viktor Frankl",
                    ImageUrl = "https://covers.openlibrary.org/b/id/8516506-L.jpg",
                    IsCurrentlyReading = true,
                    SortOrder = 1,
                },
                new OutsideCodeBook
                {
                    Title = "Thinking, Fast and Slow",
                    Author = "Daniel Kahneman",
                    Note = "Bikin sadar: logic itu lebih sering kalah sama insting cepat daripada yang kita kira.",
                    ImageUrl = "https://covers.openlibrary.org/b/id/13290711-L.jpg",
                    IsCurrentlyReading = false,
                    SortOrder = 2,
                },
                new OutsideCodeBook
                {
                    Title = "Atomic Habits",
                    Author = "James Clear",
                    Note = "Beberapa habit kecil dari buku ini masih jalan sampai sekarang.",
                    ImageUrl = "https://covers.openlibrary.org/b/id/12539702-L.jpg",
                    IsCurrentlyReading = false,
                    SortOrder = 3,
                }
            );
        }

        if (!db.LifeInspirations.Any())
        {
            db.LifeInspirations.AddRange(
                new LifeInspiration
                {
                    Name = "Raditya Dika",
                    Aspect = "Pengetahuan",
                    Note = "Caranya ngemas hal yang berat jadi ringan dan gampang dicerna — tanpa kehilangan isinya.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/25/Raditya_Dika_on_Interview_GoGirl_TV.jpg",
                    SortOrder = 1,
                },
                new LifeInspiration
                {
                    Name = "dr. Tirta",
                    Aspect = "Kesehatan & Lifestyle",
                    Note = "Blak-blakan soal kesehatan dan kebiasaan hidup, tanpa basa-basi yang nggak perlu.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/7e/Tirta_Mandira_Hudhi_%28cropped%29.jpg",
                    SortOrder = 2,
                },
                new LifeInspiration
                {
                    Name = "Windah Basudara",
                    Aspect = "Funny tapi ada sisi serius",
                    Note = "Kelihatan konyol di permukaan, tapi konsisten dan kerja keras di baliknya — jarang orang notice itu.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/eb/Windah_Basudara_di_tahun_2022.jpg",
                    SortOrder = 3,
                },
                new LifeInspiration
                {
                    Name = "Marc Márquez",
                    Aspect = "Ambisi & Tekad",
                    Note = "Cedera parah berkali-kali, tapi tetap balik lagi ke lintasan. Itu level tekad yang susah ditiru.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a7/Marc_Marquez_at_the_2026_Spanish_Grand_Prix_%28cropped%29.jpg",
                    SortOrder = 4,
                }
            );
        }

        await db.SaveChangesAsync();
    }
}
