# Clean Minimal API 
This is a sample application to be an example of using [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) alongside .net 6's [Minimal APIs](https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-6-preview-4/#introducing-minimal-apis).

## Features
- Logging using [Serilog](https://github.com/serilog/serilog)
- Mediator Pattern using [Mediatr](https://github.com/jbogard/MediatR)
- Validation using [FluentValidation](https://github.com/FluentValidation/FluentValidation)
- Testing using [Shouldly](https://github.com/shouldly/shouldly) and [NSubstitute](https://github.com/nsubstitute/NSubstitute)
- OpenApi using [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

## Docker
There's a dockerfile included in the build folder and serves the purpose of restoring, building, testing, publishing and then creating a runtime image of the API.  Works on my machine.....

```
docker build -f ./build/Dockerfile -t minapi:latest .
```

## Prerequisites
This solution depends on the [pre-release SDK for .net 6](https://dotnet.microsoft.com/download/dotnet/6.0), you need to install that before it will work for you.

## Projects
This solution contains a few projects to follow the Clean Architecture patterns, it's by no means perfect.  But it's an example so cut me some slack.  Each project has a purpose and is separated from the others, for a far more well thought out solution please see [Jason Taylor's Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture) 

## Resources
This sample would not have been possible without gaining inspiration from the following resources.  If you are on your own learning adventure please read the following blogs and documentation.
- [David Fowler - Minimal APIs at a glance](https://gist.github.com/davidfowl/ff1addd02d239d2d26f4648a06158727)
- [Damian Edwards - Minimal API Playground](https://github.com/DamianEdwards/MinimalApiPlayground)
- [Scott Hanselman - Minimal APIs in .NET 6 but where are the Unit Tests?](https://www.hanselman.com/blog/minimal-apis-in-net-6-but-where-are-the-unit-tests)
- [Andrew Lock - Reducing log verbosity with Serilog RequestLogging](https://andrewlock.net/using-serilog-aspnetcore-in-asp-net-core-3-reducing-log-verbosity/)
