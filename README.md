# Clean Minimal API 
[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/stphnwlsh/CleanMinimalApi/Build%20Pipeline?label=Build%20Pipeline&logo=github&logoColor=white&style=for-the-badge)](https://github.com/stphnwlsh/CleanMinimalApi/actions/workflows/build-pipeline.yml)
[![Codecov](https://img.shields.io/codecov/c/github/stphnwlsh/CleanMinimalApi?label=Code%20Coverage&logo=codecov&logoColor=white&style=for-the-badge)](https://codecov.io/gh/stphnwlsh/CleanMinimalApi)
![Nuget](https://img.shields.io/nuget/v/CleanMinimalApi.Template?label=nuget%20template&logo=nuget&logoColor=white&style=for-the-badge)

This is a template API using [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) alongside .net 6's [Minimal APIs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0).

## Prerequisites
This solution in built on the [.net 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0), you need to install that before it will work for you.  If you want to build the Dockerfile you will need to install  [Docker](https://www.docker.com/products/docker-desktop) as well.

## Installation
This is a template and you can install it using the [dotnet new cli](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new).  To install the lastest version of the template run the following command.
```
dotnet new --install CleanMinimalApi.Template
```
To create a new solution using this template run the following command
```
dotnet new cleanminimalapi --name {YOUR_SOLUTION_NAMESPACE} --au "{YOU_AUTHORS_NAME}"
```

## Docker
There's a dockerfile included in the build folder and serves the purpose of restoring, building, testing, publishing and then creating a runtime image of the API.  Works on my machine.....you can add a version prefix and suffix to version the service in the assembly.  The Dockerfile does have stages so you can just run the tests or publish the solution depending on your needs.  Review the `build-pipeline.yml` in the github folder for more detailed usage.

```
docker build . -t cleanminimalapi:latest --build-arg VERSION_PREFIX {VERSION_NUMBER} -- build-arg VERSION_SUFFIX {PRERELEASE_NAME}
```

The Github Action does publish an image of this API and you check it out for yourself by runnning this command in docker.

```
docker pull stphnwlsh/cleanminimalapi
```

## Architecture
This solution is loosly based on Clean Architecture patterns, it's by no means perfect.  I prefer to call it "Minimal Clean Architecture".  Much inspiration has been taken from [Jason Taylor's Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture), but I have takend some things further and scaled back others.

1. As with Jason's solution the Infrastructure project is referenced by the Presentation project.  This is for Dependency Injection purposes, so to protect this a little further, all classes in the Infrastructure project are `internal`.  This stops them being accidentally used in the Presentation project.
1. There's no Automapper, it's a great library but there's no need for it here.  The domain is simple and so save effor, heartache and pain I am not mapping anything.  The Domain models run through the project, this is why I call this clean-ish architecture. 

## Features
There are plenty of handy implementations of features throughout this solution, in no particular order here are some that might interest you.

- Logging using [Serilog](https://github.com/serilog/serilog)
- Mediator Pattern using [Mediatr](https://github.com/jbogard/MediatR)
- Validation using [FluentValidation](https://github.com/FluentValidation/FluentValidation)
- Testing using [Shouldly](https://github.com/shouldly/shouldly) and [NSubstitute](https://github.com/nsubstitute/NSubstitute)
- OpenApi using [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

## Resources
This sample would not have been possible without gaining inspiration from the following resources.  If you are on your own learning adventure please read the following blogs and documentation.
- [David Fowler - Minimal APIs at a glance](https://gist.github.com/davidfowl/ff1addd02d239d2d26f4648a06158727)
- [Damian Edwards - Minimal API Playground](https://github.com/DamianEdwards/MinimalApiPlayground)
- [Scott Hanselman - Minimal APIs in .NET 6 but where are the Unit Tests?](https://www.hanselman.com/blog/minimal-apis-in-net-6-but-where-are-the-unit-tests)
- [Andrew Lock - Reducing log verbosity with Serilog RequestLogging](https://andrewlock.net/using-serilog-aspnetcore-in-asp-net-core-3-reducing-log-verbosity/)

## Support
I'm sharing some of my work here and if it helps you, I'd love it if you'd consider supporting me.

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/guidelines/download-assets-sm-1.svg)](https://www.buymeacoffee.com/stphnwlsh)

