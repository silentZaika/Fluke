using Fluke.Core.Parsers;

namespace Fluke.Core.Service;

public interface IParserResolver
{
    public ITestResultParser Resolve(string rawDataFormat);
}