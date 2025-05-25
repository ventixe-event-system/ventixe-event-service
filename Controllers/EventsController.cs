// Controllers/EventsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ventixe_event_service.Data;
using ventixe_event_service.Models;

namespace ventixe_event_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventDbContext _context;

        public EventsController(EventDbContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents([FromQuery] string? category = null)
        {
            try
            {
                var query = _context.Events.AsQueryable();

                if (!string.IsNullOrEmpty(category) && category != "All Category")
                {
                    query = query.Where(e => e.Category == category);
                }

                var events = await query.OrderBy(e => e.Date).ToListAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Server error: {ex.Message}" });
            }
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            try
            {
                var eventItem = await _context.Events.FindAsync(id);

                if (eventItem == null)
                {
                    return NotFound(new { message = "Event hittades inte" });
                }

                return Ok(eventItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Server error: {ex.Message}" });
            }
        }

        // POST: api/Events/book
        [HttpPost("book")]
        public async Task<ActionResult> BookEvent([FromBody] BookingRequest request)
        {
            try
            {
                var eventItem = await _context.Events.FindAsync(request.EventId);

                if (eventItem == null)
                {
                    return NotFound(new { message = "Event hittades inte" });
                }

                if (eventItem.SoldTickets + request.Tickets > eventItem.MaxTickets)
                {
                    return BadRequest(new { message = "Inte tillräckligt med biljetter kvar" });
                }

                // Skapa bokning
                var booking = new Booking
                {
                    EventId = request.EventId,
                    UserId = request.UserId,
                    Tickets = request.Tickets,
                    TotalPrice = eventItem.Price * request.Tickets
                };

                _context.Bookings.Add(booking);

                // Uppdatera sålda biljetter
                eventItem.SoldTickets += request.Tickets;
                eventItem.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = $"Bokning lyckades! {request.Tickets} biljetter för {eventItem.Title}",
                    booking = booking,
                    totalPrice = booking.TotalPrice
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Server error: {ex.Message}" });
            }
        }

        // GET: api/Events/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            try
            {
                var categories = await _context.Events
                    .Select(e => e.Category)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Server error: {ex.Message}" });
            }
        }

        // GET: api/Events/stats
        [HttpGet("stats")]
        public async Task<ActionResult> GetEventStats()
        {
            try
            {
                var totalEvents = await _context.Events.CountAsync();
                var totalBookings = await _context.Bookings.CountAsync();
                var totalTicketsSold = await _context.Bookings.SumAsync(b => b.Tickets);
                var totalRevenue = await _context.Bookings.SumAsync(b => b.TotalPrice);

                var categoryStats = await _context.Events
                    .GroupBy(e => e.Category)
                    .Select(g => new {
                        Category = g.Key,
                        Count = g.Count(),
                        Percentage = Math.Round((double)g.Count() / totalEvents * 100, 1)
                    })
                    .OrderByDescending(x => x.Count)
                    .ToListAsync();

                return Ok(new
                {
                    totalEvents,
                    totalBookings,
                    totalTicketsSold,
                    totalRevenue,
                    categoryStats
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Server error: {ex.Message}" });
            }
        }

        // Test endpoint
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Event Service API fungerar!", timestamp = DateTime.UtcNow });
        }
    }
}