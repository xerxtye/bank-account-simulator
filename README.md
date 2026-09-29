# Bank Account Simulator

A small ASP.NET Core API that simulates bank account registration, JWT login, balance viewing, deposits, and withdrawals. Passwords are hashed and finance endpoints require authentication.

## Setup

The project keeps secrets outside source control. From the project directory, configure both the PostgreSQL connection string and a JWT signing secret:

```bash
dotnet user-secrets set "BankAccountContext:connectionString" "Host=localhost;Port=5432;Database=bankdb;Username=postgres;Password=YOUR_PASSWORD"

dotnet user-secrets set "Jwt:Secret" "replace-with-a-random-secret-at-least-32-characters"
```

Make sure PostgreSQL is running, then apply migrations:

```bash
dotnet ef database update
```

Start the API:

```bash
dotnet run --launch-profile https
```

## Swagger UI

While the application is running in Development, open:

- https://localhost:7010/swagger
- or http://localhost:5062/swagger when using the HTTP profile

Register with `POST /api/auth/register`. The response contains the generated account `id`. Use that ID and the password with `POST /api/auth/login`.

To call protected endpoints:

1. Copy `accessToken` from the login response.
2. Click **Authorize** in Swagger.
3. Paste only the token, without writing `Bearer` yourself.
4. Call the balance or bank-account endpoints.

## Tests

Run the unit tests with:

```bash
dotnet test BankAccountApi.Tests/BankAccountApi.Tests.csproj
```
