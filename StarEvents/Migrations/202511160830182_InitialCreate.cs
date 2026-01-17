namespace StarEvents.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.ActivityLogs",
                c => new
                    {
                        ActivityLogId = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        ActivityType = c.String(),
                        Description = c.String(),
                        PerformedBy = c.String(),
                        RelatedEntityId = c.Int(),
                        EntityType = c.String(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ActivityLogId)
                .ForeignKey("public.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "public.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        PasswordHash = c.String(),
                        Role = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "public.Admins",
                c => new
                    {
                        AdminId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.AdminId)
                .ForeignKey("public.Users", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "public.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        BookingCode = c.String(),
                        BookingDate = c.DateTime(),
                        Quantity = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        PaymentId = c.Int(),
                        SeatCategoryId = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("public.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("public.Users", t => t.User_UserId)
                .Index(t => t.EventId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "public.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        OrganizerId = c.Int(nullable: false),
                        Title = c.String(),
                        Category = c.String(),
                        Description = c.String(),
                        EventDate = c.DateTime(nullable: false),
                        Location = c.String(),
                        VenueId = c.Int(),
                        ImageUrl = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        IsActive = c.Boolean(),
                        IsPublished = c.Boolean(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("public.Users", t => t.User_UserId)
                .ForeignKey("public.Venues", t => t.VenueId)
                .Index(t => t.VenueId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "public.EventDiscounts",
                c => new
                    {
                        DiscountId = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        DiscountName = c.String(),
                        DiscountType = c.String(),
                        DiscountPercent = c.Decimal(precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(precision: 18, scale: 2),
                        SeatCategory = c.String(),
                        MaxUsage = c.Int(),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.DiscountId)
                .ForeignKey("public.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
            CreateTable(
                "public.SeatCategories",
                c => new
                    {
                        SeatCategoryId = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        CategoryName = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalSeats = c.Int(nullable: false),
                        AvailableSeats = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SeatCategoryId)
                .ForeignKey("public.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
            CreateTable(
                "public.Tickets",
                c => new
                    {
                        TicketId = c.Int(nullable: false, identity: true),
                        BookingId = c.Int(nullable: false),
                        SeatCategoryId = c.Int(nullable: false),
                        TicketCode = c.String(),
                        QRCodePath = c.String(),
                        IsUsed = c.Boolean(),
                        CreatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.TicketId)
                .ForeignKey("public.Bookings", t => t.BookingId, cascadeDelete: true)
                .ForeignKey("public.SeatCategories", t => t.SeatCategoryId, cascadeDelete: false)
                .Index(t => t.BookingId)
                .Index(t => t.SeatCategoryId);
            
            CreateTable(
                "public.Venues",
                c => new
                    {
                        VenueId = c.Int(nullable: false, identity: true),
                        VenueName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Capacity = c.Int(),
                    })
                .PrimaryKey(t => t.VenueId);
            
            CreateTable(
                "public.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        BookingId = c.Int(nullable: false),
                        PaymentReference = c.String(),
                        PaymentMethod = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        PaidAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("public.Bookings", t => t.BookingId, cascadeDelete: true)
                .Index(t => t.BookingId);
            
            CreateTable(
                "public.CustomerProfiles",
                c => new
                    {
                        CustomerId = c.Int(nullable: false),
                        FullName = c.String(),
                        PhoneNumber = c.String(),
                        Address = c.String(),
                        LoyaltyPoints = c.Int(),
                        ProfilePhoto = c.String(),
                        DateOfBirth = c.DateTime(),
                        Gender = c.String(),
                    })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("public.Users", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "public.CustomerCards",
                c => new
                    {
                        CardId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CardNumber = c.String(),
                        CardHolder = c.String(),
                        Expiry = c.String(),
                        CVV = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CardId)
                .ForeignKey("public.CustomerProfiles", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "public.LoyaltyPoints",
                c => new
                    {
                        LoyaltyId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TransactionType = c.String(),
                        Points = c.Int(nullable: false),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Description = c.String(),
                        CreatedDate = c.DateTime(),
                        RelatedOrderId = c.Int(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.LoyaltyId)
                .ForeignKey("public.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "public.OrganizerProfiles",
                c => new
                    {
                        OrganizerId = c.Int(nullable: false),
                        OrganizationName = c.String(),
                        ContactPerson = c.String(),
                        PhoneNumber = c.String(),
                        Address = c.String(),
                        Description = c.String(),
                        ProfilePhoto = c.String(),
                    })
                .PrimaryKey(t => t.OrganizerId)
                .ForeignKey("public.Users", t => t.OrganizerId)
                .Index(t => t.OrganizerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.OrganizerProfiles", "OrganizerId", "public.Users");
            DropForeignKey("public.LoyaltyPoints", "UserId", "public.Users");
            DropForeignKey("public.CustomerProfiles", "CustomerId", "public.Users");
            DropForeignKey("public.CustomerCards", "CustomerId", "public.CustomerProfiles");
            DropForeignKey("public.Bookings", "User_UserId", "public.Users");
            DropForeignKey("public.Payments", "BookingId", "public.Bookings");
            DropForeignKey("public.Events", "VenueId", "public.Venues");
            DropForeignKey("public.Events", "User_UserId", "public.Users");
            DropForeignKey("public.Tickets", "SeatCategoryId", "public.SeatCategories");
            DropForeignKey("public.Tickets", "BookingId", "public.Bookings");
            DropForeignKey("public.SeatCategories", "EventId", "public.Events");
            DropForeignKey("public.EventDiscounts", "EventId", "public.Events");
            DropForeignKey("public.Bookings", "EventId", "public.Events");
            DropForeignKey("public.Admins", "AdminId", "public.Users");
            DropForeignKey("public.ActivityLogs", "UserId", "public.Users");
            DropIndex("public.OrganizerProfiles", new[] { "OrganizerId" });
            DropIndex("public.LoyaltyPoints", new[] { "UserId" });
            DropIndex("public.CustomerCards", new[] { "CustomerId" });
            DropIndex("public.CustomerProfiles", new[] { "CustomerId" });
            DropIndex("public.Payments", new[] { "BookingId" });
            DropIndex("public.Tickets", new[] { "SeatCategoryId" });
            DropIndex("public.Tickets", new[] { "BookingId" });
            DropIndex("public.SeatCategories", new[] { "EventId" });
            DropIndex("public.EventDiscounts", new[] { "EventId" });
            DropIndex("public.Events", new[] { "User_UserId" });
            DropIndex("public.Events", new[] { "VenueId" });
            DropIndex("public.Bookings", new[] { "User_UserId" });
            DropIndex("public.Bookings", new[] { "EventId" });
            DropIndex("public.Admins", new[] { "AdminId" });
            DropIndex("public.ActivityLogs", new[] { "UserId" });
            DropTable("public.OrganizerProfiles");
            DropTable("public.LoyaltyPoints");
            DropTable("public.CustomerCards");
            DropTable("public.CustomerProfiles");
            DropTable("public.Payments");
            DropTable("public.Venues");
            DropTable("public.Tickets");
            DropTable("public.SeatCategories");
            DropTable("public.EventDiscounts");
            DropTable("public.Events");
            DropTable("public.Bookings");
            DropTable("public.Admins");
            DropTable("public.Users");
            DropTable("public.ActivityLogs");
        }
    }
}
