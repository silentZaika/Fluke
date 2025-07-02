using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Fluke.CollectorAPI.Controllers;
using PactNet;

namespace FlukeTests.IntegrationTests;

public class CliConsumerPactTests
{
    private readonly IPactBuilderV4 _pactBuilder;

    public CliConsumerPactTests()
    {
        var pact = Pact.V4("CliUploader", nameof(TestResultController), new PactConfig()
        {
            PactDir = @"./pacts",
            LogLevel = PactLogLevel.Debug
        });
        
        _pactBuilder = pact.WithHttpInteractions();
    }

    [Test, Category("Pact")]
    public async Task SendValidTestResults_Returns200()
    {
        var payload = new
        {
            commit = "abc123",
            format = "trx",
            rawTestData = await File.ReadAllTextAsync("IntegrationTests/Assets/test_results_passed.trx")
        };
        var httpContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, MediaTypeNames.Application.Json);
        
        _pactBuilder.UponReceiving("a valid test result upload")
            .WithRequest(HttpMethod.Post, "/api/TestResult/upload")
            .WithJsonBody(payload)
            .WillRespond()
            .WithStatus(HttpStatusCode.OK);
        
        await _pactBuilder.VerifyAsync(async ctx =>
        {
            var client = new HttpClient() { BaseAddress = ctx.MockServerUri };
            var response = await client.PostAsync("/api/TestResult/upload", httpContent);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });
    }
}