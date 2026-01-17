namespace StarEvents.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class StarEventsDBEntities : DbContext
    {
        public StarEventsDBEntities()
            : base("name=StarEventsDBEntities")
        {
            // Disable database initialization and validation since we're using manually created tables
            Database.SetInitializer<StarEventsDBEntities>(null);
            Configuration.ValidateOnSaveEnabled = false;
            Configuration.AutoDetectChangesEnabled = true;
            
            // Ensure the database connection uses the correct provider
            Database.Connection.StateChange += (sender, e) =>
            {
                if (e.CurrentState == System.Data.ConnectionState.Open)
                {
                    // Connection opened successfully
                }
            };
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Explicitly set schema to "public" for all tables (PostgreSQL default schema)
            modelBuilder.HasDefaultSchema("public");

            // Configure User as the principal in 1:1 relationships with CustomerProfile and OrganizerProfile
            // Shared primary key: CustomerProfile.CustomerId == User.UserId
            modelBuilder.Entity<CustomerProfile>()
                .HasKey(c => c.CustomerId)
                .ToTable("CustomerProfiles", "public");

            modelBuilder.Entity<CustomerProfile>()
                .Property(c => c.CustomerId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<User>()
                .ToTable("Users", "public")
                .HasOptional(u => u.CustomerProfile)
                .WithRequired(c => c.User);

            // Shared primary key: OrganizerProfile.OrganizerId == User.UserId
            modelBuilder.Entity<OrganizerProfile>()
                .HasKey(o => o.OrganizerId)
                .ToTable("OrganizerProfiles", "public");

            modelBuilder.Entity<OrganizerProfile>()
                .Property(o => o.OrganizerId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.OrganizerProfile)
                .WithRequired(o => o.User);

            // Explicit keys and table mappings for entities that don't follow EF's default key naming convention
            modelBuilder.Entity<EventDiscount>()
                .HasKey(ed => ed.DiscountId)
                .ToTable("EventDiscounts", "public");

            modelBuilder.Entity<CustomerCard>()
                .HasKey(cc => cc.CardId)
                .ToTable("CustomerCards", "public");

            modelBuilder.Entity<LoyaltyPoint>()
                .HasKey(lp => lp.LoyaltyId)
                .ToTable("LoyaltyPoints", "public");

            // Admin: shared primary key 1:0..1 with User (AdminId == UserId)
            modelBuilder.Entity<Admin>()
                .HasKey(a => a.AdminId)
                .ToTable("Admins", "public");

            modelBuilder.Entity<Admin>()
                .Property(a => a.AdminId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Admin)
                .WithRequired(a => a.User);

            // Explicit table mappings for all other entities
            modelBuilder.Entity<Booking>().ToTable("Bookings", "public");
            modelBuilder.Entity<Event>().ToTable("Events", "public");
            modelBuilder.Entity<Payment>().ToTable("Payments", "public");
            modelBuilder.Entity<SeatCategory>().ToTable("SeatCategories", "public");
            modelBuilder.Entity<Ticket>().ToTable("Tickets", "public");
            modelBuilder.Entity<Venue>().ToTable("Venues", "public");
            modelBuilder.Entity<ActivityLog>().ToTable("ActivityLogs", "public");

            // Relationships that could create multiple cascade paths:
            // Ticket has FKs to both Booking and SeatCategory. We disable cascade delete on SeatCategory -> Tickets
            // and keep cascade on Booking -> Tickets (handled by convention).
            modelBuilder.Entity<Ticket>()
                .HasRequired(t => t.SeatCategory)
                .WithMany(sc => sc.Tickets)
                .HasForeignKey(t => t.SeatCategoryId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    
        public virtual DbSet<CustomerProfile> CustomerProfiles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<OrganizerProfile> OrganizerProfiles { get; set; }
        public virtual DbSet<CustomerCard> CustomerCards { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<SeatCategory> SeatCategories { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<EventDiscount> EventDiscounts { get; set; }
        public virtual DbSet<LoyaltyPoint> LoyaltyPoints { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
    }
}
