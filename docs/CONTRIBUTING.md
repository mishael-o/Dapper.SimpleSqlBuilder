# Contributing

Thinking of contributing to the project? Super 😊, we are grateful and excited for the support. When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change. You can also pick an existing issue.

Please note we have a [Code of Conduct](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/blob/main/docs/CODE_OF_CONDUCT.md), please follow it in all your interactions with the project.

## Project Prerequisites

- Visual Studio 2022 is the recommended IDE.
- Docker is required if you would like to run the integration tests locally.

## Change Guidelines

- Ensure you follow style guidelines of this project when writing code.
- Ensure unit tests are written or updated for the feature or bugfix.
- Add code comments if necessary to hard-to-understand code.
- Add documentation comments to new public APIs created.
- Document new feature in project ReadMe if necessary.

## Pull requests

1. Fork the repository.
2. Create a branch to work with and use `feature/` or `bugfix/` as a prefix.
3. Implement change
4. Once done, rebase and create a pull request.

### Tests

To run the tests you can run them via Visual Studio's Test explorer or via the .NET CLI.

**Note**: If you don't have Docker installed, the integration tests will fail as we use [TestContainers](https://github.com/testcontainers/testcontainers-dotnet) to run them. You can either not run them or install Docker to run them locally.
