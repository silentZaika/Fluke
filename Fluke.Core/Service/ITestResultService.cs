using Fluke.Core.Model;

namespace Fluke.Core.Service;

public interface ITestResultService
{
    Task ProcessTestResultAsync(RawTestResult result);
}