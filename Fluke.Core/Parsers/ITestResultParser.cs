using Fluke.Core.Model;

namespace Fluke.Core.Parsers;
public interface ITestResultParser
{
    public TestRun Parse(string rawResult);
}
