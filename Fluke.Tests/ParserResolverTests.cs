using Fluke.Core.Parsers;
using Fluke.Core.Service;
using Moq;

namespace FlukeTests;

public class ParserResolverTests
{
    [Test]
    [TestCase("xml")]
    [TestCase(" xml ")]
    [TestCase("XML ")]
    public void Resolve_XmlFormat_ReturnsXmlParser(string format)
    {
        var xmlParserMock = new Mock<NunitXmlTestResultParser>();
        var trxParserMock = new Mock<NunitTrxTestResultParser>();
        var resolver = new ParserResolver(trxParserMock.Object, xmlParserMock.Object);

        var returnedParser = resolver.Resolve(format);
        
        Assert.That(returnedParser, Is.EqualTo(xmlParserMock.Object));
    }
    
    [Test]
    [TestCase("trx")]
    [TestCase(" trx ")]
    [TestCase(" TRX")]
    public void Resolve_TrxFormat_ReturnsTrxParser(string format)
    {
        var xmlParserMock = new Mock<NunitXmlTestResultParser>();
        var trxParserMock = new Mock<NunitTrxTestResultParser>();
        var resolver = new ParserResolver(trxParserMock.Object, xmlParserMock.Object);

        var returnedParser = resolver.Resolve(format);
        
        Assert.That(returnedParser, Is.EqualTo(trxParserMock.Object));
    }

    [Test]
    public void Resolve_UnsupportedFormat_Throws()
    {
        var xmlParserMock = new Mock<NunitXmlTestResultParser>();
        var trxParserMock = new Mock<NunitTrxTestResultParser>();
        var resolver = new ParserResolver(trxParserMock.Object, xmlParserMock.Object);

        Assert.Throws<NotSupportedException>(() => resolver.Resolve("test"));
    }
}