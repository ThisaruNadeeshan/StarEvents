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

- **Integrations**
  - ImageKit for cloud image storage (profile photos, event images, QR codes)
  - Resend for transactional emails (welcome emails, booking confirmations)

- **Other Highlights**
  - Modern, responsive UI
  - Entity Framework Code First with migrations
  - Built with ASP.NET MVC, Entity Framework 6, C#, and SQL Server

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/ThisaruNadeeshan/StarEvents/
```

### 2. Open in Visual Studio
- Open the `.sln` file with Visual Studio 2019/2022.

### 3. Database Configuration

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

### 5. Build and Run
- Press `F5` or use the "Start" button in Visual Studio.

## Technology Stack

- **Framework**: ASP.NET MVC 5.2.9
- **ORM**: Entity Framework 6.5.1 (Code First)
- **Database**: SQL Server
- **Image Storage**: ImageKit
- **Email Service**: Resend
- **Frontend**: Bootstrap 5, jQuery 3.7.0

## License

This project is for educational and demonstration purposes.
