using Fluke.Core.Model;

namespace Fluke.Core.Service;

public interface ITestResultRepository
{
    // Task StoreTestResultAsync(Model.TestResult testResult);
    // Task StoreTestResultsAsync(List<Model.TestResult> testResults);
    Task StoreTestRunAsync(TestRun testRun);
}