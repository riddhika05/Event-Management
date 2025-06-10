using EventSphere.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EventSphere.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Host relationship (one-to-many)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.HostUser)
                .WithMany(u => u.HostedEvents)
                .HasForeignKey(e => e.HostUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent host user deletion

            // Configure Attendees relationship (many-to-many - automatic join table)
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Attendees)
                .WithMany(u => u.AttendedEvents);
        }
    }
}
