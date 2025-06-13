using EventSphere.Data;
using Microsoft.AspNetCore.Authorization;
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

          
            var events = _context.Events.ToList();  // Fetch all events from DB
            return View(events);
        }
        // GET: Events/Host

        [Authorize]
        public IActionResult Host()
        {
            return View();
        }

        // POST: Events/Host
        [Authorize]
        [HttpPost]
        public IActionResult Host(Event model)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("This is a test log");

                // Set HostUserId from the current user (assumes authentication)
                model.HostUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // Save this event to the DB:
                _context.Events.Add(model);
                _context.SaveChanges();

                // For now, redirect to Explorer or confirmation page
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Field: {entry.Key}, Error: {error.ErrorMessage}");
                    }
                }
               // return RedirectToAction("Explorer", "Events", new { id = model.Id });

            }

            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpGet]
        public IActionResult Hosted_Event ()
        {
            string currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Fetch events hosted by this user
            var hostedEvents = _context.Events
                .Where(e => e.HostUserId == currentUserId)
                .ToList();

            return View(hostedEvents);
        }



    }
}