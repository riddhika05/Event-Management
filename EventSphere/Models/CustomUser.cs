using Microsoft.AspNetCore.Identity;

public class CustomUser : IdentityUser
{
    // Hosted events (one-to-many)
    public ICollection<Event> HostedEvents { get; set; } = new List<Event>();

    // Attended events (many-to-many)
    public ICollection<Event> AttendedEvents { get; set; } = new List<Event>();
}