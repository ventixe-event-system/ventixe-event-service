// Models/Event.cs
namespace ventixe_event_service.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Time { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = "Active";
        public string Color { get; set; } = "#e91e63";
        public int MaxTickets { get; set; } = 100;
        public int SoldTickets { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CreateEventRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Time { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int MaxTickets { get; set; } = 100;
    }

    public class BookingRequest
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public int Tickets { get; set; } = 1;
    }

    public class Booking
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public int Tickets { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Confirmed";

        // Navigation property
        public Event? Event { get; set; }
    }
}