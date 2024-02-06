# Documentation

The documentation is powered by [Docfx](https://dotnet.github.io/docfx/). The Docs are written in Markdown and is located in the `docs` folder while the API Docs are generated with Docfx. The documentation is automatically built and deployed to GitHub Pages using GitHub Actions.

## How to build the documentation

To build the documentation, you need to have the .NET Core SDK installed and also bash. Then you can run the following command in the github-pages folder:

```bash
_scripts/local-doc-gen.sh
```

The script will clean up the old documentation, run docfx to generate the documentation and serve it on `http://localhost:8080`. You can then preview the documentation in your browser.
