namespace StarEvents.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLogs",
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
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
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
                "dbo.Admins",
                c => new
                    {
                        AdminId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.AdminId)
                .ForeignKey("dbo.Users", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.Bookings",
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
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.EventId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Events",
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
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .ForeignKey("dbo.Venues", t => t.VenueId)
                .Index(t => t.VenueId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.EventDiscounts",
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
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.SeatCategories",
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
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Tickets",
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
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: true)
                .ForeignKey("dbo.SeatCategories", t => t.SeatCategoryId, cascadeDelete: false)
                .Index(t => t.BookingId)
                .Index(t => t.SeatCategoryId);
            
            CreateTable(
                "dbo.Venues",
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
                "dbo.Payments",
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
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: true)
                .Index(t => t.BookingId);
            
            CreateTable(
                "dbo.CustomerProfiles",
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
                .ForeignKey("dbo.Users", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.CustomerCards",
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
                .ForeignKey("dbo.CustomerProfiles", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.LoyaltyPoints",
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
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OrganizerProfiles",
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
                .ForeignKey("dbo.Users", t => t.OrganizerId)
                .Index(t => t.OrganizerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrganizerProfiles", "OrganizerId", "dbo.Users");
            DropForeignKey("dbo.LoyaltyPoints", "UserId", "dbo.Users");
            DropForeignKey("dbo.CustomerProfiles", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.CustomerCards", "CustomerId", "dbo.CustomerProfiles");
            DropForeignKey("dbo.Bookings", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Payments", "BookingId", "dbo.Bookings");
            DropForeignKey("dbo.Events", "VenueId", "dbo.Venues");
            DropForeignKey("dbo.Events", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Tickets", "SeatCategoryId", "dbo.SeatCategories");
            DropForeignKey("dbo.Tickets", "BookingId", "dbo.Bookings");
            DropForeignKey("dbo.SeatCategories", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventDiscounts", "EventId", "dbo.Events");
            DropForeignKey("dbo.Bookings", "EventId", "dbo.Events");
            DropForeignKey("dbo.Admins", "AdminId", "dbo.Users");
            DropForeignKey("dbo.ActivityLogs", "UserId", "dbo.Users");
            DropIndex("dbo.OrganizerProfiles", new[] { "OrganizerId" });
            DropIndex("dbo.LoyaltyPoints", new[] { "UserId" });
            DropIndex("dbo.CustomerCards", new[] { "CustomerId" });
            DropIndex("dbo.CustomerProfiles", new[] { "CustomerId" });
            DropIndex("dbo.Payments", new[] { "BookingId" });
            DropIndex("dbo.Tickets", new[] { "SeatCategoryId" });
            DropIndex("dbo.Tickets", new[] { "BookingId" });
            DropIndex("dbo.SeatCategories", new[] { "EventId" });
            DropIndex("dbo.EventDiscounts", new[] { "EventId" });
            DropIndex("dbo.Events", new[] { "User_UserId" });
            DropIndex("dbo.Events", new[] { "VenueId" });
            DropIndex("dbo.Bookings", new[] { "User_UserId" });
            DropIndex("dbo.Bookings", new[] { "EventId" });
            DropIndex("dbo.Admins", new[] { "AdminId" });
            DropIndex("dbo.ActivityLogs", new[] { "UserId" });
            DropTable("dbo.OrganizerProfiles");
            DropTable("dbo.LoyaltyPoints");
            DropTable("dbo.CustomerCards");
            DropTable("dbo.CustomerProfiles");
            DropTable("dbo.Payments");
            DropTable("dbo.Venues");
            DropTable("dbo.Tickets");
            DropTable("dbo.SeatCategories");
            DropTable("dbo.EventDiscounts");
            DropTable("dbo.Events");
            DropTable("dbo.Bookings");
            DropTable("dbo.Admins");
            DropTable("dbo.Users");
            DropTable("dbo.ActivityLogs");
        }
    }
}
