# Manual Supabase Database Setup Guide

This guide explains how to manually create the database schema and seed data in Supabase, bypassing Entity Framework migrations.

## Prerequisites

1. Access to your Supabase project dashboard
2. Connection string: `postgresql://postgres.hxonariivyzsbzzkrmri:ThisaruCross!123@aws-1-ap-southeast-2.pooler.supabase.com:5432/postgres`

## Steps

### 1. Open Supabase SQL Editor

1. Go to your Supabase project dashboard
2. Navigate to **SQL Editor** from the left sidebar
3. Click **New Query**

### 2. Create the Database Schema

1. Open the file `StarEvents/supabase_schema.sql`
2. Copy the entire contents
3. Paste into the Supabase SQL Editor
4. Click **Run** (or press Ctrl+Enter)

This will create all the necessary tables, indexes, and foreign key relationships.

**Note:** If you see any errors about tables already existing, you can either:
- Drop existing tables first (uncomment the DROP TABLE statements at the top of the schema file)
- Or ignore the errors if the schema is already correct

### 3. Insert Seed Data

1. Open the file `StarEvents/supabase_seed_data.sql`
2. Copy the entire contents
3. Paste into a new query in the Supabase SQL Editor
4. Click **Run**

This will insert:
- **Admin account**: `admin@starevents.com` / `Admin@123`
- **3 Organizer accounts**: `organizer1@musicfest.com`, `organizer2@concerts.com`, `organizer3@sports.com` / `Org@123` (all use same password)
- **6 Customer accounts**: `alice@example.com`, `bob@example.com`, `charlie@example.com`, `diana@example.com`, `edward@example.com`, `fiona@example.com` / `Customer@123` (all use same password)
- **4 Venues**
- **5 Events** with seat categories
- **4 Event Discounts**

**Important Note:** The password hashes in the SQL file use a placeholder value. You should update them with actual SHA256 hashes:
- Generate hashes at: https://www.sha256online.com/
- Or use PowerShell: 
  ```powershell
  $sha = [System.Security.Cryptography.SHA256]::Create()
  $hash = $sha.ComputeHash([System.Text.Encoding]::UTF8.GetBytes('YourPassword'))
  ([System.BitConverter]::ToString($hash) -replace '-', '').ToLower()
  ```

### 4. Verify the Setup

Run these queries to verify data was inserted correctly:

```sql
-- Check users
SELECT "UserId", "Username", "Email", "Role" FROM "public"."Users";

-- Check venues
SELECT * FROM "public"."Venues";

-- Check events
SELECT "EventId", "Title", "Category", "EventDate" FROM "public"."Events";

-- Check seat categories
SELECT sc."CategoryName", sc."Price", sc."AvailableSeats", e."Title" as "EventTitle"
FROM "public"."SeatCategories" sc
JOIN "public"."Events" e ON sc."EventId" = e."EventId";
```

### 5. Test Login Credentials

After setup, you can test logging in with:
- **Admin**: `admin@starevents.com` / `Admin@123`
- **Organizer**: `organizer1@musicfest.com` / `Org@123`
- **Customer**: `alice@example.com` / `Customer@123`

## Connection String Configuration

Your `Web.config` should already be configured with:
```xml
<connectionStrings>
  <add name="StarEventsDBEntities" connectionString="Host=aws-1-ap-southeast-2.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.hxonariivyzsbzzkrmri;Password=ThisaruCross!123;SSL Mode=Require;Trust Server Certificate=true" providerName="Npgsql" />
</connectionStrings>
```

## Troubleshooting

### "Table already exists" errors
- This means the schema is already created. You can ignore these errors or drop existing tables first.

### "ON CONFLICT DO NOTHING" behavior
- The seed data script uses `ON CONFLICT DO NOTHING` to prevent duplicate inserts. This is safe to run multiple times.

### Password hash issues
- Ensure you're using the correct SHA256 hash format (lowercase hexadecimal, 64 characters)
- The application's `PasswordHelper.HashPassword()` method generates these hashes

### Foreign key constraint errors
- Make sure you run `supabase_schema.sql` before `supabase_seed_data.sql`
- Ensure all referenced IDs exist before inserting related records

## Next Steps

After manual database setup:
1. **Rebuild your solution** in Visual Studio
2. **Test the application** by running it locally
3. **Verify all functionalities** work correctly with the new database

## Optional: Add Sample Bookings

If you want to add sample bookings, payments, tickets, and loyalty points, you can do so manually through the application UI or by creating additional SQL scripts. The core schema and user data are the most important for initial testing.
