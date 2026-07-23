using System.Diagnostics;
using System.Net;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace ProductApi.Tests;

public sealed class ContainerizedApiTests : IAsyncLifetime
{
    private IContainer? _container;

    public async Task InitializeAsync()
    {
        if (!IsDockerAvailable())
        {
            return;
        }

        var solutionDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        var image = new ImageFromDockerfileBuilder()
            .WithName("product-api-tests")
            .WithDockerfileDirectory(solutionDirectory)
            .WithDockerfile("src/ProductApi/Dockerfile")
            .Build();

        await image.CreateAsync();

        _container = new ContainerBuilder()
            .WithImage(image)
            .WithName($"product-api-tests-{Guid.NewGuid():N}")
            .WithEnvironment("ASPNETCORE_URLS", "http://+:8080")
            .WithExposedPort(8080)
            .WithPortBinding(8080, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(
                strategy => strategy.ForPort(8080).ForPath("/health"),
                _ => { }))
            .Build();

        await _container.StartAsync();
    }

    [Fact]
    public async Task HealthEndpoint_ShouldRespondWithOk_WhenContainerStarts()
    {
        if (_container is null)
        {
            return;
        }

        using var client = new HttpClient();
        var port = _container.GetMappedPublicPort(8080);
        var response = await client.GetAsync($"http://127.0.0.1:{port}/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    public async Task DisposeAsync()
    {
        if (_container is not null)
        {
            await _container.DisposeAsync();
        }
    }

    private static bool IsDockerAvailable()
    {
        try
        {
            using var process = Process.Start(new ProcessStartInfo("docker", "info")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            });
            process?.WaitForExit(10000);
            return process?.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}
