using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Verifier;

namespace FlukeTests.IntegrationTests;

public class TestResultControllerTest
{
    private TestResultControllerApiFixture _fixture;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _fixture = new TestResultControllerApiFixture();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _fixture.Dispose();
    }

    [Test]
    public void Controlled_Honours_Pact_WithConsumers()
    {
        var config = new PactVerifierConfig()
        {
            Outputters = new List<IOutput> { new ConsoleOutput() },
            LogLevel = PactLogLevel.Debug
        };

        var pactPath = Path.Combine(@"./pacts", "CliUploader-TestResultController.json");
        Console.WriteLine(pactPath);
        var pactVerifier = new PactVerifier("TestResultControllerAPI", config);
        pactVerifier
            .WithHttpEndpoint(_fixture.ServerUri)
            .WithFileSource(new FileInfo(pactPath))
            .WithProviderStateUrl(new Uri(_fixture.ServerUri, "/provider-states"))
            .Verify();
        
    }
}