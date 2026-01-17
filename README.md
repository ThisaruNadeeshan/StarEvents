# StarEvents - Online Event Ticketing Web Application

StarEvents is a full-featured ASP.NET MVC web application for discovering, booking, and managing events online. Designed for both customers and organizers, StarEvents makes event management and ticketing seamless, secure, and user-friendly.

## Features

- **Customer Portal**
  - Browse and search upcoming events
  - Book tickets and manage bookings
  - View booking history and loyalty points
  - Update profile and account settings

- **Organizer Portal**
  - Create, edit, and manage events
  - View event bookings and manage attendees
  - Access organizer account and security settings

- **Admin Panel**
  - Manage users, events, and platform settings
  - Generate reports and analytics

- **Security**
  - Secure authentication and password management
  - User roles: Customer, Organizer, Admin

<<<<<<< HEAD
- **Integrations**
  - ImageKit for cloud image storage (profile photos, event images, QR codes)
  - Resend for transactional emails (welcome emails, booking confirmations)
=======
- **AI Chatbot**
  - Customer-facing AI assistant powered by OpenAI
  - Access booking history and available events
  - Get real-time seat availability and event information
  - Integrated into main navigation for easy access

- **Integrations**
  - ImageKit for cloud image storage (profile photos, event images, QR codes)
  - Resend for transactional emails (welcome emails, booking confirmations)
  - OpenAI API for AI chatbot functionality
>>>>>>> dev/cross_main

- **Other Highlights**
  - Modern, responsive UI
  - Entity Framework Code First with migrations
<<<<<<< HEAD
  - Built with ASP.NET MVC, Entity Framework 6, C#, and SQL Server
=======
  - Built with ASP.NET MVC, Entity Framework 6, C#, and Supabase (PostgreSQL)
>>>>>>> dev/cross_main

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/ThisaruNadeeshan/StarEvents/
```

### 2. Open in Visual Studio
- Open the `.sln` file with Visual Studio 2019/2022.

### 3. Database Configuration

<<<<<<< HEAD
The project uses **Entity Framework Code First** with migrations.

1. **Create an empty SQL Server database** named `StarEventsDB` (or your preferred name).

2. **Update the connection string** in `Web.config`:
   ```xml
   <connectionStrings>
       <add name="StarEventsDBEntities" 
            connectionString="data source=YOUR_SERVER;initial catalog=StarEventsDB;integrated security=True;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework" 
            providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

3. **Run migrations** in Package Manager Console:
   ```powershell
   Enable-Migrations -ContextTypeName StarEvents.Models.StarEventsDBEntities
   Add-Migration InitialCreate
   Update-Database
   ```

This will create all tables and relationships in your database.
=======
The project uses **Entity Framework Code First** with migrations and **Supabase PostgreSQL** as the database.

1. **Install Required Packages** via Package Manager Console:
   ```powershell
   Install-Package EntityFramework6.Npgsql -Version 6.4.1
   Install-Package Npgsql -Version 4.1.9
   ```

2. **Configure Supabase Connection String** in `Web.config`:
   The connection string is already configured for Supabase. If you need to update it, use this format:
   ```xml
   <connectionStrings>
       <add name="StarEventsDBEntities" 
            connectionString="Host=YOUR_HOST;Port=5432;Database=postgres;Username=YOUR_USERNAME;Password=YOUR_PASSWORD;SslMode=Require;TrustServerCertificate=true;Pooling=true;Timeout=30" 
            providerName="Npgsql" />
   </connectionStrings>
   ```
   **Note**: Use `SslMode` (not `SSL Mode`) and `TrustServerCertificate` (not `Trust Server Certificate`) for proper PostgreSQL connection.

3. **Update Entity Framework Provider** in `Web.config` (already configured):
   ```xml
   <entityFramework>
       <providers>
           <provider invariantName="Npgsql" type="Npgsql.NpgsqlServices, EntityFramework6.Npgsql" />
       </providers>
   </entityFramework>
   ```

4. **Run migrations** in Package Manager Console:
   ```powershell
   Update-Database
   ```

This will create all tables and relationships in your Supabase database, and seed initial data including:
- Admin account (Email: `admin@starevents.com`, Password: `Admin@123`)
- Sample organizers, customers, venues, events, bookings, and tickets
>>>>>>> dev/cross_main

### 4. Configure External Services

#### ImageKit (for image uploads)
Update `Web.config` with your ImageKit credentials:
```xml
<appSettings>
    <add key="ImageKit.PublicKey" value="your_public_key" />
    <add key="ImageKit.PrivateKey" value="your_private_key" />
    <add key="ImageKit.UrlEndpoint" value="https://ik.imagekit.io/your_endpoint" />
    <add key="ImageKit.UploadFolder" value="/starevents" />
</appSettings>
```

#### Resend (for emails)
Update `Web.config` with your Resend API key:
```xml
<appSettings>
    <add key="Resend.ApiKey" value="your_resend_api_key" />
    <add key="Resend.FromEmail" value="tickets@yourdomain.com" />
</appSettings>
```

<<<<<<< HEAD
=======
#### OpenAI (for AI Chatbot)
Update `Web.config` with your OpenAI API key:
```xml
<appSettings>
    <add key="OpenAI.ApiKey" value="your_openai_api_key" />
    <add key="OpenAI.Model" value="gpt-4o-mini" />
    <add key="OpenAI.MaxTokens" value="1000" />
    <add key="OpenAI.Temperature" value="0.7" />
</appSettings>
```
**Note**: The chatbot requires a valid OpenAI API key. Without it, the chatbot will display a configuration error message.

>>>>>>> dev/cross_main
### 5. Build and Run
- Press `F5` or use the "Start" button in Visual Studio.

## Technology Stack

- **Framework**: ASP.NET MVC 5.2.9
- **ORM**: Entity Framework 6.5.1 (Code First)
<<<<<<< HEAD
- **Database**: SQL Server
- **Image Storage**: ImageKit
- **Email Service**: Resend
- **Frontend**: Bootstrap 5, jQuery 3.7.0
=======
- **Database**: Supabase (PostgreSQL) with Npgsql provider
- **Image Storage**: ImageKit
- **Email Service**: Resend
- **AI Service**: OpenAI API (GPT-4o-mini)
- **Frontend**: Bootstrap 5, jQuery 3.7.0, Font Awesome 6.4.0

## Default Login Credentials

After running migrations, you can log in with the seeded admin account:
- **Email**: `admin@starevents.com`
- **Password**: `Admin@123`

Sample organizer and customer accounts are also seeded with password `Org@123` and `Customer@123` respectively.

## AI Chatbot

The application includes an AI-powered chatbot accessible from the main navigation. The chatbot can:
- Answer questions about available events
- Show seat availability and pricing information
- Display user's booking history (for logged-in customers)
- Provide general information about the platform

To use the chatbot:
1. Ensure you have a valid OpenAI API key configured in `Web.config`
2. Log in to your account
3. Click "AI Assistant" in the navigation bar
4. Start chatting with the AI assistant

The chatbot uses context-aware prompts that include:
- User's recent booking history (for customers)
- Currently available events with seat information
- Event details, venues, and pricing
>>>>>>> dev/cross_main

## License

This project is for educational and demonstration purposes.
