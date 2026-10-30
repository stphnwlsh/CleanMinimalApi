# Clean Minimal API

A `dotnet new` template for an API built with a streamlined version of Clean Architecture and ASP.NET Core [Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis).

It's one of five [Lean Mean Clean Apps](https://leanmeancleanapps.github.io/) templates, for Minimal APIs, Model Context Protocol, GraphQL, Web API controllers and AWS Lambda, that share the same Application and Infrastructure layers, tests and CI pipeline. Full docs live at [leanmeancleanapps.github.io/minimalapi](https://leanmeancleanapps.github.io/minimalapi/).

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/products/docker-desktop) (optional, to build and run the container image)

## Install

``` bash
dotnet new install CleanMinimalApi.Template
```

## Create a solution

``` bash
dotnet new cleanminimalapi --name {YOUR_SOLUTION_NAMESPACE} --authorName "{YOUR_NAME}"
```

| Option | Description |
| --- | --- |
| `--name` | The name of the solution, also used as the root namespace. |
| `--authorName` | The name of the author of the solution. |
| `--authority` | The OAuth authorization server that issues access tokens. Leave it empty to run the API without authentication. |
| `--audience` | The audience access tokens must be issued for, usually the API's own URL. |

With an authority set, the movie, author and review endpoints need a JWT bearer token, and the version endpoint, the OpenAPI document and Scalar stay open.

## Run it

From the new solution's folder, start the app and a Seq container for logs.

``` bash
docker compose up -d
```

Then open the Scalar API reference at <http://localhost:8080/scalar>. The OpenAPI document is at <http://localhost:8080/openapi/v1.json>, and Seq shows the logs at <http://localhost:5341>.

Or run it without Docker.

``` bash
dotnet run --project ./src/Presentation
```

Run the tests with `dotnet test`. Every way to run the solution is in `docs/RUN.md` inside it.

## What's inside

- **Presentation**: Minimal API endpoints grouped by feature, documented with XML comments and browsable in [Scalar](https://github.com/scalar/scalar)
- **Application**: commands and queries through [Mediator](https://github.com/martinothamar/Mediator), validation with [FluentValidation](https://github.com/FluentValidation/FluentValidation) and mapping with [Mapperly](https://mapperly.riok.app)
- **Infrastructure**: [Entity Framework Core](https://learn.microsoft.com/ef/core) on SQLite with migrations and optional sample data
- **Tests**: [xUnit](https://xunit.net), [Shouldly](https://github.com/shouldly/shouldly) and [NSubstitute](https://github.com/nsubstitute/NSubstitute), each test on its own in-memory database
- A Dockerfile that runs as a non-root user, a `docker-compose.yml`, and a GitHub Actions pipeline with tests, a coverage gate, a container scan and CodeQL

## Updates

A new version is released on the 1st of each month with every change merged since the previous release, as long as something has changed. No change, no new version number. We're not here to pad the download stats. To update an installed template:

``` bash
dotnet new update
```

What changed in each version is in the [release notes](https://github.com/stphnwlsh/CleanMinimalApi/releases). To remove the template, run `dotnet new uninstall CleanMinimalApi.Template`.

## More help

- [Docs site](https://leanmeancleanapps.github.io/minimalapi/) for guides, a reference and why it's built this way
- [README on GitHub](https://github.com/stphnwlsh/CleanMinimalApi#readme) for the architecture, every feature and the Docker build options
- [Running guide](https://github.com/stphnwlsh/CleanMinimalApi/blob/main/docs/RUN.md) for every way to run, configure and test the solution
- [GitHub issues](https://github.com/stphnwlsh/CleanMinimalApi/issues) for bugs and ideas
- [Security policy](https://github.com/stphnwlsh/CleanMinimalApi/security/policy) to report a vulnerability privately

## Support

If this template saves you time, please consider [sponsoring the work on GitHub](https://github.com/sponsors/stphnwlsh). It keeps all five templates current, and the coffee flowing.
