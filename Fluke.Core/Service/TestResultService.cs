using Fluke.Core.Model;

namespace Fluke.Core.Service;

public class TestResultService(
    ITestResultRepository testResultRepository, IParserResolver resolver) : ITestResultService
{
    public async Task ProcessTestResultAsync(RawTestResult rawTestData)
    {
        var parser = resolver.Resolve(rawTestData.Format);
        var testRun = parser.Parse(rawTestData.RawTestData);
        testRun.CommitHash = rawTestData.Commit;
        
        //TODO: Store the processed results
        await testResultRepository.StoreTestRunAsync(testRun);
    }
}