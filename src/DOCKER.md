# Running with Docker

Build from this directory:

```bash
docker build -t bank-account-api .
```

The image uses .NET 10, runs as a non-root user, and listens on HTTP port 8080.
It does not embed passwords or apply database migrations automatically.
For deployment, terminate HTTPS at a reverse proxy.

## Configuration

Create a private environment file outside the repository, for example
`$HOME/.config/bank-account-api.env`, containing:

```dotenv
BankAccountContext__connectionString=Host=YOUR_DATABASE_HOST;Port=5432;Database=bankdb;Username=postgres;Password=YOUR_DATABASE_PASSWORD
Jwt__Secret=YOUR_RANDOM_SECRET_AT_LEAST_32_CHARACTERS
```

Use `openssl rand -hex 32` to generate a signing secret. Restrict the file's
permissions with `chmod 600 "$HOME/.config/bank-account-api.env"`.
User secrets on your host are not automatically available inside the container.
The database host must be reachable from the container; `localhost` refers to
the container itself, not your computer.

With an existing PostgreSQL database, apply migrations using the project's
normal setup (`dotnet ef database update` with host-side user secrets configured),
then run:

```bash
docker run --rm --name bank-account-api \
  --env-file "$HOME/.config/bank-account-api.env" \
  -p 127.0.0.1:8080:8080 bank-account-api
```

For local Swagger, add `-e ASPNETCORE_ENVIRONMENT=Development` before the
image name, then open http://localhost:8080/swagger. Swagger is disabled in
Production, which is the default.

## Existing Compose file

The Dockerfile provides the `final` stage referenced by `compose.yaml`.
The existing Compose file has been left unchanged. Before using it:

- Create its required `db/password.txt` secret file and protect it from source
  control (the Docker build excludes `db/`, but Git does not).
- Pass `BankAccountContext__connectionString` and `Jwt__Secret` to the `server`
  service using an `env_file` or `environment` entry. For this network the
  database host is `db`, port 5432, database `bankdb`, and user `postgres`.
  The connection password must match `db/password.txt`.
- Apply database migrations before using the API. The image does not include
  the SDK or `dotnet ef`.

Do not use `docker compose down -v` unless you intend to delete database data.
