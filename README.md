<picture>
  <source media="(prefers-color-scheme: light)" srcset="https://raw.githubusercontent.com/LeanMeanCleanApps/brand/main/assets/logos/minimalapi/png/minimalapi-light-256.png">
  <img src="https://raw.githubusercontent.com/LeanMeanCleanApps/brand/main/assets/logos/minimalapi/png/minimalapi-256.png" alt="Clean Minimal API logo" width="128" align="right">
</picture>

# Clean Minimal API

[![Build Pipeline](https://img.shields.io/github/actions/workflow/status/stphnwlsh/CleanMinimalApi/build-pipeline.yml?branch=main&label=Build%20Pipeline&logo=github&style=for-the-badge)](https://github.com/stphnwlsh/CleanMinimalApi/actions/workflows/build-pipeline.yml)
[![Code Coverage](https://img.shields.io/codecov/c/github/stphnwlsh/CleanMinimalApi?label=Code%20Coverage&logo=codecov&logoColor=white&style=for-the-badge)](https://codecov.io/gh/stphnwlsh/CleanMinimalApi)
[![NuGet](https://img.shields.io/nuget/v/CleanMinimalApi.Template?label=NuGet&logo=nuget&logoColor=white&style=for-the-badge)](https://www.nuget.org/packages/CleanMinimalApi.Template)
[![NuGet Downloads](https://img.shields.io/nuget/dt/CleanMinimalApi.Template?label=Downloads&logo=nuget&logoColor=white&style=for-the-badge)](https://www.nuget.org/packages/CleanMinimalApi.Template)
[![License](https://img.shields.io/github/license/stphnwlsh/CleanMinimalApi?label=License&style=for-the-badge)](LICENSE.md)
[![GitHub Sponsors](https://img.shields.io/github/sponsors/stphnwlsh?label=Sponsor&logo=githubsponsors&logoColor=white&color=ea4aaa&style=for-the-badge)](https://github.com/sponsors/stphnwlsh)

A `dotnet new` template for an API built with a streamlined version of Clean Architecture and ASP.NET Core [Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis). All the layers you need, none of the ceremony you don't.

It's one of five [Lean Mean Clean Apps](https://leanmeancleanapps.github.io/) templates that share the same Application and Infrastructure layers and differ only in how they present them. Same genes, different haircuts. See [Related templates](#related-templates).

## Contents

- [Getting started](#getting-started)
- [Running the project](#running-the-project)
- [Architecture](#architecture)
- [Features](#features)
- [Releases](#releases)
- [Related templates](#related-templates)
- [Resources](#resources)
- [Contributing](#contributing)
- [The team](#the-team)
- [License](#license)

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/products/docker-desktop) (optional, to build and run the container image)

### Install the template

``` bash
dotnet new install CleanMinimalApi.Template
```

### Create a solution

``` bash
dotnet new cleanminimalapi --name {YOUR_SOLUTION_NAMESPACE} --authorName "{YOUR_NAME}"
```

That's it. You now have three layers, four test projects, a Dockerfile and a CI pipeline. Go and tell your team you've been working on it all week.

### Protect the API with bearer tokens (optional)

The API runs without authentication unless you give it an authorization server. Pass one when you create the solution, or set `Authentication:Authority` and `Authentication:Audience` in `appsettings.json` later.

``` bash
dotnet new cleanminimalapi --name {YOUR_SOLUTION_NAMESPACE} --authority https://{YOUR_AUTHORIZATION_SERVER} --audience https://{YOUR_API_URL}
```

With an authority set, the movie, author and review endpoints need a JWT bearer token issued by that server for the audience, and answer `401 Unauthorized` without one. The version endpoint, the OpenAPI document and Scalar stay open. Scalar shows the bearer scheme so you can paste a token and try the endpoints.

## Running the project

``` bash
docker compose up -d
```

Then open the Scalar API reference at <http://localhost:8080/scalar>. See [docs/RUN.md](docs/RUN.md) for every way to run it.

The `Dockerfile` has stages for testing, publishing and running, so you can build just the part you need. You can also stamp a version into the assembly.

``` bash
docker build . --target run --tag cleanminimalapi:latest --build-arg VERSION_PREFIX={VERSION_NUMBER} --build-arg VERSION_SUFFIX={PRERELEASE_NAME}
```

## Architecture

This solution is loosely based on Clean Architecture. It's by no means perfect, so I prefer to call it "Lean Mean Clean Architecture". Fewer layers than an onion, more than a sandwich. Inspiration comes from [Jason Taylor's Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture), with some ideas taken further and others scaled back.

There's a little CQRS in the style of the commands and queries, but reads and writes use the same data source underneath. CQRS-curious, not CQRS-committed.

The one deliberate break from Clean Architecture is that the Presentation project references the Infrastructure project, so it can wire up dependency injection. To protect that boundary, every class in Infrastructure is `internal`, so it can't be used from Presentation by accident. Presentation can introduce itself, but it can't move in.

### Project structure

1. **Presentation** - Exposes the Application layer over HTTP using ASP.NET Core Minimal APIs. Endpoints are grouped by feature, send commands and queries through Mediator, and turn Application errors into Problem Details responses.
1. **Application** - Owns the domain and business logic. Commands and queries are validated and handled here, and each domain type has its own repository interface. This layer doesn't care what fulfils those contracts, as long as something does.
1. **Infrastructure** - Owns the data. It implements the repository interfaces to fetch, create, update and remove data, and maps its own data models onto the Application entities so persistence details don't leak upwards.

## API documentation

Every endpoint is documented with XML comments: a summary, a description with a sample request, each parameter and request body, and every response code. Request and response models are documented the same way, with examples. The built-in OpenAPI generator in .NET 10 reads those comments at build time, so the OpenAPI document at `/openapi/v1.json` always matches the code, and [Scalar](https://github.com/scalar/scalar) at `/scalar` turns it into an interactive reference with request samples you can run.

## Features

- HTTP endpoints using [Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- OpenAPI using [Microsoft.AspNetCore.OpenApi](https://learn.microsoft.com/aspnet/core/fundamentals/openapi/overview), documented from [XML comments](https://learn.microsoft.com/aspnet/core/fundamentals/openapi/openapi-comments) and browsed with [Scalar](https://github.com/scalar/scalar)
- Optional JWT bearer authentication, turned on by setting an authority, with the security scheme shown in Scalar
- Logging using [Serilog](https://github.com/serilog/serilog), with [Seq](https://datalust.co/seq) when running in Docker
- Mediator pattern using [Mediator](https://github.com/martinothamar/Mediator)
- Offset paging and search on every list endpoint, such as `/api/movies?query=star&offset=0&limit=10`. List items carry a review count and average stars worked out by the database, and the by-id endpoints return the reviews
- Health checks at `/health/live` and `/health/ready`, using [ASP.NET Core health checks](https://learn.microsoft.com/aspnet/core/host-and-deploy/health-checks)
- Validation using [FluentValidation](https://github.com/FluentValidation/FluentValidation) in an Application pipeline behaviour
- Error responses using [Problem Details (RFC 9457)](https://learn.microsoft.com/aspnet/core/fundamentals/error-handling-api)
- Object mapping using [Mapperly](https://mapperly.riok.app)
- Data stored in [SQLite](https://learn.microsoft.com/ef/core/providers/sqlite/) using [Entity Framework Core](https://learn.microsoft.com/ef/core) migrations, seeded with [Bogus](https://github.com/bchavez/Bogus) only when `Database:SeedSampleData` is on. Each test uses its own throwaway in-memory SQLite database, so tests never touch a real one or each other's data
- Testing using [xUnit](https://xunit.net), [Shouldly](https://github.com/shouldly/shouldly) and [NSubstitute](https://github.com/nsubstitute/NSubstitute), with coverage from [Coverlet](https://github.com/coverlet-coverage/coverlet)
- Build rules in `Directory.Build.props`: code style and the recommended analyzers are checked on every build, warnings fail it, and package versions live in one place with [Central Package Management](https://learn.microsoft.com/nuget/consume-packages/central-package-management)
- CI with GitHub Actions: build and test, a `dotnet format` check, 95% coverage gate with at least 90% on the lines each pull request changes, [Trivy](https://trivy.dev) container scan, [CodeQL](https://codeql.github.com) static analysis, then a monthly NuGet release and GitHub release from `main` using [trusted publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing)
- Grouped dependency updates on the 15th of each month using [Dependabot](https://docs.github.com/code-security/dependabot), including the pinned Seq image
- A Docker image that runs as a non-root user with a health check

## Releases

The template is released once a month. On the 1st, at 9am US Central time, the pipeline checks for changes since the last release. If anything outside `.github/` has changed, it builds and tests `main`, publishes the new version to [NuGet](https://www.nuget.org/packages/CleanMinimalApi.Template) and creates a [GitHub release](https://github.com/stphnwlsh/CleanMinimalApi/releases) with notes for every pull request merged since the previous one. If nothing has changed, there's no release that month, because nobody needs a new version number for the same code. Dependency updates arrive on the 15th, so they're merged in time for the next release.

Versions keep the `YYYY.MM.DD.RUN` format. Up to the end of October 2026, every merge to `main` was released, so the older releases are much smaller. To release outside the monthly cycle, run the **Build Pipeline** workflow by hand. Tick **force** to release even when only `.github/` has changed.

To update an installed template to the latest release:

``` bash
dotnet new update
```

## Related templates

All five templates share the same Application and Infrastructure layers, tests, CI pipeline and repository setup.

| Template | Presentation | Package |
| --- | --- | --- |
| **Clean Minimal API** (this repo) | ASP.NET Core Minimal APIs | [CleanMinimalApi.Template](https://www.nuget.org/packages/CleanMinimalApi.Template) |
| [Clean Web API](https://github.com/stphnwlsh/CleanWebApi) | ASP.NET Core controllers | [CleanWebApi.Template](https://www.nuget.org/packages/CleanWebApi.Template) |
| [Clean GraphQL API](https://github.com/stphnwlsh/CleanGraphQLApi) | GraphQL with Hot Chocolate | [CleanGraphQLApi.Template](https://www.nuget.org/packages/CleanGraphQLApi.Template) |
| [Clean Model Context Protocol](https://github.com/stphnwlsh/CleanModelContextProtocol) | Model Context Protocol server | [CleanModelContextProtocol.Template](https://www.nuget.org/packages/CleanModelContextProtocol.Template) |
| [Clean Lambda Function](https://github.com/stphnwlsh/CleanLamdaFunction) | AWS Lambda function | [CleanLambdaFunction.Template](https://www.nuget.org/packages/CleanLambdaFunction.Template) |

## Resources

This template wouldn't exist without these resources. If you're on your own learning adventure, they're well worth a read. Better than the comments section, anyway.

- [David Fowler - Minimal APIs at a glance](https://gist.github.com/davidfowl/ff1addd02d239d2d26f4648a06158727)
- [Damian Edwards - Minimal API Playground](https://github.com/DamianEdwards/MinimalApiPlayground)
- [Scott Hanselman - Minimal APIs in .NET 6 but where are the Unit Tests?](https://www.hanselman.com/blog/minimal-apis-in-net-6-but-where-are-the-unit-tests)
- [Andrew Lock - Reducing log verbosity with Serilog RequestLogging](https://andrewlock.net/using-serilog-aspnetcore-in-asp-net-core-3-reducing-log-verbosity/)
- [Jason Taylor - Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture)
- [Robert C. Martin - The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## Contributing

Bug reports, ideas and pull requests are welcome, and typo fixes are welcome with open arms. Please read the [contributing guide](CONTRIBUTING.md) and the [code of conduct](CODE_OF_CONDUCT.md) first. To report a security vulnerability, follow the [security policy](SECURITY.md) instead of opening an issue.

## The team

This template is part of [Lean Mean Clean Apps](https://leanmeancleanapps.github.io/), a small family of .NET templates built and looked after in spare time.

| | Who | What they do | Find them |
| --- | --- | --- | --- |
| <img src="https://github.com/stphnwlsh.png?size=96" width="48" alt=""> | **Stephen Walsh** | Maintainer. Builds every template, keeps them current with each .NET release, and drinks the coffee. | [GitHub](https://github.com/stphnwlsh) · [LinkedIn](https://www.linkedin.com/in/stphnwlsh) · [Medium](https://stphnwlsh.medium.com) · [Website](https://stphnwlsh.github.io) |

Want your name in this table? Pull requests are the way in, so start with the [contributing guide](CONTRIBUTING.md). For help using the template, see [SUPPORT.md](SUPPORT.md).

### Support the work

If this template saves you time, please consider [sponsoring Stephen on GitHub](https://github.com/sponsors/stphnwlsh). It keeps all five templates current with each .NET release, and it keeps the coffee flowing, which is the real dependency here.

## License

Released under the [MIT License](LICENSE.md). Take it, change it, ship it. Just don't blame us for your production incidents.
