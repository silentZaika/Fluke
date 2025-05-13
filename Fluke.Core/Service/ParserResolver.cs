using Fluke.Core.Parsers;

namespace Fluke.Core.Service;

public class ParserResolver(NunitTrxTestResultParser trxParser, NunitXmlTestResultParser xmParser)
    : IParserResolver
{
    public ITestResultParser Resolve(string rawDataFormat)
    {
        return rawDataFormat.ToLower().Trim() switch
        {
            "xml" => xmParser,
            "trx" => trxParser,
            _ => throw new NotSupportedException($"Test result format '{rawDataFormat}' is not supported")
        };
    }
}