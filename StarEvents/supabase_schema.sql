-- StarEvents Database Schema for Supabase PostgreSQL
-- Run this script in Supabase SQL Editor to create all tables

-- Drop existing tables if they exist (run only if needed)
-- DROP TABLE IF EXISTS "public"."Tickets" CASCADE;
-- DROP TABLE IF EXISTS "public"."Payments" CASCADE;
-- DROP TABLE IF EXISTS "public"."CustomerCards" CASCADE;
-- DROP TABLE IF EXISTS "public"."LoyaltyPoints" CASCADE;
-- DROP TABLE IF EXISTS "public"."SeatCategories" CASCADE;
-- DROP TABLE IF EXISTS "public"."EventDiscounts" CASCADE;
-- DROP TABLE IF EXISTS "public"."Bookings" CASCADE;
-- DROP TABLE IF EXISTS "public"."Events" CASCADE;
-- DROP TABLE IF EXISTS "public"."Venues" CASCADE;
-- DROP TABLE IF EXISTS "public"."CustomerProfiles" CASCADE;
-- DROP TABLE IF EXISTS "public"."OrganizerProfiles" CASCADE;
-- DROP TABLE IF EXISTS "public"."Admins" CASCADE;
-- DROP TABLE IF EXISTS "public"."ActivityLogs" CASCADE;
-- DROP TABLE IF EXISTS "public"."Users" CASCADE;

-- Create Users table first (no dependencies)
CREATE TABLE IF NOT EXISTS "public"."Users" (
    "UserId" SERIAL PRIMARY KEY,
    "Username" VARCHAR(255),
    "Email" VARCHAR(255),
    "PasswordHash" VARCHAR(255),
    "Role" VARCHAR(255),
    "IsActive" BOOLEAN NOT NULL DEFAULT true,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Create ActivityLogs table
CREATE TABLE IF NOT EXISTS "public"."ActivityLogs" (
    "ActivityLogId" SERIAL PRIMARY KEY,
    "Timestamp" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "ActivityType" VARCHAR(255),
    "Description" TEXT,
    "PerformedBy" VARCHAR(255),
    "RelatedEntityId" INTEGER,
    "EntityType" VARCHAR(255),
    "UserId" INTEGER,
    CONSTRAINT "FK_ActivityLogs_Users" FOREIGN KEY ("UserId") REFERENCES "public"."Users"("UserId")
);

CREATE INDEX IF NOT EXISTS "IX_ActivityLogs_UserId" ON "public"."ActivityLogs"("UserId");

-- Create Admins table
CREATE TABLE IF NOT EXISTS "public"."Admins" (
    "AdminId" INTEGER NOT NULL PRIMARY KEY,
    "CreatedBy" VARCHAR(255),
    "Notes" TEXT,
    CONSTRAINT "FK_Admins_Users" FOREIGN KEY ("AdminId") REFERENCES "public"."Users"("UserId")
);

CREATE INDEX IF NOT EXISTS "IX_Admins_AdminId" ON "public"."Admins"("AdminId");

-- Create Venues table
CREATE TABLE IF NOT EXISTS "public"."Venues" (
    "VenueId" SERIAL PRIMARY KEY,
    "VenueName" VARCHAR(255),
    "Address" VARCHAR(255),
    "City" VARCHAR(255),
    "Capacity" INTEGER
);

-- Create OrganizerProfiles table
CREATE TABLE IF NOT EXISTS "public"."OrganizerProfiles" (
    "OrganizerId" INTEGER NOT NULL PRIMARY KEY,
    "OrganizationName" VARCHAR(255),
    "ContactPerson" VARCHAR(255),
    "PhoneNumber" VARCHAR(255),
    "Address" VARCHAR(255),
    "Description" TEXT,
    "ProfilePhoto" VARCHAR(255),
    CONSTRAINT "FK_OrganizerProfiles_Users" FOREIGN KEY ("OrganizerId") REFERENCES "public"."Users"("UserId")
);

CREATE INDEX IF NOT EXISTS "IX_OrganizerProfiles_OrganizerId" ON "public"."OrganizerProfiles"("OrganizerId");

-- Create CustomerProfiles table
CREATE TABLE IF NOT EXISTS "public"."CustomerProfiles" (
    "CustomerId" INTEGER NOT NULL PRIMARY KEY,
    "FullName" VARCHAR(255),
    "PhoneNumber" VARCHAR(255),
    "Address" VARCHAR(255),
    "LoyaltyPoints" INTEGER DEFAULT 0,
    "ProfilePhoto" VARCHAR(255),
    "DateOfBirth" DATE,
    "Gender" VARCHAR(255),
    CONSTRAINT "FK_CustomerProfiles_Users" FOREIGN KEY ("CustomerId") REFERENCES "public"."Users"("UserId")
);

CREATE INDEX IF NOT EXISTS "IX_CustomerProfiles_CustomerId" ON "public"."CustomerProfiles"("CustomerId");

-- Create CustomerCards table
CREATE TABLE IF NOT EXISTS "public"."CustomerCards" (
    "CardId" SERIAL PRIMARY KEY,
    "CustomerId" INTEGER NOT NULL,
    "CardNumber" VARCHAR(255),
    "CardHolder" VARCHAR(255),
    "Expiry" VARCHAR(255),
    "CVV" VARCHAR(255),
    "IsDefault" BOOLEAN NOT NULL DEFAULT false,
    CONSTRAINT "FK_CustomerCards_CustomerProfiles" FOREIGN KEY ("CustomerId") REFERENCES "public"."CustomerProfiles"("CustomerId") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_CustomerCards_CustomerId" ON "public"."CustomerCards"("CustomerId");

-- Create Events table
CREATE TABLE IF NOT EXISTS "public"."Events" (
    "EventId" SERIAL PRIMARY KEY,
    "OrganizerId" INTEGER NOT NULL,
    "Title" VARCHAR(255),
    "Category" VARCHAR(255),
    "Description" TEXT,
    "EventDate" TIMESTAMP NOT NULL,
    "Location" VARCHAR(255),
    "VenueId" INTEGER,
    "ImageUrl" VARCHAR(255),
    "CreatedAt" TIMESTAMP,
    "UpdatedAt" TIMESTAMP,
    "IsActive" BOOLEAN,
    "IsPublished" BOOLEAN,
    "User_UserId" INTEGER,
    CONSTRAINT "FK_Events_Users" FOREIGN KEY ("User_UserId") REFERENCES "public"."Users"("UserId"),
    CONSTRAINT "FK_Events_Venues" FOREIGN KEY ("VenueId") REFERENCES "public"."Venues"("VenueId")
);

CREATE INDEX IF NOT EXISTS "IX_Events_VenueId" ON "public"."Events"("VenueId");
CREATE INDEX IF NOT EXISTS "IX_Events_User_UserId" ON "public"."Events"("User_UserId");

-- Create EventDiscounts table
CREATE TABLE IF NOT EXISTS "public"."EventDiscounts" (
    "DiscountId" SERIAL PRIMARY KEY,
    "EventId" INTEGER NOT NULL,
    "DiscountName" VARCHAR(255),
    "DiscountType" VARCHAR(255),
    "DiscountPercent" NUMERIC(18, 2),
    "DiscountAmount" NUMERIC(18, 2),
    "SeatCategory" VARCHAR(255),
    "MaxUsage" INTEGER,
    "Description" TEXT,
    "StartDate" TIMESTAMP NOT NULL,
    "EndDate" TIMESTAMP NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT true,
    "CreatedAt" TIMESTAMP,
    "UpdatedAt" TIMESTAMP,
    CONSTRAINT "FK_EventDiscounts_Events" FOREIGN KEY ("EventId") REFERENCES "public"."Events"("EventId") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_EventDiscounts_EventId" ON "public"."EventDiscounts"("EventId");

-- Create SeatCategories table
CREATE TABLE IF NOT EXISTS "public"."SeatCategories" (
    "SeatCategoryId" SERIAL PRIMARY KEY,
    "EventId" INTEGER NOT NULL,
    "CategoryName" VARCHAR(255),
    "Price" NUMERIC(18, 2) NOT NULL,
    "TotalSeats" INTEGER NOT NULL,
    "AvailableSeats" INTEGER NOT NULL,
    CONSTRAINT "FK_SeatCategories_Events" FOREIGN KEY ("EventId") REFERENCES "public"."Events"("EventId") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_SeatCategories_EventId" ON "public"."SeatCategories"("EventId");

-- Create Bookings table
CREATE TABLE IF NOT EXISTS "public"."Bookings" (
    "BookingId" SERIAL PRIMARY KEY,
    "EventId" INTEGER NOT NULL,
    "CustomerId" INTEGER NOT NULL,
    "BookingCode" VARCHAR(255),
    "BookingDate" TIMESTAMP,
    "Quantity" INTEGER NOT NULL,
    "TotalAmount" NUMERIC(18, 2) NOT NULL,
    "Status" VARCHAR(255),
    "PaymentId" INTEGER,
    "SeatCategoryId" INTEGER,
    "User_UserId" INTEGER,
    CONSTRAINT "FK_Bookings_Events" FOREIGN KEY ("EventId") REFERENCES "public"."Events"("EventId") ON DELETE CASCADE,
    CONSTRAINT "FK_Bookings_Users" FOREIGN KEY ("User_UserId") REFERENCES "public"."Users"("UserId")
);

CREATE INDEX IF NOT EXISTS "IX_Bookings_EventId" ON "public"."Bookings"("EventId");
CREATE INDEX IF NOT EXISTS "IX_Bookings_User_UserId" ON "public"."Bookings"("User_UserId");

-- Create Payments table
CREATE TABLE IF NOT EXISTS "public"."Payments" (
    "PaymentId" SERIAL PRIMARY KEY,
    "BookingId" INTEGER NOT NULL,
    "PaymentReference" VARCHAR(255),
    "PaymentMethod" VARCHAR(255),
    "Amount" NUMERIC(18, 2) NOT NULL,
    "Status" VARCHAR(255),
    "PaidAt" TIMESTAMP,
    CONSTRAINT "FK_Payments_Bookings" FOREIGN KEY ("BookingId") REFERENCES "public"."Bookings"("BookingId") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_Payments_BookingId" ON "public"."Payments"("BookingId");

-- Create Tickets table
CREATE TABLE IF NOT EXISTS "public"."Tickets" (
    "TicketId" SERIAL PRIMARY KEY,
    "BookingId" INTEGER NOT NULL,
    "SeatCategoryId" INTEGER NOT NULL,
    "TicketCode" VARCHAR(255),
    "QRCodePath" VARCHAR(255),
    "IsUsed" BOOLEAN,
    "CreatedAt" TIMESTAMP,
    CONSTRAINT "FK_Tickets_Bookings" FOREIGN KEY ("BookingId") REFERENCES "public"."Bookings"("BookingId") ON DELETE CASCADE,
    CONSTRAINT "FK_Tickets_SeatCategories" FOREIGN KEY ("SeatCategoryId") REFERENCES "public"."SeatCategories"("SeatCategoryId")
);

CREATE INDEX IF NOT EXISTS "IX_Tickets_BookingId" ON "public"."Tickets"("BookingId");
CREATE INDEX IF NOT EXISTS "IX_Tickets_SeatCategoryId" ON "public"."Tickets"("SeatCategoryId");

-- Create LoyaltyPoints table
CREATE TABLE IF NOT EXISTS "public"."LoyaltyPoints" (
    "LoyaltyId" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "TransactionType" VARCHAR(255),
    "Points" INTEGER NOT NULL,
    "Amount" NUMERIC(18, 2),
    "Description" TEXT,
    "CreatedDate" TIMESTAMP,
    "RelatedOrderId" INTEGER,
    "Status" VARCHAR(255),
    CONSTRAINT "FK_LoyaltyPoints_Users" FOREIGN KEY ("UserId") REFERENCES "public"."Users"("UserId") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_LoyaltyPoints_UserId" ON "public"."LoyaltyPoints"("UserId");
