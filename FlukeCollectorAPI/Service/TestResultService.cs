using FlukeCollectorAPI.Parsers;

namespace FlukeCollectorAPI.Service;

public class TestResultService : ITestResultService
{
    private readonly ITestResultRepository _repository;
    private readonly ITestResultParser _parser;
    public TestResultService(
        ITestResultRepository testResultRepository,
        ITestResultParser testResultParser)
    {
        _repository = testResultRepository;
        _parser = testResultParser;
    }
    
    public async Task ProcessTestResultAsync(RawTestResult result)
    {
        var testRun = _parser.Parse(result.RawResult);
        //TODO: Store the processed results
        await _repository.StoreTestRunAsync(testRun);
    }
}