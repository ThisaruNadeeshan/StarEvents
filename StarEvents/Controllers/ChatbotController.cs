using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using StarEvents.Helpers;
using StarEvents.Models;

namespace StarEvents.Controllers
{
    [Authorize]
    public class ChatbotController : Controller
    {
        private StarEventsDBEntities db = new StarEventsDBEntities();

        // GET: Chatbot - Display chat interface page
        public ActionResult Index()
        {
            return View();
        }

        // POST: Chatbot/Chat - Handle chat messages
        [HttpPost]
        public JsonResult Chat(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return Json(new { success = false, response = "Please enter a message." });
            }

            // Validate message length
            if (message.Length > 1000)
            {
                return Json(new { success = false, response = "Message is too long. Please keep it under 1000 characters." });
            }

            try
            {
                // Get current user (AllowAnonymous attribute means User.Identity might be null)
                var userEmail = User.Identity?.IsAuthenticated == true ? User.Identity.Name : null;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Json(new { success = false, response = "Please log in to use the chatbot." });
                }

                var user = db.Users.FirstOrDefault(u => u.Email == userEmail && u.IsActive);
                if (user == null)
                {
                    return Json(new { success = false, response = "User not found. Please log in again." });
                }

                // Build context with booking history and available events
                var systemPrompt = BuildSystemPrompt(user.UserId, user.Role);

                System.Diagnostics.Debug.WriteLine($"[Chatbot] Processing message for user: {userEmail}");
                System.Diagnostics.Debug.WriteLine($"[Chatbot] Message length: {message.Length}");

                // Call OpenAI service - Use GetAwaiter().GetResult() to avoid deadlock in ASP.NET Framework
                // ConfigureAwait(false) is used in the service to prevent context capture
                var response = OpenAIService.ChatAsync(systemPrompt, message)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                System.Diagnostics.Debug.WriteLine($"[Chatbot] Response received, length: {response?.Length ?? 0}");

                return Json(new { success = true, response = response });
            }
            catch (AggregateException ae)
            {
                // Unwrap aggregate exception to get the real exception
                var innerEx = ae.InnerException ?? ae;
                System.Diagnostics.Debug.WriteLine($"[Chatbot] AggregateException: {innerEx.GetType().Name} - {innerEx.Message}");
                return Json(new { success = false, response = $"Sorry, I encountered an error: {innerEx.Message}. Please try again." });
            }
            catch (Exception ex)
            {
                // Return user-friendly error message
                System.Diagnostics.Debug.WriteLine($"[Chatbot] Exception: {ex.GetType().Name} - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[Chatbot] Stack trace: {ex.StackTrace}");
                return Json(new { success = false, response = $"Sorry, I encountered an error: {ex.Message}. Please try again." });
            }
        }

        /// <summary>
        /// Builds the system prompt with user context, booking history, and available events
        /// </summary>
        private string BuildSystemPrompt(int userId, string userRole)
        {
            var sb = new StringBuilder();
            
            // Bot role and capabilities
            sb.AppendLine("You are a helpful AI assistant for StarEvents, an event ticketing platform. ");
            sb.AppendLine("You can help users with:");
            sb.AppendLine("- Finding events and checking seat availability");
            sb.AppendLine("- Viewing their booking history");
            sb.AppendLine("- Answering questions about events, tickets, and bookings");
            sb.AppendLine("- Providing general information about the platform");
            sb.AppendLine();
            sb.AppendLine("Be friendly, concise, and helpful. Always provide accurate information based on the context provided below.");
            sb.AppendLine("If you don't have specific information in the context, say so rather than making up details.");
            sb.AppendLine();

            // Add user's booking history (for Customer role only)
            if (string.Equals(userRole, "Customer", StringComparison.OrdinalIgnoreCase))
            {
                var bookings = db.Bookings
                    .Include(b => b.Event)
                    .Include(b => b.Event.Venue)
                    .Where(b => b.CustomerId == userId)
                    .OrderByDescending(b => b.BookingDate)
                    .Take(10)
                    .ToList();

                if (bookings.Any())
                {
                    sb.AppendLine("USER'S RECENT BOOKING HISTORY:");
                    sb.AppendLine("==============================");
                    foreach (var booking in bookings)
                    {
                        var venueName = booking.Event?.Venue?.VenueName ?? "N/A";
                        var bookingDate = booking.BookingDate?.ToString("MMM dd, yyyy") ?? "N/A";
                        sb.AppendLine($"- Booking #{booking.BookingCode}: {booking.Event?.Title ?? "N/A"} on {bookingDate} at {venueName}. ");
                        sb.AppendLine($"  Quantity: {booking.Quantity} ticket(s), Status: {booking.Status}, Total Amount: {booking.TotalAmount:C}");
                    }
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendLine("USER'S BOOKING HISTORY:");
                    sb.AppendLine("This user has no previous bookings yet.");
                    sb.AppendLine();
                }
            }

            // Add available events with seat information
            var availableEvents = db.Events
                .Include(e => e.Venue)
                .Include(e => e.SeatCategories)
                .Where(e => e.IsActive == true && 
                           e.IsPublished == true && 
                           e.EventDate >= DateTime.Now)
                .OrderBy(e => e.EventDate)
                .Take(20)
                .ToList();

            if (availableEvents.Any())
            {
                sb.AppendLine("CURRENTLY AVAILABLE EVENTS WITH SEAT INFORMATION:");
                sb.AppendLine("==================================================");
                foreach (var evt in availableEvents)
                {
                    var venueName = evt.Venue?.VenueName ?? "N/A";
                    var totalSeats = evt.SeatCategories?.Sum(sc => sc.TotalSeats) ?? 0;
                    var availableSeats = evt.SeatCategories?.Sum(sc => sc.AvailableSeats) ?? 0;
                    var minPrice = evt.SeatCategories != null && evt.SeatCategories.Any() 
                        ? evt.SeatCategories.Min(sc => sc.Price) 
                        : 0;

                    sb.AppendLine($"- Event: {evt.Title}");
                    sb.AppendLine($"  Date: {evt.EventDate:MMM dd, yyyy HH:mm}");
                    sb.AppendLine($"  Category: {evt.Category ?? "N/A"}");
                    sb.AppendLine($"  Venue: {venueName}");
                    sb.AppendLine($"  Location: {evt.Location ?? "N/A"}");
                    sb.AppendLine($"  Seats Available: {availableSeats} out of {totalSeats} total");
                    if (minPrice > 0)
                    {
                        sb.AppendLine($"  Starting Price: {minPrice:C}");
                    }
                    sb.AppendLine();
                }
            }
            else
            {
                sb.AppendLine("CURRENTLY AVAILABLE EVENTS:");
                sb.AppendLine("No upcoming events are currently available.");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
