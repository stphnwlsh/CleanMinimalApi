# Running Clean Minimal API

There are two pre-configured environments, `local` and `docker`. The matching `appsettings.*.json` files decide where logs go: the console when running locally, and a [Seq](https://datalust.co/seq) container when running in Docker.

## Database

The app stores its data in a SQLite file, `movie-reviews.db`, next to the app. In Docker the file is `/app/data/movie-reviews.db`, on the `movie-reviews-data` volume, so it survives a restart. On startup the app applies any pending Entity Framework Core migrations. When `Database:SeedSampleData` is `true`, it also seeds the same sample data into an empty database. It's on in `appsettings.local.json` and `appsettings.docker.json` and off everywhere else, so a deployed copy never starts with fake data. Delete the file, or run `docker compose down -v`, to start again. Change `ConnectionStrings:MovieReviews` to point somewhere else, or to `Data Source=MovieReviews;Mode=Memory;Cache=Shared` to keep everything in memory.

To add a migration after changing the data model:

``` bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add <Name> --project ./src/Infrastructure --output-dir Databases/MovieReviews/Migrations
```

## .NET CLI

`launchSettings.json` sets the ports, environment and logging, so one command is enough.

``` bash
dotnet run --project ./src/Presentation
```

## Docker Compose

`docker-compose.yml` starts the app and a Seq container for logs.

``` bash
docker compose up -d --build
```

- App: <http://localhost:8080/scalar> (Scalar API reference)
- OpenAPI document: <http://localhost:8080/openapi/v1.json>
- Seq: <http://localhost:5341>
- Health checks: <http://localhost:8080/health/live> (the process is up) and <http://localhost:8080/health/ready> (the database can be reached). Both answer without a token. The image's `HEALTHCHECK` uses the live one.

The container runs as the image's non-root `app` user.

To shut the containers down:

``` bash
docker compose down
```

## HTTPS

Outside the `local` and `Development` environments the app sends an HSTS header and redirects HTTP to HTTPS. The container only listens on HTTP, so in production put it behind a proxy or load balancer that ends TLS, and set `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true` so the app knows the original request was HTTPS.

## Tests

``` bash
dotnet test
```

The tests never touch `movie-reviews.db`. Each test builds its own in-memory SQLite database from the migrations and the same fixed seed, so no test can change what another one sees and the results are the same every time.
