# Contributing to Clean Minimal API

Thanks for taking the time to contribute. Bug reports, ideas, documentation fixes and pull requests are all welcome.

## Before you start

- Read and follow the [code of conduct](CODE_OF_CONDUCT.md).
- Search the [existing issues](https://github.com/stphnwlsh/CleanMinimalApi/issues) to see if your bug or idea is already being discussed.
- For anything larger than a small fix, open an issue first so we can agree on the approach before you write the code.
- Report security vulnerabilities privately, as described in the [security policy](SECURITY.md), not in a public issue.

## One template in a family of five

This repository is one of five templates that share everything except the Presentation layer:

- [Clean Minimal API](https://github.com/stphnwlsh/CleanMinimalApi)
- [Clean Model Context Protocol](https://github.com/stphnwlsh/CleanModelContextProtocol)
- [Clean Web API](https://github.com/stphnwlsh/CleanWebApi)
- [Clean GraphQL API](https://github.com/stphnwlsh/CleanGraphQLApi)
- [Clean Lambda Function](https://github.com/stphnwlsh/CleanLamdaFunction)

If your change touches the Application or Infrastructure layers, their tests, the build pipeline or the repository setup, it needs to be made in all five. Mention that in your pull request and the maintainer will carry it across.

## Development setup

1. Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0). Docker is optional but needed to build the container image.
1. Fork the repository and clone your fork.
1. Build and run the tests.

   ``` bash
   dotnet build
   dotnet test
   ```

1. Run the project as described in [docs/RUN.md](docs/RUN.md).

### Testing the template itself

To check that the template still installs and produces a working solution, install it from your local clone and create a solution from it.

``` bash
dotnet new install ./ --force
dotnet new cleanminimalapi --name Sample --output ../Sample
dotnet build ../Sample
dotnet new uninstall ./
```

## Making a change

1. Create a branch from `main` named `feature/`, `fix/` or `chore/` followed by a short description, for example `fix/review-validation`.
1. Keep the change focused. Unrelated refactors are easier to review as separate pull requests.
1. Add or update tests. The pipeline fails if line coverage drops below 95%, or if less than 90% of the lines your pull request changes are covered.
1. Follow the style in `.editorconfig` and `Directory.Build.props`. Warnings are treated as errors, so the build must be clean.
1. Update the README or docs if behaviour changes.

## Pull requests

1. Open the pull request against `main` and fill in the template.
1. Link the issue it resolves, for example `Closes #123`.
1. Every check must pass: Build and Test, Code Coverage, Container Scan and Static Analysis.
1. Pull requests are squash-merged once they're approved and green.

## Getting help

See [SUPPORT.md](SUPPORT.md).
