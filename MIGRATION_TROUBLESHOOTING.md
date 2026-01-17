# Migration Troubleshooting Guide

## Running Update-Database

Since this is an ASP.NET Framework project, you **MUST** run migrations from **Visual Studio's Package Manager Console**, not from command line.

### Steps:

1. **Open Visual Studio**
2. **Open Package Manager Console**: View → Other Windows → Package Manager Console
3. **Set Default Project**: Make sure `StarEvents` is selected as the default project
4. **Run the command**:
   ```powershell
   Update-Database
   ```

## Common Issues and Solutions

### Issue 1: "Provider not found" or "Npgsql not registered"

**Solution**: Install the NuGet packages first:
```powershell
Install-Package EntityFramework6.Npgsql -Version 6.4.1
Install-Package Npgsql -Version 4.1.9
```

Then rebuild the solution before running `Update-Database`.

### Issue 2: "Connection string error" or "Cannot connect to database"

**Check**:
- Verify the connection string in `Web.config` is correct
- Ensure Supabase database is accessible
- Check if SSL certificate validation is the issue

**Try**: Update connection string to:
```xml
Host=aws-1-ap-southeast-2.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.hxonariivyzsbzzkrmri;Password=ThisaruCross!123;SSL Mode=Require;Trust Server Certificate=true;Pooling=true;Timeout=30
```

### Issue 3: "Schema 'public' does not exist" or "Schema 'dbo' does not exist"

**Solution**: The migration file has been updated to use `public.` schema. If you still get this error, you may need to create the schema manually in PostgreSQL:
```sql
CREATE SCHEMA IF NOT EXISTS public;
```

### Issue 4: "Table already exists" errors

**Solution**: If tables already exist, you can:
1. Drop all tables manually in Supabase
2. Or run: `Update-Database -TargetMigration:0` to reset, then `Update-Database` again

### Issue 5: "Identity column" errors

**Solution**: Entity Framework 6 with Npgsql should automatically translate `identity: true` to PostgreSQL SERIAL. If you get errors, the Npgsql provider might not be properly registered.

## Verification Steps

After running `Update-Database`, verify:

1. **Check Supabase Dashboard**: Log into Supabase and verify tables were created
2. **Check Seed Data**: Verify admin account exists:
   - Email: `admin@starevents.com`
   - Password: `Admin@123`
3. **Test Connection**: Try running the application and logging in

## Manual Database Reset (if needed)

If you need to start fresh:

1. In Supabase Dashboard, go to SQL Editor
2. Run: `DROP SCHEMA public CASCADE; CREATE SCHEMA public;`
3. Then run `Update-Database` again

## Getting Detailed Error Messages

If `Update-Database` fails, run with verbose output:
```powershell
Update-Database -Verbose
```

This will show the exact SQL being executed and where it fails.
