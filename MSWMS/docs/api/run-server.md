# MSWMS Server Run Guide

This is a short instruction for running the server from the already published files, plus a brief note on DB setup.

## 1. Published build location

The published, self-contained build is here:
- `bin\Release\net9.0\win-x64\publish`

Main executable:
- `MSWMS.exe`

## 2. Quick start

1. Open a terminal in the publish folder:

```powershell
cd C:\Users\movlad.gunorev\RiderProjects\MSWMS\MSWMS\bin\Release\net9.0\win-x64\publish
```

2. Run the app:

```powershell
.\MSWMS.exe
```

After startup, the server listens on:
- `https://0.0.0.0:5262`
- Swagger: `/swagger`

## 3. HTTPS certificates

`Program.cs` expects a certificate file in the working directory:
- Development: `aspnetapp_dev.pfx`
- Production: `aspnetapp.pfx` (password `123123`)

The `publish` folder does not include these `.pfx` files. If startup fails due to SSL, copy the required `.pfx` to the publish folder.

To force dev environment:

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
.\MSWMS.exe
```

## 4. Database connection (short)

Connection strings are stored in `appsettings.json` in the publish folder. Update the `ConnectionStrings` section:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True;",
  "ExternalDb": "Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True;",
  "DCXDb": "Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True;"
}
```

Minimum required for startup is `DefaultConnection`. The other two are used for external data sources.

### Pre-flight checks

- SQL Server is reachable and port `1433` is open.
- The DB user has permissions.
- On first run, migrations and seed data are applied automatically.

## 5. Common issues

- **SSL error**: ensure the required `.pfx` is in the publish folder.
- **403 on API calls**: authentication required (JWT or cookie).
- **DB connection error**: check connection string and server reachability.
