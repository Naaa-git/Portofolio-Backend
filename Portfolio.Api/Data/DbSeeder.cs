using Portfolio.Api.Models.Entities;
using static Portfolio.Api.Data.TranslatableExtensions;

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
                Role = Tr("Fullstack Developer", "Fullstack Developer"),
                RoleAlternatives = TrList(
                    (".NET Specialist", ".NET Specialist"),
                    ("Vue.js Developer", "Vue.js Developer"),
                    ("React Developer", "React Developer")),
                Tagline = Tr(
                    "Membangun aplikasi web yang bersih dan scalable, dari backend sampai browser.",
                    "Building clean, scalable web applications from backend to browser."),
                Bio = Tr(
                    "Fullstack Developer dengan pengalaman 2+ tahun yang fokus di .NET dan Vue.js. Mahir di C#, TypeScript, " +
                    "JavaScript, React, dan framework modern lainnya, dengan pengalaman langsung di backend development, " +
                    "integrasi API, dan pengembangan aplikasi secara end-to-end.",
                    "Fullstack Developer with 2+ years of experience specializing in .NET and Vue.js. " +
                    "Proficient in C#, TypeScript, JavaScript, React, and other modern frameworks, with hands-on " +
                    "experience in backend development, API integration, and full lifecycle application development."),
                BioExtended = Tr(
                    "Saya suka menulis kode yang clean, maintainable, dan scalable. Saya senang memahami software " +
                    "architecture secara mendalam supaya bisa memberi saran teknis yang matang dan berkontribusi efektif " +
                    "dalam tim. Saat ini terbuka untuk peluang baru di mana saya bisa terus berkembang dan membangun " +
                    "produk yang berdampak.",
                    "I'm passionate about writing clean, maintainable, and scalable code. I enjoy " +
                    "understanding software architecture deeply to provide informed technical suggestions and contribute " +
                    "effectively in team environments. Currently open to new opportunities where I can continue growing " +
                    "and building impactful products."),
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
                new Skill { Category = Tr("Bahasa Pemrograman", "Languages"), Items = ["C#", "TypeScript", "JavaScript", "SQL"], SortOrder = 1 },
                new Skill { Category = Tr("Frontend", "Frontend"), Items = ["Vue.js", "React", "Nuxt 3", "Next.js", "HTML5", "CSS3", "Tailwind CSS"], SortOrder = 2 },
                new Skill { Category = Tr("Backend", "Backend"), Items = [".NET", "ASP.NET Core", "RESTful API", "JWT", "Background Jobs"], SortOrder = 3 },
                new Skill { Category = Tr("Database & Cache", "Database & Cache"), Items = ["PostgreSQL", "SQL Server", "Redis", "OpenSearch"], SortOrder = 4 },
                new Skill { Category = Tr("Tools & DevOps", "Tools & DevOps"), Items = ["Git", "GitLab", "Postman", "Docker", "Linux"], SortOrder = 5 }
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
                    Role = Tr("Full Stack Developer", "Full Stack Developer"),
                    Type = "Full-time",
                    Period = "Jul 2024 – Present",
                    Location = "Jakarta Selatan, Indonesia · On-site",
                    Current = true,
                    Description = TrList(
                        ("Meningkatkan performa aplikasi, memangkas waktu respons dari menit ke detik lewat optimasi query, indexing, perbaikan N+1 query, dan implementasi CTE.",
                         "Improved application performance, reducing response times from minutes to seconds through query optimization, indexing, resolving N+1 queries, and implementing CTEs."),
                        ("Migrasi dan penyesuaian data dari sistem legacy ke aplikasi baru, memastikan konsistensi data, integritas, dan transisi yang mulus untuk end-user.",
                         "Migrated and adapted data from legacy systems to new applications, ensuring data consistency, integrity, and smooth transition for end-users."),
                        ("Mengubah desain UI menjadi interface web yang interaktif menggunakan Vue.js, HTML5, dan TypeScript, meningkatkan pengalaman pengguna dan konsistensi tampilan.",
                         "Transformed UI designs into interactive web interfaces using Vue.js, HTML5, and TypeScript, enhancing user experience and interface consistency."),
                        ("Berkolaborasi dengan user dan stakeholder untuk memperjelas requirement dan menyelaraskan implementasi dengan business logic.",
                         "Collaborated with users and stakeholders to clarify requirements and align implementation with business logic."),
                        ("Mengimplementasikan autentikasi dan otorisasi menggunakan JWT dan role-based access control, mengamankan data aplikasi yang sensitif.",
                         "Implemented authentication and authorization using JWT and role-based access control, securing sensitive application data."),
                        ("Mengintegrasikan caching dan solusi pencarian dengan Redis dan OpenSearch, meningkatkan kecepatan pengambilan data secara signifikan.",
                         "Integrated caching and search solutions with Redis and OpenSearch, significantly improving data retrieval speed."),
                        ("Mengembangkan dan mengonsumsi RESTful API untuk komunikasi frontend-backend.",
                         "Developed and consumed RESTful APIs for frontend-backend communication."),
                        ("Membuat background job dan scheduled task untuk batch processing dan sinkronisasi data.",
                         "Created background jobs and scheduled tasks for batch processing and data synchronization.")),
                    Skills = [".NET", "Vue.js", "TypeScript", "Redis", "OpenSearch", "PostgreSQL", "JWT"],
                },
                new Experience
                {
                    Company = "PT Javan Cipta Solusi",
                    Role = Tr("Fullstack Web Developer", "Fullstack Web Developer"),
                    Type = "Part-time",
                    Period = "Oct 2023 – Apr 2024",
                    Location = "Yogyakarta, Indonesia · On-site",
                    Current = false,
                    Description = TrList(
                        ("Mengerjakan slicing untuk mengubah desain mockup menjadi kode fungsional dengan akurasi pixel-perfect menggunakan React dan TypeScript.",
                         "Executed slicing tasks to transform mockup designs into functional code with pixel-perfect accuracy using React and TypeScript."),
                        ("Memodifikasi infrastruktur backend untuk menyiapkan data yang dipakai frontend, dan menguji integrasi dengan Postman.",
                         "Modified backend infrastructure to prepare data for frontend utilization and tested integrations with Postman."),
                        ("Mengintegrasikan digital document management menggunakan OnlyOffice untuk meningkatkan efisiensi dan aksesibilitas sistem.",
                         "Integrated digital document management using OnlyOffice to enhance efficiency and accessibility within the system."),
                        ("Berinteraksi dengan backend API untuk mengambil dan menyimpan data menggunakan RESTful API.",
                         "Interacted with backend APIs for retrieving and storing data using RESTful APIs."),
                        ("Menggunakan GitLab untuk version control, kolaborasi, dan pelacakan perubahan.",
                         "Utilized GitLab for version control, collaboration, and change tracking.")),
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
                    ShortDescription = Tr(
                        "Platform HR enterprise untuk manajemen talent, payroll, dan lifecycle karyawan.",
                        "Enterprise HR platform for talent management, payroll, and employee lifecycle."),
                    LongDescription = Tr(
                        "Sistem manajemen HR komprehensif yang dibangun untuk skala enterprise. Fitur mencakup manajemen " +
                        "lifecycle karyawan, pemrosesan payroll, performance review, manajemen cuti, dan dashboard analitik " +
                        "detail. Dioptimalkan untuk operasi data volume tinggi dengan integrasi Redis caching dan OpenSearch.",
                        "A comprehensive HR management system built for enterprise scale. Features include employee lifecycle management, payroll processing, performance reviews, leave management, and detailed analytics dashboards. Optimized for high-volume data operations with Redis caching and OpenSearch integration."),
                    TechStack = [".NET", "Vue.js", "TypeScript", "PostgreSQL", "Redis", "OpenSearch"],
                    ImageUrl = "https://picsum.photos/seed/italent/800/450",
                    GithubUrl = "https://github.com/danana",
                    DemoUrl = "#",
                    Featured = true,
                    Category = Tr("Enterprise", "Enterprise"),
                },
                new Project
                {
                    Title = "DocFlow — Digital Document Management",
                    Slug = "docflow-document-management",
                    ShortDescription = Tr(
                        "Sistem manajemen dokumen dengan integrasi OnlyOffice untuk kolaborasi real-time.",
                        "Document management system with OnlyOffice integration for real-time collaboration."),
                    LongDescription = Tr(
                        "Platform manajemen dokumen digital yang mengintegrasikan OnlyOffice untuk editing kolaboratif. " +
                        "Mendukung versioning dokumen, role-based access control, workflow approval, dan audit trail. " +
                        "Dibangun dengan React dan backend .NET, dengan fitur kolaborasi real-time.",
                        "A digital document management platform that integrates OnlyOffice for collaborative editing. Supports document versioning, role-based access control, workflow approvals, and audit trails. Built with React and a .NET backend, with real-time collaboration features."),
                    TechStack = ["React", "TypeScript", ".NET", "PostgreSQL", "OnlyOffice"],
                    ImageUrl = "https://picsum.photos/seed/docflow/800/450",
                    GithubUrl = "https://github.com/danana",
                    DemoUrl = "#",
                    Featured = true,
                    Category = Tr("Produktivitas", "Productivity"),
                },
                new Project
                {
                    Title = "Analytics Dashboard",
                    Slug = "analytics-dashboard",
                    ShortDescription = Tr(
                        "Dashboard analitik real-time dengan chart interaktif dan visualisasi data.",
                        "Real-time analytics dashboard with interactive charts and data visualization."),
                    LongDescription = Tr(
                        "Dashboard analitik modern yang menyajikan insight real-time lewat chart dan visualisasi interaktif. " +
                        "Fitur mencakup widget yang bisa dikustomisasi, export data, filter rentang tanggal, dan dukungan " +
                        "multi-tenant. Dioptimalkan untuk performa dengan lazy loading dan desain API yang efisien.",
                        "A modern analytics dashboard providing real-time insights through interactive charts and visualizations. Features include customizable widgets, data export, date range filtering, and multi-tenant support. Optimized for performance with lazy loading and efficient API design."),
                    TechStack = ["Vue.js", "TypeScript", ".NET", "Chart.js", "Redis"],
                    ImageUrl = "https://picsum.photos/seed/analytics/800/450",
                    GithubUrl = "https://github.com/danana",
                    DemoUrl = "#",
                    Featured = true,
                    Category = Tr("Analitik", "Analytics"),
                },
                new Project
                {
                    Title = "AuthGuard — JWT Auth Boilerplate",
                    Slug = "authguard-jwt-boilerplate",
                    ShortDescription = Tr(
                        "Boilerplate autentikasi production-ready dengan JWT dan role-based access control.",
                        "Production-ready authentication boilerplate with JWT and role-based access control."),
                    LongDescription = Tr(
                        "Sistem autentikasi dan otorisasi yang bisa dipakai ulang, dibangun dengan .NET dan Vue.js. " +
                        "Termasuk manajemen JWT token, refresh token rotation, role-based access control (RBAC), rate " +
                        "limiting, dan audit logging yang komprehensif. Didesain sebagai starter template untuk aplikasi " +
                        "enterprise.",
                        "A reusable authentication and authorization system built with .NET and Vue.js. Includes JWT token management, refresh token rotation, role-based access control (RBAC), rate limiting, and comprehensive audit logging. Designed as a starter template for enterprise applications."),
                    TechStack = [".NET", "Vue.js", "TypeScript", "JWT", "PostgreSQL"],
                    ImageUrl = "https://picsum.photos/seed/authguard/800/450",
                    GithubUrl = "https://github.com/danana",
                    DemoUrl = "#",
                    Featured = false,
                    Category = Tr("Security", "Security"),
                },
                new Project
                {
                    Title = "TaskFlow — Project Management App",
                    Slug = "taskflow-project-management",
                    ShortDescription = Tr(
                        "Tool manajemen project ala Kanban dengan fitur kolaborasi tim.",
                        "Kanban-style project management tool with team collaboration features."),
                    LongDescription = Tr(
                        "Aplikasi manajemen project yang ringan dengan Kanban board, assignment task, pelacakan deadline, " +
                        "dan fitur kolaborasi tim. Dibangun dengan React dan RESTful API .NET, dengan update real-time " +
                        "lewat SignalR.",
                        "A lightweight project management application with Kanban boards, task assignments, deadline tracking, and team collaboration features. Built with React and a RESTful .NET API, with real-time updates via SignalR."),
                    TechStack = ["React", "TypeScript", ".NET", "SignalR", "PostgreSQL"],
                    ImageUrl = "https://picsum.photos/seed/taskflow/800/450",
                    GithubUrl = "https://github.com/danana",
                    DemoUrl = "#",
                    Featured = false,
                    Category = Tr("Produktivitas", "Productivity"),
                }
            );
        }

        if (!db.OutsideCodeIntros.Any())
        {
            db.OutsideCodeIntros.Add(new OutsideCodeIntro
            {
                Paragraph1 = Tr(
                    "Halaman ini nggak ada project, skill, atau achievement — isinya sisi gw di luar kerjaan.",
                    "This page has no projects, skills, or achievements — it's just the side of me outside of work."),
                Paragraph2 = Tr(
                    "Di luar kerjaan, perhatian gw suka pindah-pindah. Minggu ini mikirin software architecture, " +
                    "minggu depan udah kepikiran hal yang nggak ada hubungannya sama sekali, terus abis itu sibuk " +
                    "belajar gitar. Nggak ada benang merah yang rapi — emang gitu aja.",
                    "Outside of work, my attention keeps shifting around. This week it's software architecture, " +
                    "next week it's something completely unrelated, then I'm off learning guitar. There's no neat " +
                    "common thread — that's just how it is."),
            });
        }

        if (!db.AwayFromKeyboardItems.Any())
        {
            db.AwayFromKeyboardItems.AddRange(
                new AwayFromKeyboardItem
                {
                    Title = Tr("Guitar", "Guitar"),
                    Note = Tr(
                        "Mulai dari nol — belum pernah pegang alat musik sebelumnya. Lagi latihan chord, perpindahan chord, strumming.",
                        "Starting from zero — never picked up an instrument before. Working on chords, chord changes, strumming."),
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/45/GuitareClassique5.png",
                    SortOrder = 1,
                },
                new AwayFromKeyboardItem
                {
                    Title = Tr("Lari", "Running"),
                    Note = Tr(
                        "Easy running, belum fokus ngejar pace. Lebih ke suka aja habis lari.",
                        "Easy running, not chasing pace yet. Mostly just enjoying how it feels afterward."),
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/0/0a/Speedsuit.jpg",
                    SortOrder = 2,
                },
                new AwayFromKeyboardItem
                {
                    Title = Tr("Workout", "Workout"),
                    Note = Tr(
                        "Fokus hypertrophy, progres jangka panjang daripada latihan random.",
                        "Focused on hypertrophy — long-term progress over random workouts."),
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/d/dc/Small_group_fitness_sessions_bundall.jpg",
                    SortOrder = 3,
                },
                new AwayFromKeyboardItem
                {
                    Title = Tr("Membaca", "Reading"),
                    Note = Tr(
                        "Suka baca soal psychology dan self-development. Kadang kelar, kadang mandek di tengah terus pindah buku lain.",
                        "I like reading about psychology and self-development. Sometimes I finish a book, sometimes I stall halfway and jump to another."),
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/b6/Gutenberg_Bible%2C_Lenox_Copy%2C_New_York_Public_Library%2C_2009._Pic_01.jpg",
                    SortOrder = 4,
                },
                new AwayFromKeyboardItem
                {
                    Title = Tr("Games", "Games"),
                    Note = Tr(
                        "Suka main, tapi juga suka mikirin kenapa suatu game bikin pengen dimainin lagi dan lagi. Lagi iseng nyoba bikin game sendiri.",
                        "I enjoy playing, but I'm also curious why a game keeps pulling you back in. Been messing around with making one myself."),
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
                    Take = Tr(
                        "Serunya luar biasa dari awal sampai beberapa season terakhir. Endingnya menurut gw nggak sebagus itu.",
                        "Incredibly good from the start through most of the later seasons. The ending, in my opinion, wasn't that great."),
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/d/d8/Game_of_Thrones_title_card.jpg",
                    SortOrder = 1,
                },
                new MovieTake
                {
                    Title = "Attack on Titan",
                    Take = Tr(
                        "Gw di kubu Eren. Ngerti kalau ini pendapat yang nggak semua orang setuju.",
                        "I'm on Eren's side. I know not everyone agrees with that take."),
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
                    Note = Tr(
                        "Bikin sadar: logic itu lebih sering kalah sama insting cepat daripada yang kita kira.",
                        "Made me realize: logic loses to quick instinct far more often than we think."),
                    ImageUrl = "https://covers.openlibrary.org/b/id/13290711-L.jpg",
                    IsCurrentlyReading = false,
                    SortOrder = 2,
                },
                new OutsideCodeBook
                {
                    Title = "Atomic Habits",
                    Author = "James Clear",
                    Note = Tr(
                        "Beberapa habit kecil dari buku ini masih jalan sampai sekarang.",
                        "A few small habits from this book are still running to this day."),
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
                    Aspect = Tr("Pengetahuan", "Knowledge"),
                    Note = Tr(
                        "Caranya ngemas hal yang berat jadi ringan dan gampang dicerna — tanpa kehilangan isinya.",
                        "The way he packages heavy ideas into something light and easy to digest — without losing the substance."),
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/25/Raditya_Dika_on_Interview_GoGirl_TV.jpg",
                    SortOrder = 1,
                },
                new LifeInspiration
                {
                    Name = "dr. Tirta",
                    Aspect = Tr("Kesehatan & Lifestyle", "Health & Lifestyle"),
                    Note = Tr(
                        "Blak-blakan soal kesehatan dan kebiasaan hidup, tanpa basa-basi yang nggak perlu.",
                        "Straightforward about health and lifestyle habits, no unnecessary sugarcoating."),
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/7e/Tirta_Mandira_Hudhi_%28cropped%29.jpg",
                    SortOrder = 2,
                },
                new LifeInspiration
                {
                    Name = "Windah Basudara",
                    Aspect = Tr("Funny tapi ada sisi serius", "Funny but has a serious side"),
                    Note = Tr(
                        "Kelihatan konyol di permukaan, tapi konsisten dan kerja keras di baliknya — jarang orang notice itu.",
                        "Looks goofy on the surface, but there's consistency and hard work behind it — rarely noticed."),
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/eb/Windah_Basudara_di_tahun_2022.jpg",
                    SortOrder = 3,
                },
                new LifeInspiration
                {
                    Name = "Marc Márquez",
                    Aspect = Tr("Ambisi & Tekad", "Ambition & Determination"),
                    Note = Tr(
                        "Cedera parah berkali-kali, tapi tetap balik lagi ke lintasan. Itu level tekad yang susah ditiru.",
                        "Severely injured over and over, yet keeps coming back to the track. That level of determination is hard to match."),
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a7/Marc_Marquez_at_the_2026_Spanish_Grand_Prix_%28cropped%29.jpg",
                    SortOrder = 4,
                }
            );
        }

        await db.SaveChangesAsync();
    }
}
