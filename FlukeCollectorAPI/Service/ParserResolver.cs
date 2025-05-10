using FlukeCollectorAPI.Parsers;

namespace FlukeCollectorAPI.Service;

public class ParserResolver : IParserResolver
{
    private readonly NunitTrxTestResultParser _trxParser;
    private readonly NunitXmlTestResultParser _xmlParser;
    public ParserResolver(NunitTrxTestResultParser trxParser, NunitXmlTestResultParser xmParser)
    {
        _trxParser = trxParser;
        _xmlParser = xmParser;
    }
    
    public ITestResultParser Resolve(string rawDataFormat)
    {
        return rawDataFormat.ToLower().Trim() switch
        {
            "xml" => _xmlParser,
            "trx" => _trxParser,
            _ => throw new NotSupportedException($"Test result format '{rawDataFormat}' is not supported")
        };
    }
}