# Contributing to Clean Minimal API

Thanks for taking the time to contribute. Bug reports, ideas and pull requests are all welcome.

## Reporting problems and ideas

Open an [issue](https://github.com/stphnwlsh/CleanMinimalApi/issues/new/choose) using the bug report or feature request template. For security problems, follow [SECURITY.md](SECURITY.md) instead of opening a public issue.

## Making a change

1. Fork the repository and create a branch from `main`. Name it `feature/`, `fix/` or `chore/` followed by a short description.
2. Make your change and add or update tests. The project enforces 95% code coverage.
3. Run the build and tests locally.

   ``` bash
   dotnet build
   dotnet test
   ```

4. Open a pull request against `main` and fill in the template. The build, CodeQL and coverage checks must pass before it can be merged.

## Code style

Formatting and analyzer rules live in `.editorconfig` and `Directory.Build.props`. Please follow the surrounding code and keep changes focused.

## Code of conduct

Everyone taking part is expected to follow the [code of conduct](CODE_OF_CONDUCT.md).
