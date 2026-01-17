-- StarEvents Seed Data for Supabase PostgreSQL
-- Run this AFTER running supabase_schema.sql
-- 
-- Password hashes are SHA256. Generate using: https://www.sha256online.com/
-- Admin@123 hash: 240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9
-- Org@123 hash: 8e6db6d8d2d5c9e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e
-- Customer@123 hash: (calculate using SHA256 of "Customer@123")

-- Note: PostgreSQL uses NOW() for current timestamp
-- Use subqueries to get IDs after inserting

-- 1. Insert Admin User
INSERT INTO "public"."Users" ("Username", "Email", "PasswordHash", "Role", "IsActive", "CreatedAt") 
VALUES ('admin', 'admin@starevents.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Admin', true, NOW() - INTERVAL '6 months')
ON CONFLICT DO NOTHING;

-- 2. Insert Admin Profile
INSERT INTO "public"."Admins" ("AdminId", "CreatedBy", "Notes")
SELECT "UserId", 'System', 'Default administrator account created during database seeding'
FROM "public"."Users" WHERE "Email" = 'admin@starevents.com'
ON CONFLICT ("AdminId") DO NOTHING;

-- 3. Insert Organizer Users
INSERT INTO "public"."Users" ("Username", "Email", "PasswordHash", "Role", "IsActive", "CreatedAt") VALUES
('musicfest_org', 'organizer1@musicfest.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Organizer', true, NOW() - INTERVAL '5 months'),
('concertpromo', 'organizer2@concerts.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Organizer', true, NOW() - INTERVAL '4 months'),
('sportsevents', 'organizer3@sports.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Organizer', true, NOW() - INTERVAL '3 months')
ON CONFLICT DO NOTHING;

-- 4. Insert Organizer Profiles
INSERT INTO "public"."OrganizerProfiles" ("OrganizerId", "OrganizationName", "ContactPerson", "PhoneNumber", "Address", "Description", "ProfilePhoto")
SELECT "UserId", 'Music Festival Organizers', 'John Smith', '+1-555-0101', '123 Music Street, Los Angeles, CA 90001', 'Leading music festival organizers with 10+ years of experience', NULL
FROM "public"."Users" WHERE "Email" = 'organizer1@musicfest.com'
ON CONFLICT ("OrganizerId") DO NOTHING;

INSERT INTO "public"."OrganizerProfiles" ("OrganizerId", "OrganizationName", "ContactPerson", "PhoneNumber", "Address", "Description", "ProfilePhoto")
SELECT "UserId", 'Concert Promoters Ltd', 'Sarah Johnson', '+1-555-0102', '456 Concert Avenue, New York, NY 10001', 'Premium concert promotion and event management', NULL
FROM "public"."Users" WHERE "Email" = 'organizer2@concerts.com'
ON CONFLICT ("OrganizerId") DO NOTHING;

INSERT INTO "public"."OrganizerProfiles" ("OrganizerId", "OrganizationName", "ContactPerson", "PhoneNumber", "Address", "Description", "ProfilePhoto")
SELECT "UserId", 'Sports Events Co', 'Mike Davis', '+1-555-0103', '789 Sports Boulevard, Chicago, IL 60601', 'Professional sports event management and ticketing', NULL
FROM "public"."Users" WHERE "Email" = 'organizer3@sports.com'
ON CONFLICT ("OrganizerId") DO NOTHING;

-- 5. Insert Customer Users
INSERT INTO "public"."Users" ("Username", "Email", "PasswordHash", "Role", "IsActive", "CreatedAt") VALUES
('alice_williams', 'alice@example.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Customer', true, NOW() - INTERVAL '4 months'),
('bob_martin', 'bob@example.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Customer', true, NOW() - INTERVAL '3 months'),
('charlie_brown', 'charlie@example.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Customer', true, NOW() - INTERVAL '3 months'),
('diana_prince', 'diana@example.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Customer', true, NOW() - INTERVAL '2 months'),
('edward_norton', 'edward@example.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Customer', true, NOW() - INTERVAL '2 months'),
('fiona_green', 'fiona@example.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Customer', true, NOW() - INTERVAL '1 month')
ON CONFLICT DO NOTHING;

-- 6. Insert Customer Profiles
INSERT INTO "public"."CustomerProfiles" ("CustomerId", "FullName", "PhoneNumber", "Address", "LoyaltyPoints", "ProfilePhoto", "DateOfBirth", "Gender")
SELECT "UserId", 'Alice Williams', '+1-555-1001', '100 Main Street, Boston, MA 02101', 250, NULL, '1990-05-15'::DATE, 'Female'
FROM "public"."Users" WHERE "Email" = 'alice@example.com'
ON CONFLICT ("CustomerId") DO NOTHING;

INSERT INTO "public"."CustomerProfiles" ("CustomerId", "FullName", "PhoneNumber", "Address", "LoyaltyPoints", "ProfilePhoto", "DateOfBirth", "Gender")
SELECT "UserId", 'Bob Martin', '+1-555-1002', '200 Oak Avenue, Seattle, WA 98101', 180, NULL, '1985-08-22'::DATE, 'Male'
FROM "public"."Users" WHERE "Email" = 'bob@example.com'
ON CONFLICT ("CustomerId") DO NOTHING;

INSERT INTO "public"."CustomerProfiles" ("CustomerId", "FullName", "PhoneNumber", "Address", "LoyaltyPoints", "ProfilePhoto", "DateOfBirth", "Gender")
SELECT "UserId", 'Charlie Brown', '+1-555-1003', '300 Pine Road, Miami, FL 33101', 320, NULL, '1992-03-10'::DATE, 'Male'
FROM "public"."Users" WHERE "Email" = 'charlie@example.com'
ON CONFLICT ("CustomerId") DO NOTHING;

INSERT INTO "public"."CustomerProfiles" ("CustomerId", "FullName", "PhoneNumber", "Address", "LoyaltyPoints", "ProfilePhoto", "DateOfBirth", "Gender")
SELECT "UserId", 'Diana Prince', '+1-555-1004', '400 Elm Street, San Francisco, CA 94101', 450, NULL, '1988-11-28'::DATE, 'Female'
FROM "public"."Users" WHERE "Email" = 'diana@example.com'
ON CONFLICT ("CustomerId") DO NOTHING;

INSERT INTO "public"."CustomerProfiles" ("CustomerId", "FullName", "PhoneNumber", "Address", "LoyaltyPoints", "ProfilePhoto", "DateOfBirth", "Gender")
SELECT "UserId", 'Edward Norton', '+1-555-1005', '500 Maple Drive, Austin, TX 78701', 95, NULL, '1995-07-05'::DATE, 'Male'
FROM "public"."Users" WHERE "Email" = 'edward@example.com'
ON CONFLICT ("CustomerId") DO NOTHING;

INSERT INTO "public"."CustomerProfiles" ("CustomerId", "FullName", "PhoneNumber", "Address", "LoyaltyPoints", "ProfilePhoto", "DateOfBirth", "Gender")
SELECT "UserId", 'Fiona Green', '+1-555-1006', '600 Cedar Lane, Denver, CO 80201', 120, NULL, '1991-12-18'::DATE, 'Female'
FROM "public"."Users" WHERE "Email" = 'fiona@example.com'
ON CONFLICT ("CustomerId") DO NOTHING;

-- 7. Insert Customer Cards
INSERT INTO "public"."CustomerCards" ("CustomerId", "CardNumber", "CardHolder", "Expiry", "CVV", "IsDefault")
SELECT "CustomerId", '4111111111111111', 'Alice Williams', '12/25', '123', true
FROM "public"."CustomerProfiles" WHERE "FullName" = 'Alice Williams'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."CustomerCards" ("CustomerId", "CardNumber", "CardHolder", "Expiry", "CVV", "IsDefault")
SELECT "CustomerId", '4222222222222222', 'Bob Martin', '06/26', '456', true
FROM "public"."CustomerProfiles" WHERE "FullName" = 'Bob Martin'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."CustomerCards" ("CustomerId", "CardNumber", "CardHolder", "Expiry", "CVV", "IsDefault")
SELECT "CustomerId", '4333333333333333', 'Charlie Brown', '09/25', '789', true
FROM "public"."CustomerProfiles" WHERE "FullName" = 'Charlie Brown'
ON CONFLICT DO NOTHING;

-- 8. Insert Venues
INSERT INTO "public"."Venues" ("VenueName", "Address", "City", "Capacity") VALUES
('Grand Concert Hall', '100 Entertainment Boulevard', 'Los Angeles', 5000),
('Olympic Stadium', '200 Sports Arena Drive', 'New York', 8000),
('Metro Convention Center', '300 Conference Way', 'Chicago', 3000),
('Riverside Amphitheater', '400 Riverside Park', 'Seattle', 6000)
ON CONFLICT DO NOTHING;

-- 9. Insert Events
INSERT INTO "public"."Events" ("OrganizerId", "Title", "Category", "Description", "EventDate", "Location", "VenueId", "ImageUrl", "CreatedAt", "UpdatedAt", "IsActive", "IsPublished", "User_UserId")
SELECT 
    u."UserId",
    'Summer Music Festival 2024',
    'Music',
    'The biggest music festival of the year featuring top artists from around the world. Don''t miss this amazing experience!',
    NOW() + INTERVAL '30 days',
    'Grand Concert Hall, Los Angeles',
    v."VenueId",
    NULL,
    NOW() - INTERVAL '2 months',
    NOW() - INTERVAL '1 month',
    true,
    true,
    u."UserId"
FROM "public"."Users" u
CROSS JOIN "public"."Venues" v
WHERE u."Email" = 'organizer1@musicfest.com' AND v."VenueName" = 'Grand Concert Hall'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."Events" ("OrganizerId", "Title", "Category", "Description", "EventDate", "Location", "VenueId", "ImageUrl", "CreatedAt", "UpdatedAt", "IsActive", "IsPublished", "User_UserId")
SELECT 
    u."UserId",
    'Rock Concert Series',
    'Music',
    'An electrifying rock concert featuring legendary bands and rising stars.',
    NOW() + INTERVAL '45 days',
    'Olympic Stadium, New York',
    v."VenueId",
    NULL,
    NOW() - INTERVAL '1 month',
    NOW() - INTERVAL '10 days',
    true,
    true,
    u."UserId"
FROM "public"."Users" u
CROSS JOIN "public"."Venues" v
WHERE u."Email" = 'organizer2@concerts.com' AND v."VenueName" = 'Olympic Stadium'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."Events" ("OrganizerId", "Title", "Category", "Description", "EventDate", "Location", "VenueId", "ImageUrl", "CreatedAt", "UpdatedAt", "IsActive", "IsPublished", "User_UserId")
SELECT 
    u."UserId",
    'Championship Football Match',
    'Sports',
    'Watch the ultimate championship match between top teams.',
    NOW() + INTERVAL '20 days',
    'Olympic Stadium, New York',
    v."VenueId",
    NULL,
    NOW() - INTERVAL '3 months',
    NOW() - INTERVAL '5 days',
    true,
    true,
    u."UserId"
FROM "public"."Users" u
CROSS JOIN "public"."Venues" v
WHERE u."Email" = 'organizer3@sports.com' AND v."VenueName" = 'Olympic Stadium'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."Events" ("OrganizerId", "Title", "Category", "Description", "EventDate", "Location", "VenueId", "ImageUrl", "CreatedAt", "UpdatedAt", "IsActive", "IsPublished", "User_UserId")
SELECT 
    u."UserId",
    'Tech Conference 2024',
    'Conference',
    'Join industry leaders and innovators for cutting-edge technology discussions.',
    NOW() + INTERVAL '60 days',
    'Metro Convention Center, Chicago',
    v."VenueId",
    NULL,
    NOW() - INTERVAL '1 month',
    NOW() - INTERVAL '15 days',
    true,
    true,
    u."UserId"
FROM "public"."Users" u
CROSS JOIN "public"."Venues" v
WHERE u."Email" = 'organizer1@musicfest.com' AND v."VenueName" = 'Metro Convention Center'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."Events" ("OrganizerId", "Title", "Category", "Description", "EventDate", "Location", "VenueId", "ImageUrl", "CreatedAt", "UpdatedAt", "IsActive", "IsPublished", "User_UserId")
SELECT 
    u."UserId",
    'Jazz Night Under the Stars',
    'Music',
    'Experience smooth jazz in an open-air setting with delicious food and drinks.',
    NOW() + INTERVAL '35 days',
    'Riverside Amphitheater, Seattle',
    v."VenueId",
    NULL,
    NOW() - INTERVAL '20 days',
    NOW() - INTERVAL '10 days',
    true,
    true,
    u."UserId"
FROM "public"."Users" u
CROSS JOIN "public"."Venues" v
WHERE u."Email" = 'organizer2@concerts.com' AND v."VenueName" = 'Riverside Amphitheater'
ON CONFLICT DO NOTHING;

-- 10. Insert Seat Categories
INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'VIP', 299.99, 500, 420
FROM "public"."Events" e WHERE e."Title" = 'Summer Music Festival 2024'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'Standard', 149.99, 3000, 2850
FROM "public"."Events" e WHERE e."Title" = 'Summer Music Festival 2024'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'Economy', 79.99, 1500, 1450
FROM "public"."Events" e WHERE e."Title" = 'Summer Music Festival 2024'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'Front Row', 199.99, 200, 180
FROM "public"."Events" e WHERE e."Title" = 'Rock Concert Series'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'General Admission', 99.99, 7800, 7650
FROM "public"."Events" e WHERE e."Title" = 'Rock Concert Series'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'Premium', 159.99, 2000, 1850
FROM "public"."Events" e WHERE e."Title" = 'Championship Football Match'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'Standard', 89.99, 4000, 3900
FROM "public"."Events" e WHERE e."Title" = 'Championship Football Match'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'Student', 49.99, 2000, 1950
FROM "public"."Events" e WHERE e."Title" = 'Championship Football Match'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'VIP Pass', 499.99, 300, 280
FROM "public"."Events" e WHERE e."Title" = 'Tech Conference 2024'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'Standard', 199.99, 2000, 1950
FROM "public"."Events" e WHERE e."Title" = 'Tech Conference 2024'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'Student', 99.99, 700, 690
FROM "public"."Events" e WHERE e."Title" = 'Tech Conference 2024'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'Premium Table', 149.99, 200, 185
FROM "public"."Events" e WHERE e."Title" = 'Jazz Night Under the Stars'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."SeatCategories" ("EventId", "CategoryName", "Price", "TotalSeats", "AvailableSeats")
SELECT e."EventId", 'General Admission', 79.99, 5800, 5700
FROM "public"."Events" e WHERE e."Title" = 'Jazz Night Under the Stars'
ON CONFLICT DO NOTHING;

-- 11. Insert Event Discounts
INSERT INTO "public"."EventDiscounts" ("EventId", "DiscountName", "DiscountType", "DiscountPercent", "DiscountAmount", "SeatCategory", "MaxUsage", "Description", "StartDate", "EndDate", "IsActive", "CreatedAt", "UpdatedAt")
SELECT e."EventId", 'Early Bird VIP', 'percent', 15, NULL, 'VIP', 100, '15% off VIP tickets for early buyers', NOW() - INTERVAL '1 month', NOW() + INTERVAL '25 days', true, NOW() - INTERVAL '1 month', NOW() - INTERVAL '1 month'
FROM "public"."Events" e WHERE e."Title" = 'Summer Music Festival 2024'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."EventDiscounts" ("EventId", "DiscountName", "DiscountType", "DiscountPercent", "DiscountAmount", "SeatCategory", "MaxUsage", "Description", "StartDate", "EndDate", "IsActive", "CreatedAt", "UpdatedAt")
SELECT e."EventId", 'Group Discount', 'percent', 10, NULL, 'Standard,Economy', 500, '10% off for groups of 5 or more', NOW() - INTERVAL '10 days', NOW() + INTERVAL '28 days', true, NOW() - INTERVAL '10 days', NOW() - INTERVAL '10 days'
FROM "public"."Events" e WHERE e."Title" = 'Summer Music Festival 2024'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."EventDiscounts" ("EventId", "DiscountName", "DiscountType", "DiscountPercent", "DiscountAmount", "SeatCategory", "MaxUsage", "Description", "StartDate", "EndDate", "IsActive", "CreatedAt", "UpdatedAt")
SELECT e."EventId", 'Flash Sale', 'amount', NULL, 20, 'General Admission', 300, '$20 off general admission tickets - limited time!', NOW() - INTERVAL '5 days', NOW() + INTERVAL '40 days', true, NOW() - INTERVAL '5 days', NOW() - INTERVAL '5 days'
FROM "public"."Events" e WHERE e."Title" = 'Rock Concert Series'
ON CONFLICT DO NOTHING;

INSERT INTO "public"."EventDiscounts" ("EventId", "DiscountName", "DiscountType", "DiscountPercent", "DiscountAmount", "SeatCategory", "MaxUsage", "Description", "StartDate", "EndDate", "IsActive", "CreatedAt", "UpdatedAt")
SELECT e."EventId", 'Student Special', 'percent', 20, NULL, 'Student', 500, 'Extra 20% off student tickets', NOW() - INTERVAL '7 days', NOW() + INTERVAL '18 days', true, NOW() - INTERVAL '7 days', NOW() - INTERVAL '7 days'
FROM "public"."Events" e WHERE e."Title" = 'Championship Football Match'
ON CONFLICT DO NOTHING;

-- Note: Bookings, Payments, Tickets, and LoyaltyPoints will be added in a separate script
-- or you can insert them manually after verifying the above data
