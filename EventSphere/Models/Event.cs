using EventSphere.Models;
using System.ComponentModel.DataAnnotations;
using EventSphere.Models.Validation;
public class Event
{
    public int Id { get; set; }
    public string ?Name { get; set; }
    public string ?Description { get; set; }

    [DataType(DataType.DateTime)]
    [FutureDate]    
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public string Organizer_Name { get; set; }
    public int AttendeeCount { get; set; }

    public string ImageUrl { get; set; } // Optional image URL for the event

    // Host (one-to-many)
    public string? HostUserId { get; set; }
    public CustomUser ?HostUser { get; set; }

    // Attendees (many-to-many - EF Core will handle the join table automatically)
    public ICollection<CustomUser> Attendees { get; set; } = new List<CustomUser>();
}