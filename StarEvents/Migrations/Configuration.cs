namespace StarEvents.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using StarEvents.Models;
    using StarEvents.Helpers;

    internal sealed class Configuration : DbMigrationsConfiguration<StarEvents.Models.StarEventsDBEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StarEvents.Models.StarEventsDBEntities context)
        {
            // Admin Account
            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    Username = "admin",
                    Email = "admin@starevents.com",
                    PasswordHash = PasswordHelper.HashPassword("Admin@123"),
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                });

            context.SaveChanges();

            var adminUserId = context.Users.First(u => u.Email == "admin@starevents.com").UserId;

            context.Admins.AddOrUpdate(
                a => a.AdminId,
                new Admin
                {
                    AdminId = adminUserId,
                    CreatedBy = "System",
                    Notes = "Default administrator account created during database seeding"
                });

            // Organizers (2-3)
            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    Username = "musicfest_org",
                    Email = "organizer1@musicfest.com",
                    PasswordHash = PasswordHelper.HashPassword("Org@123"),
                    Role = "Organizer",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-5)
                });

            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    Username = "concertpromo",
                    Email = "organizer2@concerts.com",
                    PasswordHash = PasswordHelper.HashPassword("Org@123"),
                    Role = "Organizer",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-4)
                });

            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    Username = "sportsevents",
                    Email = "organizer3@sports.com",
                    PasswordHash = PasswordHelper.HashPassword("Org@123"),
                    Role = "Organizer",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-3)
                });

            context.SaveChanges();

            var org1Id = context.Users.First(u => u.Email == "organizer1@musicfest.com").UserId;
            var org2Id = context.Users.First(u => u.Email == "organizer2@concerts.com").UserId;
            var org3Id = context.Users.First(u => u.Email == "organizer3@sports.com").UserId;

            context.OrganizerProfiles.AddOrUpdate(
                o => o.OrganizerId,
                new OrganizerProfile
                {
                    OrganizerId = org1Id,
                    OrganizationName = "Music Festival Organizers",
                    ContactPerson = "John Smith",
                    PhoneNumber = "+1-555-0101",
                    Address = "123 Music Street, Los Angeles, CA 90001",
                    Description = "Leading music festival organizers with 10+ years of experience",
                    ProfilePhoto = null
                },
                new OrganizerProfile
                {
                    OrganizerId = org2Id,
                    OrganizationName = "Concert Promoters Ltd",
                    ContactPerson = "Sarah Johnson",
                    PhoneNumber = "+1-555-0102",
                    Address = "456 Concert Avenue, New York, NY 10001",
                    Description = "Premium concert promotion and event management",
                    ProfilePhoto = null
                },
                new OrganizerProfile
                {
                    OrganizerId = org3Id,
                    OrganizationName = "Sports Events Co",
                    ContactPerson = "Mike Davis",
                    PhoneNumber = "+1-555-0103",
                    Address = "789 Sports Boulevard, Chicago, IL 60601",
                    Description = "Professional sports event management and ticketing",
                    ProfilePhoto = null
                });

            // Customers (5-6)
            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    Username = "alice_williams",
                    Email = "alice@example.com",
                    PasswordHash = PasswordHelper.HashPassword("Customer@123"),
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-4)
                });

            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    Username = "bob_martin",
                    Email = "bob@example.com",
                    PasswordHash = PasswordHelper.HashPassword("Customer@123"),
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-3)
                });

            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    Username = "charlie_brown",
                    Email = "charlie@example.com",
                    PasswordHash = PasswordHelper.HashPassword("Customer@123"),
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-3)
                });

            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    Username = "diana_prince",
                    Email = "diana@example.com",
                    PasswordHash = PasswordHelper.HashPassword("Customer@123"),
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-2)
                });

            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    Username = "edward_norton",
                    Email = "edward@example.com",
                    PasswordHash = PasswordHelper.HashPassword("Customer@123"),
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-2)
                });

            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    Username = "fiona_green",
                    Email = "fiona@example.com",
                    PasswordHash = PasswordHelper.HashPassword("Customer@123"),
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-1)
                });

            context.SaveChanges();

            var cust1Id = context.Users.First(u => u.Email == "alice@example.com").UserId;
            var cust2Id = context.Users.First(u => u.Email == "bob@example.com").UserId;
            var cust3Id = context.Users.First(u => u.Email == "charlie@example.com").UserId;
            var cust4Id = context.Users.First(u => u.Email == "diana@example.com").UserId;
            var cust5Id = context.Users.First(u => u.Email == "edward@example.com").UserId;
            var cust6Id = context.Users.First(u => u.Email == "fiona@example.com").UserId;

            context.CustomerProfiles.AddOrUpdate(
                c => c.CustomerId,
                new CustomerProfile
                {
                    CustomerId = cust1Id,
                    FullName = "Alice Williams",
                    PhoneNumber = "+1-555-1001",
                    Address = "100 Main Street, Boston, MA 02101",
                    LoyaltyPoints = 250,
                    ProfilePhoto = null,
                    DateOfBirth = new DateTime(1990, 5, 15),
                    Gender = "Female"
                },
                new CustomerProfile
                {
                    CustomerId = cust2Id,
                    FullName = "Bob Martin",
                    PhoneNumber = "+1-555-1002",
                    Address = "200 Oak Avenue, Seattle, WA 98101",
                    LoyaltyPoints = 180,
                    ProfilePhoto = null,
                    DateOfBirth = new DateTime(1985, 8, 22),
                    Gender = "Male"
                },
                new CustomerProfile
                {
                    CustomerId = cust3Id,
                    FullName = "Charlie Brown",
                    PhoneNumber = "+1-555-1003",
                    Address = "300 Pine Road, Miami, FL 33101",
                    LoyaltyPoints = 320,
                    ProfilePhoto = null,
                    DateOfBirth = new DateTime(1992, 3, 10),
                    Gender = "Male"
                },
                new CustomerProfile
                {
                    CustomerId = cust4Id,
                    FullName = "Diana Prince",
                    PhoneNumber = "+1-555-1004",
                    Address = "400 Elm Street, San Francisco, CA 94101",
                    LoyaltyPoints = 450,
                    ProfilePhoto = null,
                    DateOfBirth = new DateTime(1988, 11, 28),
                    Gender = "Female"
                },
                new CustomerProfile
                {
                    CustomerId = cust5Id,
                    FullName = "Edward Norton",
                    PhoneNumber = "+1-555-1005",
                    Address = "500 Maple Drive, Austin, TX 78701",
                    LoyaltyPoints = 95,
                    ProfilePhoto = null,
                    DateOfBirth = new DateTime(1995, 7, 5),
                    Gender = "Male"
                },
                new CustomerProfile
                {
                    CustomerId = cust6Id,
                    FullName = "Fiona Green",
                    PhoneNumber = "+1-555-1006",
                    Address = "600 Cedar Lane, Denver, CO 80201",
                    LoyaltyPoints = 120,
                    ProfilePhoto = null,
                    DateOfBirth = new DateTime(1991, 12, 18),
                    Gender = "Female"
                });

            // Customer Cards
            context.CustomerCards.AddOrUpdate(
                c => new { c.CustomerId, c.CardNumber },
                new CustomerCard
                {
                    CustomerId = cust1Id,
                    CardNumber = "4111111111111111",
                    CardHolder = "Alice Williams",
                    Expiry = "12/25",
                    CVV = "123",
                    IsDefault = true
                },
                new CustomerCard
                {
                    CustomerId = cust2Id,
                    CardNumber = "4222222222222222",
                    CardHolder = "Bob Martin",
                    Expiry = "06/26",
                    CVV = "456",
                    IsDefault = true
                },
                new CustomerCard
                {
                    CustomerId = cust3Id,
                    CardNumber = "4333333333333333",
                    CardHolder = "Charlie Brown",
                    Expiry = "09/25",
                    CVV = "789",
                    IsDefault = true
                });

            // Venues (3-4)
            context.Venues.AddOrUpdate(
                v => v.VenueName,
                new Venue
                {
                    VenueName = "Grand Concert Hall",
                    Address = "100 Entertainment Boulevard",
                    City = "Los Angeles",
                    Capacity = 5000
                });

            context.Venues.AddOrUpdate(
                v => v.VenueName,
                new Venue
                {
                    VenueName = "Olympic Stadium",
                    Address = "200 Sports Arena Drive",
                    City = "New York",
                    Capacity = 8000
                });

            context.Venues.AddOrUpdate(
                v => v.VenueName,
                new Venue
                {
                    VenueName = "Metro Convention Center",
                    Address = "300 Conference Way",
                    City = "Chicago",
                    Capacity = 3000
                });

            context.Venues.AddOrUpdate(
                v => v.VenueName,
                new Venue
                {
                    VenueName = "Riverside Amphitheater",
                    Address = "400 Riverside Park",
                    City = "Seattle",
                    Capacity = 6000
                });

            context.SaveChanges();

            var venue1Id = context.Venues.First(v => v.VenueName == "Grand Concert Hall").VenueId;
            var venue2Id = context.Venues.First(v => v.VenueName == "Olympic Stadium").VenueId;
            var venue3Id = context.Venues.First(v => v.VenueName == "Metro Convention Center").VenueId;
            var venue4Id = context.Venues.First(v => v.VenueName == "Riverside Amphitheater").VenueId;

            // Events (4-5)
            context.Events.AddOrUpdate(
                e => e.Title,
                new Event
                {
                    OrganizerId = org1Id,
                    Title = "Summer Music Festival 2024",
                    Category = "Music",
                    Description = "The biggest music festival of the year featuring top artists from around the world. Don't miss this amazing experience!",
                    EventDate = DateTime.Now.AddDays(30),
                    Location = "Grand Concert Hall, Los Angeles",
                    VenueId = venue1Id,
                    ImageUrl = null,
                    CreatedAt = DateTime.Now.AddMonths(-2),
                    UpdatedAt = DateTime.Now.AddMonths(-1),
                    IsActive = true,
                    IsPublished = true
                });

            context.Events.AddOrUpdate(
                e => e.Title,
                new Event
                {
                    OrganizerId = org2Id,
                    Title = "Rock Concert Series",
                    Category = "Music",
                    Description = "An electrifying rock concert featuring legendary bands and rising stars.",
                    EventDate = DateTime.Now.AddDays(45),
                    Location = "Olympic Stadium, New York",
                    VenueId = venue2Id,
                    ImageUrl = null,
                    CreatedAt = DateTime.Now.AddMonths(-1),
                    UpdatedAt = DateTime.Now.AddDays(-10),
                    IsActive = true,
                    IsPublished = true
                });

            context.Events.AddOrUpdate(
                e => e.Title,
                new Event
                {
                    OrganizerId = org3Id,
                    Title = "Championship Football Match",
                    Category = "Sports",
                    Description = "Watch the ultimate championship match between top teams.",
                    EventDate = DateTime.Now.AddDays(20),
                    Location = "Olympic Stadium, New York",
                    VenueId = venue2Id,
                    ImageUrl = null,
                    CreatedAt = DateTime.Now.AddMonths(-3),
                    UpdatedAt = DateTime.Now.AddDays(-5),
                    IsActive = true,
                    IsPublished = true
                });

            context.Events.AddOrUpdate(
                e => e.Title,
                new Event
                {
                    OrganizerId = org1Id,
                    Title = "Tech Conference 2024",
                    Category = "Conference",
                    Description = "Join industry leaders and innovators for cutting-edge technology discussions.",
                    EventDate = DateTime.Now.AddDays(60),
                    Location = "Metro Convention Center, Chicago",
                    VenueId = venue3Id,
                    ImageUrl = null,
                    CreatedAt = DateTime.Now.AddMonths(-1),
                    UpdatedAt = DateTime.Now.AddDays(-15),
                    IsActive = true,
                    IsPublished = true
                });

            context.Events.AddOrUpdate(
                e => e.Title,
                new Event
                {
                    OrganizerId = org2Id,
                    Title = "Jazz Night Under the Stars",
                    Category = "Music",
                    Description = "Experience smooth jazz in an open-air setting with delicious food and drinks.",
                    EventDate = DateTime.Now.AddDays(35),
                    Location = "Riverside Amphitheater, Seattle",
                    VenueId = venue4Id,
                    ImageUrl = null,
                    CreatedAt = DateTime.Now.AddDays(-20),
                    UpdatedAt = DateTime.Now.AddDays(-10),
                    IsActive = true,
                    IsPublished = true
                });

            context.SaveChanges();

            var event1Id = context.Events.First(e => e.Title == "Summer Music Festival 2024").EventId;
            var event2Id = context.Events.First(e => e.Title == "Rock Concert Series").EventId;
            var event3Id = context.Events.First(e => e.Title == "Championship Football Match").EventId;
            var event4Id = context.Events.First(e => e.Title == "Tech Conference 2024").EventId;
            var event5Id = context.Events.First(e => e.Title == "Jazz Night Under the Stars").EventId;

            // Seat Categories for Events
            context.SeatCategories.AddOrUpdate(
                sc => new { sc.EventId, sc.CategoryName },
                // Event 1 - Summer Music Festival
                new SeatCategory { EventId = event1Id, CategoryName = "VIP", Price = 299.99m, TotalSeats = 500, AvailableSeats = 420 },
                new SeatCategory { EventId = event1Id, CategoryName = "Standard", Price = 149.99m, TotalSeats = 3000, AvailableSeats = 2850 },
                new SeatCategory { EventId = event1Id, CategoryName = "Economy", Price = 79.99m, TotalSeats = 1500, AvailableSeats = 1450 },
                // Event 2 - Rock Concert
                new SeatCategory { EventId = event2Id, CategoryName = "Front Row", Price = 199.99m, TotalSeats = 200, AvailableSeats = 180 },
                new SeatCategory { EventId = event2Id, CategoryName = "General Admission", Price = 99.99m, TotalSeats = 7800, AvailableSeats = 7650 },
                // Event 3 - Football Match
                new SeatCategory { EventId = event3Id, CategoryName = "Premium", Price = 159.99m, TotalSeats = 2000, AvailableSeats = 1850 },
                new SeatCategory { EventId = event3Id, CategoryName = "Standard", Price = 89.99m, TotalSeats = 4000, AvailableSeats = 3900 },
                new SeatCategory { EventId = event3Id, CategoryName = "Student", Price = 49.99m, TotalSeats = 2000, AvailableSeats = 1950 },
                // Event 4 - Tech Conference
                new SeatCategory { EventId = event4Id, CategoryName = "VIP Pass", Price = 499.99m, TotalSeats = 300, AvailableSeats = 280 },
                new SeatCategory { EventId = event4Id, CategoryName = "Standard", Price = 199.99m, TotalSeats = 2000, AvailableSeats = 1950 },
                new SeatCategory { EventId = event4Id, CategoryName = "Student", Price = 99.99m, TotalSeats = 700, AvailableSeats = 690 },
                // Event 5 - Jazz Night
                new SeatCategory { EventId = event5Id, CategoryName = "Premium Table", Price = 149.99m, TotalSeats = 200, AvailableSeats = 185 },
                new SeatCategory { EventId = event5Id, CategoryName = "General Admission", Price = 79.99m, TotalSeats = 5800, AvailableSeats = 5700 }
            );

            // Event Discounts
            context.EventDiscounts.AddOrUpdate(
                ed => new { ed.EventId, ed.DiscountName },
                // Event 1 discounts
                new EventDiscount
                {
                    EventId = event1Id,
                    DiscountName = "Early Bird VIP",
                    DiscountType = "percent",
                    DiscountPercent = 15,
                    DiscountAmount = null,
                    SeatCategory = "VIP",
                    MaxUsage = 100,
                    Description = "15% off VIP tickets for early buyers",
                    StartDate = DateTime.Now.AddMonths(-1),
                    EndDate = DateTime.Now.AddDays(25),
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-1),
                    UpdatedAt = DateTime.Now.AddMonths(-1)
                },
                new EventDiscount
                {
                    EventId = event1Id,
                    DiscountName = "Group Discount",
                    DiscountType = "percent",
                    DiscountPercent = 10,
                    DiscountAmount = null,
                    SeatCategory = "Standard,Economy",
                    MaxUsage = 500,
                    Description = "10% off for groups of 5 or more",
                    StartDate = DateTime.Now.AddDays(-10),
                    EndDate = DateTime.Now.AddDays(28),
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-10),
                    UpdatedAt = DateTime.Now.AddDays(-10)
                },
                // Event 2 discounts
                new EventDiscount
                {
                    EventId = event2Id,
                    DiscountName = "Flash Sale",
                    DiscountType = "amount",
                    DiscountPercent = null,
                    DiscountAmount = 20,
                    SeatCategory = "General Admission",
                    MaxUsage = 300,
                    Description = "$20 off general admission tickets - limited time!",
                    StartDate = DateTime.Now.AddDays(-5),
                    EndDate = DateTime.Now.AddDays(40),
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-5),
                    UpdatedAt = DateTime.Now.AddDays(-5)
                },
                // Event 3 discounts
                new EventDiscount
                {
                    EventId = event3Id,
                    DiscountName = "Student Special",
                    DiscountType = "percent",
                    DiscountPercent = 20,
                    DiscountAmount = null,
                    SeatCategory = "Student",
                    MaxUsage = 500,
                    Description = "Extra 20% off student tickets",
                    StartDate = DateTime.Now.AddDays(-7),
                    EndDate = DateTime.Now.AddDays(18),
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-7),
                    UpdatedAt = DateTime.Now.AddDays(-7)
                }
            );

            context.SaveChanges();

            // Bookings with Payments and Tickets
            var booking1Date = DateTime.Now.AddDays(-15);
            var booking1Code = "BK" + DateTime.Now.Ticks.ToString().Substring(0, 8);
            context.Bookings.AddOrUpdate(
                b => b.BookingCode,
                new Booking
                {
                    EventId = event1Id,
                    CustomerId = cust1Id,
                    BookingCode = booking1Code,
                    BookingDate = booking1Date,
                    Quantity = 2,
                    TotalAmount = 299.98m,
                    Status = "Paid",
                    SeatCategoryId = context.SeatCategories.First(sc => sc.EventId == event1Id && sc.CategoryName == "VIP").SeatCategoryId
                });

            var booking2Date = DateTime.Now.AddDays(-10);
            var booking2Code = "BK" + (DateTime.Now.Ticks + 1).ToString().Substring(0, 8);
            context.Bookings.AddOrUpdate(
                b => b.BookingCode,
                new Booking
                {
                    EventId = event2Id,
                    CustomerId = cust2Id,
                    BookingCode = booking2Code,
                    BookingDate = booking2Date,
                    Quantity = 4,
                    TotalAmount = 319.96m,
                    Status = "Paid",
                    SeatCategoryId = context.SeatCategories.First(sc => sc.EventId == event2Id && sc.CategoryName == "General Admission").SeatCategoryId
                });

            var booking3Date = DateTime.Now.AddDays(-8);
            var booking3Code = "BK" + (DateTime.Now.Ticks + 2).ToString().Substring(0, 8);
            context.Bookings.AddOrUpdate(
                b => b.BookingCode,
                new Booking
                {
                    EventId = event3Id,
                    CustomerId = cust3Id,
                    BookingCode = booking3Code,
                    BookingDate = booking3Date,
                    Quantity = 1,
                    TotalAmount = 159.99m,
                    Status = "Paid",
                    SeatCategoryId = context.SeatCategories.First(sc => sc.EventId == event3Id && sc.CategoryName == "Premium").SeatCategoryId
                });

            var booking4Date = DateTime.Now.AddDays(-5);
            var booking4Code = "BK" + (DateTime.Now.Ticks + 3).ToString().Substring(0, 8);
            context.Bookings.AddOrUpdate(
                b => b.BookingCode,
                new Booking
                {
                    EventId = event1Id,
                    CustomerId = cust4Id,
                    BookingCode = booking4Code,
                    BookingDate = booking4Date,
                    Quantity = 3,
                    TotalAmount = 449.97m,
                    Status = "Paid",
                    SeatCategoryId = context.SeatCategories.First(sc => sc.EventId == event1Id && sc.CategoryName == "VIP").SeatCategoryId
                });

            var booking5Date = DateTime.Now.AddDays(-3);
            var booking5Code = "BK" + (DateTime.Now.Ticks + 4).ToString().Substring(0, 8);
            context.Bookings.AddOrUpdate(
                b => b.BookingCode,
                new Booking
                {
                    EventId = event4Id,
                    CustomerId = cust5Id,
                    BookingCode = booking5Code,
                    BookingDate = booking5Date,
                    Quantity = 1,
                    TotalAmount = 199.99m,
                    Status = "Paid",
                    SeatCategoryId = context.SeatCategories.First(sc => sc.EventId == event4Id && sc.CategoryName == "Standard").SeatCategoryId
                });

            var booking6Date = DateTime.Now.AddDays(-2);
            var booking6Code = "BK" + (DateTime.Now.Ticks + 5).ToString().Substring(0, 8);
            context.Bookings.AddOrUpdate(
                b => b.BookingCode,
                new Booking
                {
                    EventId = event2Id,
                    CustomerId = cust6Id,
                    BookingCode = booking6Code,
                    BookingDate = booking6Date,
                    Quantity = 2,
                    TotalAmount = 159.98m,
                    Status = "Paid",
                    SeatCategoryId = context.SeatCategories.First(sc => sc.EventId == event2Id && sc.CategoryName == "General Admission").SeatCategoryId
                });

            var booking7Date = DateTime.Now.AddDays(-1);
            var booking7Code = "BK" + (DateTime.Now.Ticks + 6).ToString().Substring(0, 8);
            context.Bookings.AddOrUpdate(
                b => b.BookingCode,
                new Booking
                {
                    EventId = event5Id,
                    CustomerId = cust1Id,
                    BookingCode = booking7Code,
                    BookingDate = booking7Date,
                    Quantity = 2,
                    TotalAmount = 299.98m,
                    Status = "Pending",
                    SeatCategoryId = context.SeatCategories.First(sc => sc.EventId == event5Id && sc.CategoryName == "Premium Table").SeatCategoryId
                });

            context.SaveChanges();

            // Get actual booking IDs from database using booking codes
            var actualBooking1 = context.Bookings.FirstOrDefault(b => b.BookingCode == booking1Code);
            var actualBooking2 = context.Bookings.FirstOrDefault(b => b.BookingCode == booking2Code);
            var actualBooking3 = context.Bookings.FirstOrDefault(b => b.BookingCode == booking3Code);
            var actualBooking4 = context.Bookings.FirstOrDefault(b => b.BookingCode == booking4Code);
            var actualBooking5 = context.Bookings.FirstOrDefault(b => b.BookingCode == booking5Code);
            var actualBooking6 = context.Bookings.FirstOrDefault(b => b.BookingCode == booking6Code);
            var actualBooking7 = context.Bookings.FirstOrDefault(b => b.BookingCode == booking7Code);

            // Skip if bookings weren't found
            if (actualBooking1 == null || actualBooking2 == null || actualBooking3 == null || 
                actualBooking4 == null || actualBooking5 == null || actualBooking6 == null || actualBooking7 == null)
                return;

            // Payments for paid bookings
            context.Payments.AddOrUpdate(
                p => p.PaymentReference,
                new Payment
                {
                    BookingId = actualBooking1.BookingId,
                    PaymentReference = "PAY" + actualBooking1.BookingId + "001",
                    PaymentMethod = "Credit Card",
                    Amount = 299.98m,
                    Status = "Paid",
                    PaidAt = booking1Date.AddMinutes(5)
                },
                new Payment
                {
                    BookingId = actualBooking2.BookingId,
                    PaymentReference = "PAY" + actualBooking2.BookingId + "002",
                    PaymentMethod = "Credit Card",
                    Amount = 319.96m,
                    Status = "Paid",
                    PaidAt = booking2Date.AddMinutes(3)
                },
                new Payment
                {
                    BookingId = actualBooking3.BookingId,
                    PaymentReference = "PAY" + actualBooking3.BookingId + "003",
                    PaymentMethod = "Debit Card",
                    Amount = 159.99m,
                    Status = "Paid",
                    PaidAt = booking3Date.AddMinutes(7)
                },
                new Payment
                {
                    BookingId = actualBooking4.BookingId,
                    PaymentReference = "PAY" + actualBooking4.BookingId + "004",
                    PaymentMethod = "Credit Card",
                    Amount = 449.97m,
                    Status = "Paid",
                    PaidAt = booking4Date.AddMinutes(4)
                },
                new Payment
                {
                    BookingId = actualBooking5.BookingId,
                    PaymentReference = "PAY" + actualBooking5.BookingId + "005",
                    PaymentMethod = "Credit Card",
                    Amount = 199.99m,
                    Status = "Paid",
                    PaidAt = booking5Date.AddMinutes(6)
                },
                new Payment
                {
                    BookingId = actualBooking6.BookingId,
                    PaymentReference = "PAY" + actualBooking6.BookingId + "006",
                    PaymentMethod = "Credit Card",
                    Amount = 159.98m,
                    Status = "Paid",
                    PaidAt = booking6Date.AddMinutes(5)
                }
            );

            context.SaveChanges();

            // Tickets
            var seatCat1 = context.SeatCategories.First(sc => sc.EventId == event1Id && sc.CategoryName == "VIP");
            var seatCat2 = context.SeatCategories.First(sc => sc.EventId == event2Id && sc.CategoryName == "General Admission");
            var seatCat3 = context.SeatCategories.First(sc => sc.EventId == event3Id && sc.CategoryName == "Premium");
            var seatCat4 = context.SeatCategories.First(sc => sc.EventId == event4Id && sc.CategoryName == "Standard");
            var seatCat5 = context.SeatCategories.First(sc => sc.EventId == event5Id && sc.CategoryName == "Premium Table");

            context.Tickets.AddOrUpdate(
                t => t.TicketCode,
                // Booking 1 - 2 tickets
                new Ticket { BookingId = actualBooking1.BookingId, SeatCategoryId = seatCat1.SeatCategoryId, TicketCode = "TKT" + (actualBooking1.BookingId * 10 + 1), QRCodePath = "/qr/ticket_" + (actualBooking1.BookingId * 10 + 1) + ".png", IsUsed = false, CreatedAt = booking1Date },
                new Ticket { BookingId = actualBooking1.BookingId, SeatCategoryId = seatCat1.SeatCategoryId, TicketCode = "TKT" + (actualBooking1.BookingId * 10 + 2), QRCodePath = "/qr/ticket_" + (actualBooking1.BookingId * 10 + 2) + ".png", IsUsed = false, CreatedAt = booking1Date },
                // Booking 2 - 4 tickets
                new Ticket { BookingId = actualBooking2.BookingId, SeatCategoryId = seatCat2.SeatCategoryId, TicketCode = "TKT" + (actualBooking2.BookingId * 10 + 1), QRCodePath = "/qr/ticket_" + (actualBooking2.BookingId * 10 + 1) + ".png", IsUsed = false, CreatedAt = booking2Date },
                new Ticket { BookingId = actualBooking2.BookingId, SeatCategoryId = seatCat2.SeatCategoryId, TicketCode = "TKT" + (actualBooking2.BookingId * 10 + 2), QRCodePath = "/qr/ticket_" + (actualBooking2.BookingId * 10 + 2) + ".png", IsUsed = false, CreatedAt = booking2Date },
                new Ticket { BookingId = actualBooking2.BookingId, SeatCategoryId = seatCat2.SeatCategoryId, TicketCode = "TKT" + (actualBooking2.BookingId * 10 + 3), QRCodePath = "/qr/ticket_" + (actualBooking2.BookingId * 10 + 3) + ".png", IsUsed = false, CreatedAt = booking2Date },
                new Ticket { BookingId = actualBooking2.BookingId, SeatCategoryId = seatCat2.SeatCategoryId, TicketCode = "TKT" + (actualBooking2.BookingId * 10 + 4), QRCodePath = "/qr/ticket_" + (actualBooking2.BookingId * 10 + 4) + ".png", IsUsed = false, CreatedAt = booking2Date },
                // Booking 3 - 1 ticket
                new Ticket { BookingId = actualBooking3.BookingId, SeatCategoryId = seatCat3.SeatCategoryId, TicketCode = "TKT" + (actualBooking3.BookingId * 10 + 1), QRCodePath = "/qr/ticket_" + (actualBooking3.BookingId * 10 + 1) + ".png", IsUsed = false, CreatedAt = booking3Date },
                // Booking 4 - 3 tickets
                new Ticket { BookingId = actualBooking4.BookingId, SeatCategoryId = seatCat1.SeatCategoryId, TicketCode = "TKT" + (actualBooking4.BookingId * 10 + 1), QRCodePath = "/qr/ticket_" + (actualBooking4.BookingId * 10 + 1) + ".png", IsUsed = false, CreatedAt = booking4Date },
                new Ticket { BookingId = actualBooking4.BookingId, SeatCategoryId = seatCat1.SeatCategoryId, TicketCode = "TKT" + (actualBooking4.BookingId * 10 + 2), QRCodePath = "/qr/ticket_" + (actualBooking4.BookingId * 10 + 2) + ".png", IsUsed = false, CreatedAt = booking4Date },
                new Ticket { BookingId = actualBooking4.BookingId, SeatCategoryId = seatCat1.SeatCategoryId, TicketCode = "TKT" + (actualBooking4.BookingId * 10 + 3), QRCodePath = "/qr/ticket_" + (actualBooking4.BookingId * 10 + 3) + ".png", IsUsed = false, CreatedAt = booking4Date },
                // Booking 5 - 1 ticket
                new Ticket { BookingId = actualBooking5.BookingId, SeatCategoryId = seatCat4.SeatCategoryId, TicketCode = "TKT" + (actualBooking5.BookingId * 10 + 1), QRCodePath = "/qr/ticket_" + (actualBooking5.BookingId * 10 + 1) + ".png", IsUsed = false, CreatedAt = booking5Date },
                // Booking 6 - 2 tickets
                new Ticket { BookingId = actualBooking6.BookingId, SeatCategoryId = seatCat2.SeatCategoryId, TicketCode = "TKT" + (actualBooking6.BookingId * 10 + 1), QRCodePath = "/qr/ticket_" + (actualBooking6.BookingId * 10 + 1) + ".png", IsUsed = false, CreatedAt = booking6Date },
                new Ticket { BookingId = actualBooking6.BookingId, SeatCategoryId = seatCat2.SeatCategoryId, TicketCode = "TKT" + (actualBooking6.BookingId * 10 + 2), QRCodePath = "/qr/ticket_" + (actualBooking6.BookingId * 10 + 2) + ".png", IsUsed = false, CreatedAt = booking6Date }
            );

            context.SaveChanges();

            // Loyalty Points
            context.LoyaltyPoints.AddOrUpdate(
                lp => new { lp.UserId, lp.CreatedDate, lp.Description },
                new LoyaltyPoint
                {
                    UserId = cust1Id,
                    TransactionType = "Earned",
                    Points = 30,
                    Amount = 299.98m,
                    Description = "Points earned from booking: Summer Music Festival 2024",
                    CreatedDate = booking1Date,
                    RelatedOrderId = actualBooking1.BookingId,
                    Status = "Active"
                },
                new LoyaltyPoint
                {
                    UserId = cust1Id,
                    TransactionType = "Earned",
                    Points = 30,
                    Amount = 299.98m,
                    Description = "Points earned from booking: Jazz Night Under the Stars",
                    CreatedDate = booking7Date,
                    RelatedOrderId = actualBooking7.BookingId,
                    Status = "Active"
                },
                new LoyaltyPoint
                {
                    UserId = cust2Id,
                    TransactionType = "Earned",
                    Points = 32,
                    Amount = 319.96m,
                    Description = "Points earned from booking: Rock Concert Series",
                    CreatedDate = booking2Date,
                    RelatedOrderId = actualBooking2.BookingId,
                    Status = "Active"
                },
                new LoyaltyPoint
                {
                    UserId = cust3Id,
                    TransactionType = "Earned",
                    Points = 16,
                    Amount = 159.99m,
                    Description = "Points earned from booking: Championship Football Match",
                    CreatedDate = booking3Date,
                    RelatedOrderId = actualBooking3.BookingId,
                    Status = "Active"
                },
                new LoyaltyPoint
                {
                    UserId = cust4Id,
                    TransactionType = "Earned",
                    Points = 45,
                    Amount = 449.97m,
                    Description = "Points earned from booking: Summer Music Festival 2024",
                    CreatedDate = booking4Date,
                    RelatedOrderId = actualBooking4.BookingId,
                    Status = "Active"
                },
                new LoyaltyPoint
                {
                    UserId = cust5Id,
                    TransactionType = "Earned",
                    Points = 20,
                    Amount = 199.99m,
                    Description = "Points earned from booking: Tech Conference 2024",
                    CreatedDate = booking5Date,
                    RelatedOrderId = actualBooking5.BookingId,
                    Status = "Active"
                },
                new LoyaltyPoint
                {
                    UserId = cust6Id,
                    TransactionType = "Earned",
                    Points = 16,
                    Amount = 159.98m,
                    Description = "Points earned from booking: Rock Concert Series",
                    CreatedDate = booking6Date,
                    RelatedOrderId = actualBooking6.BookingId,
                    Status = "Active"
                }
            );

            context.SaveChanges();
        }
    }
}
