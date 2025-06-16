using EventSphere.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventSphere.Controllers
{
    //[Route("Events")]
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
            var now = DateTime.Now;

            var events = _context.Events
             .Where(e => e.Date >= now)
             .OrderBy(e => e.Date)
              .ToList();  
            return View(events);
        }
        public IActionResult Describe(int eventId)
        {
            var eventDetails = _context.Events.FirstOrDefault(e => e.Id == eventId);
            if (eventDetails == null)
            {
                return NotFound();
            }
            return View(eventDetails);
        }

        // GET: Events/Host
       
        public IActionResult PastEvents()
        {
            var now = DateTime.Now;

            var events = _context.Events
            .Where(e => e.Date < now)
            .OrderBy(e => e.Date)
             .ToList();    // Fetch all events from DB
            return View(events);
        }

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
        //[Authorize]

        //[AllowAnonymous]
        //public IActionResult Hosted_Event()
        //{
        //    return View(); // ← This renders the Razor view
        //}
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpGet("/Events/Get_Hosted_Event")]
        //public IActionResult Get_Hosted_Event ()
        //{
        //    string currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        //    // Fetch events hosted by this user
        //    var hostedEvents = _context.Events
        //        .Where(e => e.HostUserId == currentUserId)
        //        .ToList();

        //    return Ok(hostedEvents);
        //}
        [Authorize]
        public IActionResult Hosted_Event()
        {
            string currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Fetch events hosted by this user
            var hostedEvents = _context.Events
                .Where(e => e.HostUserId == currentUserId)
                .ToList();

            return View(hostedEvents);
        }
        [Authorize]
        [HttpGet]
        
        public async Task<IActionResult> Register(int eventId)
        {
            if (eventId == 0)
                return BadRequest("Invalid event ID.");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var ev = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                return NotFound();

            if (ev.Attendees.Contains(user))
                return RedirectToAction("Show_Registered");

            ev.Attendees.Add(user);
            ev.AttendeeCount++;

            await _context.SaveChangesAsync();

            return RedirectToAction("Explore"); // or wherever you want to go after registration
        }
        [HttpGet]
        [Authorize]
        public IActionResult Show_Registered()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        
        public async Task<IActionResult> Registered_Event()
        {
            var user = await _userManager.GetUserAsync(User);

            // Load the user's attended events using eager loading
            var attendedEvents = await _context.Users
                .Where(u => u.Id == user.Id)
                .SelectMany(u => u.AttendedEvents)
                .ToListAsync();

            return View(attendedEvents);
        }



    }
}