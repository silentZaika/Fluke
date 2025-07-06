using System.Net;
using System.Net.Http.Json;
using DotNet.Testcontainers.Containers;
using Fluke.Tests.Integration.Fixtures;
using Fluke.Tests.Integration.Utilities;

namespace Fluke.Tests.Integration.Tests;

[TestFixture, Category("Integration")]
public class CollectorApiTests
{
    private ushort _port;
    private CollectorApiFixture _fixture;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _fixture = new CollectorApiFixture();
        _port = await _fixture.InitializeAsync();
    }
    
    [Test]
    public async Task HealthCheck_Returns200()
    {
        var url = $"http://localhost:{_port}/health";
        using var client = HttpClientFactory.Create();
        var response = await client.GetAsync(url);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var content = await response.Content.ReadFromJsonAsync<string>();// .ReadAsStringAsync();
        Assert.That(content, Is.EqualTo("Healthy"));
    }
}