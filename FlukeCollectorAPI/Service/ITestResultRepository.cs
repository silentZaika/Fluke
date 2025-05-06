using FlukeCollectorAPI.Model;

namespace FlukeCollectorAPI.Service;

public interface ITestResultRepository
{
    // Task StoreTestResultAsync(Model.TestResult testResult);
    // Task StoreTestResultsAsync(List<Model.TestResult> testResults);
    Task StoreTestRunAsync(TestRun testRun);
}