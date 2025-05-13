using Fluke.Core.Model;
using Fluke.Core.Parsers;
using Fluke.Core.Service;
using Moq;

namespace FlukeTests;

public class TestResultServiceTests
{
    [Test]
    public async Task ProcessTestResultAsync_StoresParsedResults()
    {
        var repositoryMock = new Mock<ITestResultRepository>();
        var parserMock = new Mock<ITestResultParser>();
        var resolverMock = new Mock<IParserResolver>();
        var service = new TestResultService(repositoryMock.Object, resolverMock.Object);
        
        var rawResults = new RawTestResult("rawResult", "xml");
        var parsedResult = new TestRun() { TestRunName = "parsedTest" };
        parserMock.Setup(p => p.Parse("rawResult")).Returns(parsedResult);
        resolverMock.Setup(r => r.Resolve(It.IsAny<string>())).Returns(parserMock.Object);
        
        await service.ProcessTestResultAsync(rawResults);
        
        repositoryMock.Verify(e => e.StoreTestRunAsync(parsedResult), Times.Once);
    }
}