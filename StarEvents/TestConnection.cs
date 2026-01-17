// Temporary connection test utility
// Add this to a controller action to test the database connection
/*
using StarEvents.Models;
using System;
using System.Web.Mvc;

public class TestController : Controller
{
    public ActionResult TestDb()
    {
        try
        {
            using (var db = new StarEventsDBEntities())
            {
                // Test connection by trying to query Users table
                var userCount = db.Users.Count();
                return Content($"Connection successful! Users table has {userCount} records.");
            }
        }
        catch (Exception ex)
        {
            return Content($"Connection failed: {ex.Message}\n\nInner Exception: {ex.InnerException?.Message}\n\nStack Trace: {ex.StackTrace}");
        }
    }
}
*/
