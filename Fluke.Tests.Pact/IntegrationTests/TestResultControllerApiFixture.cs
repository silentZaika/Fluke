using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FlukeTests.IntegrationTests;

public class TestResultControllerApiFixture : IDisposable
{
    private readonly IHost _server;
    public Uri ServerUri { get; }

    public TestResultControllerApiFixture()
    {
        ServerUri = new Uri("https://localhost:7173");
        _server = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls(ServerUri.ToString());
                webBuilder.UseStartup<TestStartup>();
            })
            .Build();
        _server.Start();
    }

    public void Dispose()
    {
        _server.Dispose();
    }
}