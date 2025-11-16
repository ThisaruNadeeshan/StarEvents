using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using StarEvents.Models;

namespace StarEvents.Helpers
{
    public static class EmailService
    {
        private static readonly string _apiKey;
        private static readonly string _fromEmail;

        static EmailService()
        {
            _apiKey = ConfigurationManager.AppSettings["Resend.ApiKey"];
            _fromEmail = ConfigurationManager.AppSettings["Resend.FromEmail"];
        }

        /// <summary>
        /// Send a simple welcome email after registration.
        /// </summary>
        public static void SendWelcomeEmail(string toEmail, string userName, string role)
        {
            if (string.IsNullOrWhiteSpace(_apiKey) || string.IsNullOrWhiteSpace(_fromEmail))
                return; // silently ignore if not configured

            var subject = "Welcome to StarEvents";
            var body = new StringBuilder();
            body.Append("<h1>Welcome to StarEvents 🎟</h1>");
            body.AppendFormat("<p>Hi {0},</p>", HttpUtility.HtmlEncode(userName));
            body.Append("<p>Your account has been created successfully.</p>");
            body.AppendFormat("<p>Role: <strong>{0}</strong></p>", HttpUtility.HtmlEncode(role));
            body.Append("<p>You can now sign in and start using the platform.</p>");
            body.Append("<p><a href=\"https://thisarunadeeshan.com\" target=\"_blank\">Go to StarEvents</a></p>");
            body.Append("<p>Best regards,<br/>StarEvents Team</p>");

            SendEmail(toEmail, subject, body.ToString());
        }

        /// <summary>
        /// Send booking confirmation with basic details.
        /// </summary>
        public static void SendBookingConfirmation(string toEmail, Booking booking, Event evt, IEnumerable<Ticket> tickets)
        {
            if (string.IsNullOrWhiteSpace(_apiKey) || string.IsNullOrWhiteSpace(_fromEmail))
                return;

            if (booking == null || evt == null) return;

            var subject = $"Your booking for {evt.Title}";
            var body = new StringBuilder();

            body.Append("<h1>Booking Confirmed ✅</h1>");
            body.Append("<p>Thank you for your purchase. Here are your booking details:</p>");

            body.Append("<h3>Event</h3>");
            body.AppendFormat("<p><strong>{0}</strong><br/>Date: {1:yyyy-MM-dd HH:mm}<br/>Location: {2}</p>",
                HttpUtility.HtmlEncode(evt.Title),
                evt.EventDate,
                HttpUtility.HtmlEncode(evt.Location));

            body.Append("<h3>Booking</h3>");
            body.AppendFormat("<p>Booking Code: <strong>{0}</strong><br/>Quantity: {1}<br/>Total Amount: {2:C}</p>",
                HttpUtility.HtmlEncode(booking.BookingCode),
                booking.Quantity,
                booking.TotalAmount);

            if (tickets != null)
            {
                body.Append("<h3>Tickets</h3><ul>");
                foreach (var t in tickets)
                {
                    body.AppendFormat("<li>Ticket Code: {0}</li>",
                        HttpUtility.HtmlEncode(t.TicketCode));
                }
                body.Append("</ul>");
            }

            body.Append("<p>You can also view your e-tickets in the My Bookings section.</p>");
            body.Append("<p>Best regards,<br/>StarEvents Team</p>");

            SendEmail(toEmail, subject, body.ToString());
        }

        private static void SendEmail(string toEmail, string subject, string htmlBody)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.resend.com/");
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _apiKey);

                    var payload = new
                    {
                        from = _fromEmail,
                        to = new[] { toEmail },
                        subject = subject,
                        html = htmlBody
                    };

                    var json = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Fire-and-forget style; we don't want email issues to break the app
                    var response = client.PostAsync("emails", content).Result;
                    // Optionally, could log failures here if needed
                }
            }
            catch
            {
                // Swallow exceptions; email failures should not crash the site.
            }
        }
    }
}


