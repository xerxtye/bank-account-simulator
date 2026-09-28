# Bank Account API

## JWT configuration

The JWT signing secret is intentionally not stored in source control. Configure a secret of at least 32 characters before starting the API:

```bash
dotnet user-secrets set "Jwt:Secret" "replace-with-a-random-secret-at-least-32-characters"
```

Alternatively, set the environment variable `Jwt__Secret`.

Apply database migrations:

```bash
dotnet ef database update
```

## Authentication flow

1. Register with `POST /api/auth/register`.
2. Log in with `POST /api/auth/login`.
3. Copy `accessToken` from the response.
4. In Swagger UI, click **Authorize** and enter the token. Swagger adds the `Bearer` prefix automatically.
5. Call an endpoint under `/api/BankAccountItems`.

The authentication endpoints are public. All bank-account endpoints require a valid JWT and return `401 Unauthorized` without one.

Passwords are hashed with ASP.NET Core `PasswordHasher<TUser>` and only the resulting hash is stored.

## Further reading

- JWT: https://jwt.io/introduction
- ASP.NET Core authentication: https://learn.microsoft.com/aspnet/core/security/authentication/
- ASP.NET Core authorization: https://learn.microsoft.com/aspnet/core/security/authorization/introduction
- JWT Bearer authentication: https://learn.microsoft.com/aspnet/core/security/authentication/configure-jwt-bearer-authentication
- Middleware: https://learn.microsoft.com/aspnet/core/fundamentals/middleware/
- Password hashing: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.identity.passwordhasher-1
