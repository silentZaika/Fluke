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

    [Test]
    public async Task SendValidTestResults_Returns200()
    {
        var payload = new
        {
            commit = "abc123",
            format = "trx",
            rawTestData = "<TestRun><Results><Test /></Results></TestRun>"
        };
        
        _pactBuilder.UponReceiving("a valid test result upload")
            .WithRequest(HttpMethod.Post, "/api/TestResult/upload")
            .WithJsonBody(payload)
            .WillRespond()
            .WithStatus(HttpStatusCode.OK);

        await _pactBuilder.VerifyAsync(async ctx =>
        {
            var client = new HttpClient() { BaseAddress = ctx.MockServerUri };
            var response = await client.PostAsync("/api/TestResult/upload", 
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, MediaTypeNames.Application.Json));
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });
    }
}