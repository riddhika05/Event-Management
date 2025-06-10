using EventSphere.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.Controllers
{
    public class Events : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CustomUser> _userManager;

        public Events(ApplicationDbContext context, UserManager<CustomUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Explore()
        {
            // Dummy host user
            var hostUser = new CustomUser
            {
                Id = "host123",
                UserName = "event.master",
                Email = "host@example.com"
            };

            var events = new List<Event>
        {
            new Event
            {
                Id = 1,
                Name = "Tech Innovations 2025",
                Description = "Explore the future of technology with industry leaders.",
                Date = new DateTime(2025, 7, 10),
                Location = "Mumbai, India",
                Organizer_Name = "TechWorld India",
                AttendeeCount = 120,
                ImageUrl = "/images/events/tech.jpg",
                HostUserId = hostUser.Id,
                HostUser = hostUser
            },
            new Event
            {
                Id = 2,
                Name = "Art & Culture Fest",
                Description = "A colorful celebration of traditional and modern art forms.",
                Date = new DateTime(2025, 8, 15),
                Location = "Jaipur, India",
                Organizer_Name = "Creative Souls",
                AttendeeCount = 250,
                ImageUrl = "/images/events/art.jpg",
                HostUserId = hostUser.Id,
                HostUser = hostUser
            },
            new Event
            {
                Id = 3,
                Name = "Startup Expo 2025",
                Description = "Showcasing innovative startups and networking with investors.",
                Date = new DateTime(2025, 9, 20),
                Location = "Bangalore, India",
                Organizer_Name = "StartupIndia",
                AttendeeCount = 300,
                ImageUrl = "/images/events/startup.jpg",
                HostUserId = hostUser.Id,
                HostUser = hostUser
            },
            new Event
            {
                Id = 4,
                Name = "Marathon for Health",
                Description = "Join the run to support mental and physical wellness.",
                Date = new DateTime(2025, 10, 5),
                Location = "Delhi, India",
                Organizer_Name = "FitLife Org",
                AttendeeCount = 500,
                ImageUrl = "/images/events/marathon.jpg",
                HostUserId = hostUser.Id,
                HostUser = hostUser
            },
            new Event
            {
                Id = 5,
                Name = "Music Fiesta",
                Description = "Live performances from top bands and solo artists.",
                Date = new DateTime(2025, 11, 12),
                Location = "Goa, India",
                Organizer_Name = "SoundWave Entertainment",
                AttendeeCount = 1000,
                ImageUrl = "/images/events/music.jpg",
                HostUserId = hostUser.Id,
                HostUser = hostUser
            }
          };

            return View(events);
        }

        public IActionResult Host()
        {
            return View();
        }
    }
}