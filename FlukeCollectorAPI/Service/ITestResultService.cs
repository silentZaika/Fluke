namespace FlukeCollectorAPI.Service;

public interface ITestResultService
{
    Task ProcessTestResultAsync(RawTestResult result);
}