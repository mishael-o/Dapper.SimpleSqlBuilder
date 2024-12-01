# Documentation

The documentation is powered by [Docfx](https://dotnet.github.io/docfx/). Docs are written in Markdown and is located in the `docs` folder while the API Docs are generated with Docfx. The documentation is built and deployed to GitHub Pages using GitHub Actions.

## How to build the documentation

### Prerequisites

- Install the .NET Core SDK that the project is using.
- Install [Docfx](https://dotnet.github.io/docfx/). The project version can be found in [ci-cd.yml](../../.github/workflows/ci-cd.yml) file.
- Bash. If you are on Windows, you can use Git Bash or WSL.

To build the documentation run the following command in the github-pages folder:

```bash
_scripts/local-doc-gen.sh
```

The script will perform some clean up, run `docfx` to generate the documentation and then serve it on `http://localhost:8080`. You can then preview the documentation in your browser.

To serve the documentation with a different port, you can pass the port number as an argument to the script:

```bash
_scripts/local-doc-gen.sh 8081
```
