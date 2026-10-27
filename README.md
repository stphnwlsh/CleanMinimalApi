# Clean Minimal API

[![Build Pipeline](https://img.shields.io/github/actions/workflow/status/stphnwlsh/CleanMinimalApi/build-pipeline.yml?branch=main&label=Build%20Pipeline&logo=github&style=for-the-badge)](https://github.com/stphnwlsh/CleanMinimalApi/actions/workflows/build-pipeline.yml)
[![Code Coverage](https://img.shields.io/codecov/c/github/stphnwlsh/CleanMinimalApi?label=Code%20Coverage&logo=codecov&logoColor=white&style=for-the-badge)](https://codecov.io/gh/stphnwlsh/CleanMinimalApi)
[![OpenSSF Scorecard](https://img.shields.io/ossf-scorecard/github.com/stphnwlsh/CleanMinimalApi?label=OpenSSF%20Scorecard&style=for-the-badge)](https://scorecard.dev/viewer/?uri=github.com/stphnwlsh/CleanMinimalApi)
[![NuGet](https://img.shields.io/nuget/v/CleanMinimalApi.Template?label=NuGet&logo=nuget&logoColor=white&style=for-the-badge)](https://www.nuget.org/packages/CleanMinimalApi.Template)
[![NuGet Downloads](https://img.shields.io/nuget/dt/CleanMinimalApi.Template?label=Downloads&logo=nuget&logoColor=white&style=for-the-badge)](https://www.nuget.org/packages/CleanMinimalApi.Template)
[![License](https://img.shields.io/github/license/stphnwlsh/CleanMinimalApi?label=License&style=for-the-badge)](LICENSE.md)
[![GitHub Sponsors](https://img.shields.io/github/sponsors/stphnwlsh?label=Sponsor&logo=githubsponsors&logoColor=white&color=ea4aaa&style=for-the-badge)](https://github.com/sponsors/stphnwlsh)

A `dotnet new` template for an API built with a streamlined version of Clean Architecture and ASP.NET Core [Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis).

It's one of five templates that share the same Application and Infrastructure layers and differ only in how they present them. See [Related templates](#related-templates).

## Contents

- [Getting started](#getting-started)
- [Running the project](#running-the-project)
- [Architecture](#architecture)
- [Features](#features)
- [Related templates](#related-templates)
- [Resources](#resources)
- [Contributing](#contributing)
- [Support](#support)
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
dotnet new cleanminimalapi --name {YOUR_SOLUTION_NAMESPACE} --author "{YOUR_NAME}"
```

## Running the project

``` bash
docker compose up -d
```

Then open the Swagger UI at <http://localhost:8080/swagger>. See [docs/RUN.md](docs/RUN.md) for every way to run it.

The `Dockerfile` has stages for testing, publishing and running, so you can build just the part you need. You can also stamp a version into the assembly.

``` bash
docker build . --target run --tag cleanminimalapi:latest --build-arg VERSION_PREFIX={VERSION_NUMBER} --build-arg VERSION_SUFFIX={PRERELEASE_NAME}
```

Every push to `main` publishes an image to Docker Hub.

``` bash
docker pull stphnwlsh/cleanminimalapi
```

## Architecture

This solution is loosely based on Clean Architecture. It's by no means perfect, so I prefer to call it "Lean Mean Clean Architecture". Inspiration comes from [Jason Taylor's Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture), with some ideas taken further and others scaled back.

There's a little CQRS in the style of the commands and queries, but reads and writes use the same data source underneath.

The one deliberate break from Clean Architecture is that the Presentation project references the Infrastructure project, so it can wire up dependency injection. To protect that boundary, every class in Infrastructure is `internal`, so it can't be used from Presentation by accident.

### Project structure

1. **Presentation** - Exposes the Application layer over HTTP using ASP.NET Core Minimal APIs. Endpoints are grouped by feature and pass requests through endpoint filters for validation before handing them to MediatR.
1. **Application** - Owns the domain and business logic. Commands and queries are validated and handled here, and each domain type has its own repository interface. This layer doesn't care what fulfils those contracts, as long as something does.
1. **Infrastructure** - Owns the data. It implements the repository interfaces to fetch, create, update and remove data, and maps its own data models onto the Application entities so persistence details don't leak upwards.

## Features

- HTTP endpoints using [Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- OpenAPI using [Microsoft.AspNetCore.OpenApi](https://learn.microsoft.com/aspnet/core/fundamentals/openapi/overview) and [Swagger UI](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- Logging using [Serilog](https://github.com/serilog/serilog), with [Seq](https://datalust.co/seq) when running in Docker
- Mediator pattern using [Mediator](https://github.com/martinothamar/Mediator)
- Validation using [FluentValidation](https://github.com/FluentValidation/FluentValidation) in an Application pipeline behaviour
- Error responses using [Problem Details (RFC 9457)](https://learn.microsoft.com/aspnet/core/fundamentals/error-handling-api)
- Object mapping using [Mapperly](https://mapperly.riok.app)
- In-memory data using [Entity Framework Core](https://learn.microsoft.com/ef/core/providers/in-memory/) seeded with [Bogus](https://github.com/bchavez/Bogus)
- Testing using [xUnit](https://xunit.net), [Shouldly](https://github.com/shouldly/shouldly) and [NSubstitute](https://github.com/nsubstitute/NSubstitute), with coverage from [Coverlet](https://github.com/coverlet-coverage/coverlet)
- CI with GitHub Actions: build and test, 95% coverage gate, [Trivy](https://trivy.dev) container scan, [CodeQL](https://codeql.github.com) static analysis, then Docker Hub and NuGet publishing from `main` using [trusted publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing)
- Monthly grouped dependency updates using [Dependabot](https://docs.github.com/code-security/dependabot)

## Related templates

All five templates share the same Application and Infrastructure layers, tests, CI pipeline and repository setup.

| Template | Presentation | Package |
| --- | --- | --- |
| **Clean Minimal API** (this repo) | ASP.NET Core Minimal APIs | [CleanMinimalApi.Template](https://www.nuget.org/packages/CleanMinimalApi.Template) |
| [Clean Web API](https://github.com/stphnwlsh/CleanWebApi) | ASP.NET Core controllers | [CleanWebApi.Template](https://www.nuget.org/packages/CleanWebApi.Template) |
| [Clean GraphQL API](https://github.com/stphnwlsh/CleanGraphQLApi) | GraphQL | [CleanGraphQLApi.Template](https://www.nuget.org/packages/CleanGraphQLApi.Template) |
| [Clean Model Context Protocol](https://github.com/stphnwlsh/CleanModelContextProtocol) | Model Context Protocol server | [CleanModelContextProtocol.Template](https://www.nuget.org/packages/CleanModelContextProtocol.Template) |
| [Clean Lambda Function](https://github.com/stphnwlsh/CleanLamdaFunction) | AWS Lambda function | [CleanLambdaFunction.Template](https://www.nuget.org/packages/CleanLambdaFunction.Template) |

## Resources

This template wouldn't exist without these resources. If you're on your own learning adventure, they're well worth a read.

- [David Fowler - Minimal APIs at a glance](https://gist.github.com/davidfowl/ff1addd02d239d2d26f4648a06158727)
- [Damian Edwards - Minimal API Playground](https://github.com/DamianEdwards/MinimalApiPlayground)
- [Scott Hanselman - Minimal APIs in .NET 6 but where are the Unit Tests?](https://www.hanselman.com/blog/minimal-apis-in-net-6-but-where-are-the-unit-tests)
- [Andrew Lock - Reducing log verbosity with Serilog RequestLogging](https://andrewlock.net/using-serilog-aspnetcore-in-asp-net-core-3-reducing-log-verbosity/)
- [Jason Taylor - Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture)
- [Robert C. Martin - The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## Contributing

Bug reports, ideas and pull requests are welcome. Please read the [contributing guide](CONTRIBUTING.md) and the [code of conduct](CODE_OF_CONDUCT.md) first. To report a security vulnerability, follow the [security policy](SECURITY.md) instead of opening an issue.

## Support

If this template saves you time, please consider [sponsoring me on GitHub](https://github.com/sponsors/stphnwlsh). It helps me keep all five templates up to date.

[![Please sponsor me](docs/sponsor.jpg)](https://github.com/sponsors/stphnwlsh)

You can also connect with me on [LinkedIn](https://www.linkedin.com/in/stphnwlsh), follow me on [Medium](https://stphnwlsh.medium.com) or [GitHub](https://github.com/stphnwlsh). For help using the template, see [SUPPORT.md](SUPPORT.md).

## License

Released under the [MIT License](LICENSE.md).
