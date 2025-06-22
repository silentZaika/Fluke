using Fluke.Core.Model;
using Fluke.Core.Service;

namespace Fluke.Analytics;

public class UnstabilityDetector
{
    private readonly ITestResultRepository _repository;

    public UnstabilityDetector(ITestResultRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TestResult>> GetUnstableTestsAsync()
    {
        throw new NotImplementedException();
    }
}