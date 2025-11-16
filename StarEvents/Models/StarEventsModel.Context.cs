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
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure User as the principal in 1:1 relationships with CustomerProfile and OrganizerProfile
            // Shared primary key: CustomerProfile.CustomerId == User.UserId
            modelBuilder.Entity<CustomerProfile>()
                .HasKey(c => c.CustomerId);

            modelBuilder.Entity<CustomerProfile>()
                .Property(c => c.CustomerId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.CustomerProfile)
                .WithRequired(c => c.User);

            // Shared primary key: OrganizerProfile.OrganizerId == User.UserId
            modelBuilder.Entity<OrganizerProfile>()
                .HasKey(o => o.OrganizerId);

            modelBuilder.Entity<OrganizerProfile>()
                .Property(o => o.OrganizerId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.OrganizerProfile)
                .WithRequired(o => o.User);

            // Explicit keys for entities that don't follow EF's default key naming convention
            modelBuilder.Entity<EventDiscount>()
                .HasKey(ed => ed.DiscountId);

            modelBuilder.Entity<CustomerCard>()
                .HasKey(cc => cc.CardId);

            modelBuilder.Entity<LoyaltyPoint>()
                .HasKey(lp => lp.LoyaltyId);

            // Admin: shared primary key 1:0..1 with User (AdminId == UserId)
            modelBuilder.Entity<Admin>()
                .HasKey(a => a.AdminId);

            modelBuilder.Entity<Admin>()
                .Property(a => a.AdminId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Admin)
                .WithRequired(a => a.User);

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
