# Product API

A minimal ASP.NET Core 10 Web API following a ports-and-adapters structure for managing products. The application uses Entity Framework Core InMemory for persistence and exposes CRUD endpoints under `/products`.

## Structure

- `src/ProductApi/` - web API application
- `test/ProductApi.Tests/` - unit and container-based integration tests

## Run locally

1. Restore dependencies:
   ```bash
   dotnet restore ProductApi.slnx
   ```
2. Run the API:
   ```bash
   dotnet run --project src/ProductApi/ProductApi.csproj --urls http://localhost:8080
   ```
3. Test the endpoints:
   ```bash
   curl http://localhost:8080/health
   curl http://localhost:8080/products
   ```

## Build and run the Docker image

From the repository root:

```bash
docker build -t product-api:latest .
docker run -p 8080:8080 product-api:latest
```

Then call:

```bash
curl http://localhost:8080/health
```

## Run tests

```bash
dotnet test ProductApi.slnx --configuration Release
```

The container-based integration test uses Testcontainers and requires Docker to be installed and running.

## CI/CD

The GitHub Actions workflow will:

- run tests on pull requests and pushes to `main`
- create a git tag matching the version found in `src/ProductApi/ProductApi.csproj`
- build and push a Docker image to GHCR
- run Trivy to scan the image for vulnerabilities
