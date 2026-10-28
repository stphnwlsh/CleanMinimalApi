# Running Clean Minimal API

There are two pre-configured environments, `local` and `docker`. The matching `appsettings.*.json` files decide where logs go: the console when running locally, and a [Seq](https://datalust.co/seq) container when running in Docker.

## Database

The app stores its data in a SQLite file, `movie-reviews.db`, next to the app. On startup it applies any pending Entity Framework Core migrations and seeds the same sample data every time if the database is empty. Delete the file to start again. Change `ConnectionStrings:MovieReviews` to point somewhere else, or to `Data Source=MovieReviews;Mode=Memory;Cache=Shared` to keep everything in memory.

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

To shut the containers down:

``` bash
docker compose down
```

## Tests

``` bash
dotnet test
```

The tests never touch `movie-reviews.db`. Each test run builds its own in-memory SQLite database from the migrations and the same fixed seed, so the results are the same every time.
