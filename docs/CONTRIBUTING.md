# Contributing

Thinking of contributing to the project? Super 😊, we are grateful and excited for the support. Give this [project](https://github.com/mishael-o/Dapper.SimpleSqlBuilder) a star ⭐️ and share it with your friends on social media.

When contributing to this repository, please first discuss the change you wish to make via an issue, email, or any other method with the owners of this repository before making a change. You can also pick an existing issue.

Please note we have a [Code of Conduct](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/blob/main/docs/CODE_OF_CONDUCT.md), follow it in all your interactions with the project.

## Project Prerequisites

- Visual Studio 2022 or JetBrains Rider is preferred, however any modern .NET IDE can also be used.
- Docker is required to run the integration tests locally.

## Change Guidelines

- Ensure you follow the style guidelines of this project when writing code.
- Ensure unit tests are written or updated for the feature, bugfix or hotfix.
- Add code comments if necessary to hard-to-understand code.
- Add documentation comments to new public APIs created.
- Document new features in project `ReadMe` if necessary.

## Pull requests

1. Fork the repository.
2. Create a branch from `develop` to work with and use `feature/`, `bugfix/` or `hotfix/` as a prefix.
3. Implement change
4. Once done, rebase from `develop` and create a pull request to merge into `develop`.

### Tests

To run the tests, you can run them via your IDE test explorer or via the .NET CLI.

**Note**: If you don't have Docker installed, the integration tests will fail as we use [TestContainers](https://github.com/testcontainers/testcontainers-dotnet) to run them. You can either not run them or install Docker to run them locally.

## For Maintainers

**Git Flow**

- New features, enhancements and bug fixes should be branched off from `develop` branch.
- `hotfix` branches should be branched off from `main` branch.
- All pull request(s) into `develop` branch should be completed with a `squash merge`.
- Only `release/*` or a `hotfix/*` branch should be merged (`merge commit`) into `main`
- Once `develop` has reached a release milestone, create a `release` branch from `develop`, setup a pull request, and then merge (`merge commit`) into `main`. Finally delete `release` branch.
- Once a stable release has been published, `main` should be merged (`merge commit`) into `develop`.


**Important Pull Request Labels**

The following labels are used to categorize pull requests and define what is documented in the release. See [release.yml](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/blob/main/.github/release.yml) for release docs configuration.

- `hotfix` - Pull request fixes a major bug in the latest release and needs to be merged ASAP.
- `breaking-change` - Pull request that adds a new feature, bugfix or enhancement which introduces a breaking change.
- `feature` - Pull request adds a new feature.
- `bugfix` - Pull request fixes a bug.
- `enhancement` - Pull request improves an existing feature.
- `documentation` - Pull request updates documentation.
- `ci-cd` - Pull request improves the CI/CD pipeline.
- `ignore-for-release` - Pull request should not be included in the release docs.

Click [here](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/labels) to view the full list of label.