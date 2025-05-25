// Data/EventDbContext.cs
using Microsoft.EntityFrameworkCore;
using ventixe_event_service.Models;

namespace ventixe_event_service.Data
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.TotalPrice).HasColumnType("decimal(18,2)");
                entity.HasOne(b => b.Event)
                      .WithMany()
                      .HasForeignKey(b => b.EventId);
            });

            // Seed test events (baserat på dina bilder)
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = "Adventure Gear Show",
                    Description = "Utställning av äventyrsutrusning och outdoor-produkter",
                    Category = "Outdoor & Adventure",
                    Date = new DateTime(2024, 6, 5),
                    Time = "9:00 AM",
                    Location = "Rocky Ridge Exhibition Hall, Denver, CO",
                    Price = 30m,
                    Color = "#4CAF50",
                    MaxTickets = 200,
                    SoldTickets = 45
                },
                new Event
                {
                    Id = 2,
                    Title = "Symphony Under the Stars",
                    Description = "Magisk symfonikonsert under stjärnhimlen",
                    Category = "Music",
                    Date = new DateTime(2024, 4, 10),
                    Time = "7:00 PM",
                    Location = "Sunset Park, Los Angeles, CA",
                    Price = 40m,
                    Color = "#e91e63",
                    MaxTickets = 500,
                    SoldTickets = 234
                },
                new Event
                {
                    Id = 3,
                    Title = "Runway Revolution 2029",
                    Description = "Framtidens mode visas på catwalken",
                    Category = "Fashion",
                    Date = new DateTime(2024, 5, 1),
                    Time = "8:00 PM",
                    Location = "Vogue Hall, New York, NY",
                    Price = 110m,
                    Color = "#9c27b0",
                    MaxTickets = 300,
                    SoldTickets = 267
                },
                new Event
                {
                    Id = 4,
                    Title = "Global Wellness Summit",
                    Description = "Internationell konferens om hälsa och välmående",
                    Category = "Health & Wellness",
                    Date = new DateTime(2024, 5, 5),
                    Time = "9:00 AM",
                    Location = "Wellness Arena, Miami, FL",
                    Price = 85m,
                    Color = "#FF9800",
                    MaxTickets = 400,
                    SoldTickets = 156
                },
                new Event
                {
                    Id = 5,
                    Title = "Tech Future Expo",
                    Description = "Teknikens framtid demonstreras live",
                    Category = "Technology",
                    Date = new DateTime(2024, 6, 1),
                    Time = "10:00 AM",
                    Location = "Silicon Valley, Bay Area, CA",
                    Price = 120m,
                    Color = "#2196F3",
                    MaxTickets = 600,
                    SoldTickets = 334
                },
                new Event
                {
                    Id = 6,
                    Title = "Culinary Delights Festival",
                    Description = "Matfestival med världens bästa kockar",
                    Category = "Food & Culinary",
                    Date = new DateTime(2024, 5, 25),
                    Time = "11:00 AM",
                    Location = "Gourmet Plaza, San Francisco, CA",
                    Price = 55m,
                    Color = "#795548",
                    MaxTickets = 250,
                    SoldTickets = 89
                }
            );
        }
    }
}