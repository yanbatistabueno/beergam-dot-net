# Beergam (.NET)

## Stack
- **Target framework:** `net10.0`
- **Web API:** ASP.NET Core
- **ORM:** Entity Framework Core
- **MySQL provider:** Pomelo.EntityFrameworkCore.MySql
- **Cache / token storage:** Redis via StackExchange.Redis
- **Auth:** JWT bearer auth stored in cookies

## Main services
- **MySQL 8** stores the application data
- **Redis 7** stores auth/cache state
- **JWT** signs access and refresh token flows

## Project layout
- `Beergam/Beergam.csproj` — main project
- `Beergam/Program.cs` — app startup, DI, auth, migrations
- `Beergam/compose.yaml` — local Docker stack
- `Beergam/Dockerfile` — API image and dev stage
- `Beergam/appsettings.json` — base config
- `Beergam/appsettings.Development.json` — dev overrides

## Run locally
```bash
dotnet run --project Beergam/Beergam.csproj
```

You need MySQL and Redis reachable through the configured connection strings.

## Run with Docker
```bash
docker compose -f Beergam/compose.yaml up --build
```

Default host ports:
- **API:** `http://localhost:5000`
- **MySQL:** `localhost:3306`
- **Redis:** `localhost:6379`
- **RedisInsight:** `http://localhost:5540`

## Configuration
### `appsettings.json`
- `ConnectionStrings:Database`
- `ConnectionStrings:Redis`
- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:SecretKey`

### `compose.yaml`
Sets the same values through environment variables:
- `ConnectionStrings__Database`
- `ConnectionStrings__Redis`
- `Jwt__Issuer`
- `Jwt__Audience`
- `Jwt__SecretKey`

## Migrations
Create a migration:
```bash
dotnet ef migrations add <MigrationName> --project Beergam/Beergam.csproj --startup-project Beergam/Beergam.csproj
```

Apply migrations:
```bash
dotnet ef database update --project Beergam/Beergam.csproj --startup-project Beergam/Beergam.csproj
```

The app also runs `db.Database.Migrate()` on startup, so pending migrations are applied automatically.

## Docker dev image
The `dev` stage in `Beergam/Dockerfile` installs `dotnet-ef` and runs:
```bash
dotnet watch run --no-launch-profile --urls http://+:8000
```

## Auth flow
- Login and register issue access + refresh tokens
- Tokens are stored in HTTP-only cookies
- JWT `sub` contains the user pin
- Auth validation checks the current `jti` in Redis

## Redis usage
Redis is used for:
- refresh token storage
- revocation tracking
- current JWT `jti`
- TTL lookup for token expiration

## Notes
- OpenAPI is enabled in development
- Most routes require authentication because the app uses a fallback auth policy
