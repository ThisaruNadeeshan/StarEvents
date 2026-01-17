using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Npgsql;
using StarEvents.Models;
using StarEvents.ViewModels;

namespace StarEvents.Controllers
{
    public class HomeController : Controller
    {
        private StarEventsDBEntities db = new StarEventsDBEntities();

        // Helper to infer promotion type, icon, and color from discount name
        private string GetPromotionType(string name)
        {
            name = (name ?? "").ToLower();
            if (name.Contains("vip") || name.Contains("premium") || name.Contains("gold"))
                return "vip";
            if (name.Contains("early") || name.Contains("advance") || name.Contains("pre"))
                return "earlybird";
            if (name.Contains("flash") || name.Contains("today") || name.Contains("urgent"))
                return "flashsale";
            if (name.Contains("group") || name.Contains("team"))
                return "group";
            return "discount";
        }

        private (string icon, string color) GetIconAndColor(string type)
        {
            switch (type)
            {
                case "vip": return ("💎", "#8e24aa");
                case "earlybird": return ("🔥", "#388e3c");
                case "flashsale": return ("⚡", "#ff7043");
                case "group": return ("👥", "#1976d2");
                default: return ("🎁", "#ff7043");
            }
        }

        // Helper method to get featured events with starting price from SeatCategories and mapped promotions
        private List<EventCardViewModel> GetFeaturedEvents()
        {
            try
            {
                var featuredEvents = db.Events
                    .Include("Venue")
                    .Include("EventDiscounts")
                    .Where(e => e.IsActive == true && e.IsPublished == true && e.EventDate >= DateTime.Now)
                    .OrderBy(e => e.EventDate)
                    .Take(4)
                    .ToList()
                .Select(e => new EventCardViewModel
                {
                    EventId = e.EventId,
                    Title = e.Title,
                    EventDate = e.EventDate,
                    Location = e.Location,
                    Description = e.Description,
                    ImageUrl = e.ImageUrl,
                    Category = e.Category,
                    VenueName = e.Venue != null ? e.Venue.VenueName : "",
                    StartingPrice = e.SeatCategories != null && e.SeatCategories.Any()
                        ? e.SeatCategories.Min(sc => (decimal?)sc.Price) ?? 0
                        : 0,
                    Promotions = e.EventDiscounts != null
                        ? e.EventDiscounts
                            .Where(d => d.IsActive &&
                                        (d.StartDate == null || d.StartDate <= DateTime.Now) &&
                                        (d.EndDate == null || d.EndDate >= DateTime.Now))
                            .Select(d => {
                                var promoType = GetPromotionType(d.DiscountName);
                                var (icon, color) = GetIconAndColor(promoType);
                                // Handle nullable DateTime
                                var startDate = d.StartDate;
                                var endDate = d.EndDate;
                                string description = $"Valid until {endDate:MMM dd}";
                                return new PromotionViewModel
                                {
                                    PromotionId = d.DiscountId,
                                    Title = d.DiscountName,
                                    DiscountValue = d.DiscountPercent ?? 0,
                                    DiscountType = "Percentage", // Or "Fixed" if you have that logic
                                    StartDate = startDate,
                                    EndDate = endDate,
                                    IsActive = d.IsActive,
                                    Label = d.DiscountName + (d.DiscountPercent.HasValue ? $": {d.DiscountPercent}% off" : ""),
                                    Type = promoType,
                                    Icon = icon,
                                    Description = description,
                                    Color = color
                                };
                            }).ToList()
                        : new List<PromotionViewModel>()
                })
                .ToList();

                return featuredEvents;
            }
            catch (Exception ex)
            {
                // Log the full exception details for debugging
                System.Diagnostics.Debug.WriteLine($"Database Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                
                // Return empty list on error to prevent page crash
                return new List<EventCardViewModel>();
            }
        }

        public ActionResult Index()
        {
            try
            {
                var model = new HomeViewModel
                {
                    FeaturedEvents = GetFeaturedEvents()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                // Return detailed error information for debugging
                ViewBag.Error = $"Database Connection Error: {ex.Message}";
                if (ex.InnerException != null)
                {
                    ViewBag.InnerError = ex.InnerException.Message;
                    ViewBag.InnerStackTrace = ex.InnerException.StackTrace;
                }
                return View(new HomeViewModel { FeaturedEvents = new List<EventCardViewModel>() });
            }
        }
        
        // Test connection endpoint
        public ActionResult TestConnection()
        {
            var results = new System.Text.StringBuilder();
            results.AppendLine("<h2>Database Connection Test</h2>");
            
            // Test 1: Raw Npgsql connection
            results.AppendLine("<h3>Test 1: Raw Npgsql Connection</h3>");
            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["StarEventsDBEntities"].ConnectionString))
                {
                    conn.Open();
                    results.AppendLine("✅ Raw Npgsql connection successful!<br/>");
                    
                    using (var cmd = new Npgsql.NpgsqlCommand("SELECT version();", conn))
                    {
                        var version = cmd.ExecuteScalar().ToString();
                        results.AppendLine($"PostgreSQL Version: {version}<br/>");
                    }
                    
                    using (var cmd = new Npgsql.NpgsqlCommand("SELECT COUNT(*) FROM \"public\".\"Users\";", conn))
                    {
                        var userCount = cmd.ExecuteScalar();
                        results.AppendLine($"Users table count: {userCount}<br/>");
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                results.AppendLine($"❌ Raw Npgsql connection failed!<br/>");
                results.AppendLine($"Error: {ex.Message}<br/>");
                if (ex.InnerException != null)
                {
                    results.AppendLine($"Inner: {ex.InnerException.Message}<br/>");
                    // Check for LoaderExceptions
                    var reflectionEx = ex.InnerException as System.Reflection.ReflectionTypeLoadException;
                    if (reflectionEx != null && reflectionEx.LoaderExceptions != null)
                    {
                        results.AppendLine("<strong>Loader Exceptions:</strong><br/>");
                        foreach (var loaderEx in reflectionEx.LoaderExceptions)
                        {
                            results.AppendLine($"  - {loaderEx.Message}<br/>");
                        }
                    }
                }
            }
            
            results.AppendLine("<hr/>");
            
            // Test 2: Entity Framework connection
            results.AppendLine("<h3>Test 2: Entity Framework Connection</h3>");
            try
            {
                // First, try to just create the context and check if connection can be opened
                using (var testDb = new StarEventsDBEntities())
                {
                    var connection = testDb.Database.Connection;
                    results.AppendLine($"Connection string: {connection.ConnectionString}<br/>");
                    results.AppendLine($"Provider: {connection.GetType().FullName}<br/>");
                    
                    // Try to open the connection manually
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                        results.AppendLine("✅ EF connection opened successfully!<br/>");
                        
                        using (var cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = "SELECT COUNT(*) FROM \"public\".\"Users\"";
                            var userCount = cmd.ExecuteScalar();
                            results.AppendLine($"Users count via EF: {userCount}<br/>");
                        }
                        connection.Close();
                    }
                    
                    // Now try EF query
                    var userCountEF = testDb.Users.Count();
                    results.AppendLine($"✅ EF query successful! Found {userCountEF} users.<br/>");
                }
            }
            catch (Exception ex)
            {
                results.AppendLine($"❌ Entity Framework connection failed!<br/>");
                results.AppendLine($"Error: {ex.Message}<br/>");
                if (ex.InnerException != null)
                {
                    results.AppendLine($"Inner Exception: {ex.InnerException.Message}<br/>");
                    
                    // Check for ReflectionTypeLoadException to see what types failed to load
                    var reflectionEx = ex.InnerException as System.Reflection.ReflectionTypeLoadException;
                    if (reflectionEx != null && reflectionEx.LoaderExceptions != null)
                    {
                        results.AppendLine("<strong>Loader Exceptions (Failed Type Loads):</strong><br/>");
                        foreach (var loaderEx in reflectionEx.LoaderExceptions)
                        {
                            if (loaderEx != null)
                            {
                                results.AppendLine($"  - {loaderEx.GetType().Name}: {loaderEx.Message}<br/>");
                                var fileEx = loaderEx as System.IO.FileNotFoundException;
                                if (fileEx != null)
                                {
                                    results.AppendLine($"    File: {fileEx.FileName}<br/>");
                                }
                            }
                        }
                    }
                    
                    if (ex.InnerException.StackTrace != null)
                    {
                        results.AppendLine($"Inner Stack: {ex.InnerException.StackTrace.Replace("\n", "<br/>")}<br/>");
                    }
                }
                results.AppendLine($"Stack Trace: {ex.StackTrace.Replace("\n", "<br/>")}<br/>");
            }
            
            results.AppendLine("<hr/>");
            results.AppendLine("<h3>Provider Information</h3>");
            try
            {
                var providers = System.Data.Common.DbProviderFactories.GetFactoryClasses();
                results.AppendLine("Installed Providers:<br/>");
                results.AppendLine("<ul>");
                foreach (System.Data.DataRow row in providers.Rows)
                {
                    results.AppendLine($"<li>{row["InvariantName"]} - {row["Name"]}</li>");
                }
                results.AppendLine("</ul>");
            }
            catch (Exception ex)
            {
                results.AppendLine($"Error getting providers: {ex.Message}<br/>");
            }
            
            return Content(results.ToString());
        }

        public ActionResult About()
        {
            ViewBag.Message = "About";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }

    public class HomeViewModel
    {
        public List<EventCardViewModel> FeaturedEvents { get; set; }
    }
}