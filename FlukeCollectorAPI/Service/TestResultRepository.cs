using FlukeCollectorAPI.Model;

namespace FlukeCollectorAPI.Service;

public class TestResultRepository(ApplicationDbContext context) : ITestResultRepository
{
    // public async Task StoreTestResultAsync(Model.TestResult testResult)
    // {
    //     await context.TestResults.AddAsync(testResult);
    //     await context.SaveChangesAsync();
    // }
    //
    // public async Task StoreTestResultsAsync(List<Model.TestResult> testResults)
    // {
    //     await context.TestResults.AddRangeAsync(testResults);
    //     await context.SaveChangesAsync();
    // }
        
    public async Task StoreTestRunAsync(TestRun testRun)
    {
        await context.TestRuns.AddAsync(testRun);
        await context.SaveChangesAsync();
    }
}