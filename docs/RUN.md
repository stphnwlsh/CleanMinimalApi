# Running Clean Minimal API

There are two pre-configured environments, `local` and `docker`. The matching `appsettings.*.json` files decide where logs go: the console when running locally, and a [Seq](https://datalust.co/seq) container when running in Docker.

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
