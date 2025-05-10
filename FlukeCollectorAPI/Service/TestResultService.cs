using FlukeCollectorAPI.Model;

namespace FlukeCollectorAPI.Service;

public class TestResultService(
    ITestResultRepository testResultRepository, IParserResolver resolver) : ITestResultService
{
    public async Task ProcessTestResultAsync(RawTestResult result)
    {
        var parser = resolver.Resolve(result.Format);
        var testRun = parser.Parse(result.RawResult);
        

        //TODO: Store the processed results
        await testResultRepository.StoreTestRunAsync(testRun);
    }
}