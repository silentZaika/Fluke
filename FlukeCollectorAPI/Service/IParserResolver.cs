using FlukeCollectorAPI.Parsers;

namespace FlukeCollectorAPI.Service;

public interface IParserResolver
{
    public ITestResultParser Resolve(string rawDataFormat);
}