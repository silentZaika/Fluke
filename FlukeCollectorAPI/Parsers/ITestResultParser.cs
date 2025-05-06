using FlukeCollectorAPI.Model;

namespace FlukeCollectorAPI.Parsers;
public interface ITestResultParser
{
    public TestRun Parse(string rawResult);
}
