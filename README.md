# Manisa Web Platform

This project is an ASP.NET Core MVC application for the Manisa brand, built around a modular monolith and SQLite persistence.

## Phase 1 delivered

- ASP.NET Core MVC with Identity and SQLite
- SQLite database stored outside the web root in `%APPDATA%\Manisa\manisa.db` by default
- EF Core migrations and idempotent seed data for brand settings and demo records
- Public pages in Persian, including home, about, factory, product listing, contact, and request-quote flows
- Lead intake with server-side validation, honeypot protection, and Persian/Arabic digit normalization
- Dashboard and admin summary pages with real database metrics
- Business-logic tests covering digit normalization, Iranian mobile validation, WhatsApp links, status transitions, slug generation, and SEO metadata

## Database configuration

The application uses the `Database:Path` setting or the `MANISA_DB_PATH` environment variable when present. Default behavior is to create the database under the current user's AppData folder.

Example:

```json
{
  "Database": {
    "Path": "C:/Users/siavash/AppData/Roaming/Manisa/manisa.db"
  }
}
```

## Running locally

```bash
dotnet restore

dotnet build

dotnet run --project Manisa.Web/Manisa.Web.csproj
```

## Database migration and backup

To create or update migrations:

```bash
dotnet ef migrations add <MigrationName> --project Manisa.Web/Manisa.Web.csproj --startup-project Manisa.Web/Manisa.Web.csproj
```

To apply migrations:

```bash
dotnet ef database update --project Manisa.Web/Manisa.Web.csproj --startup-project Manisa.Web/Manisa.Web.csproj
```

Backup procedure:

```bash
copy "%APPDATA%\Manisa\manisa.db" "%APPDATA%\Manisa\manisa-backup.db"
```

Restore procedure:

```bash
copy "%APPDATA%\Manisa\manisa-backup.db" "%APPDATA%\Manisa\manisa.db"
```

## Security notes

- No default production admin account is created.
- Identity registration and role assignment remain the safe first-run path for administrators.
- Sensitive business information is not exposed to the browser.
- Demo records are clearly marked and should be removed before production use.
