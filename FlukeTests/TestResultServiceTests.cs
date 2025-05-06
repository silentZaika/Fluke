using FlukeCollectorAPI;
using FlukeCollectorAPI.Model;
using FlukeCollectorAPI.Parsers;
using FlukeCollectorAPI.Service;
using Moq;

namespace FlukeTests;

public class TestResultServiceTests
{
    [Test]
    public async Task ProcessTestResultAsync_StoresParsedResults()
    {
        var repositoryMock = new Mock<ITestResultRepository>();
        var parserMock = new Mock<ITestResultParser>();
        var service = new TestResultService(repositoryMock.Object, parserMock.Object);
        
        var rawResults = new RawTestResult("rawResult");
        var parsedResult = new TestRun() { TestRunName = "parsedTest" };
        parserMock.Setup(p => p.Parse("rawResult")).Returns(parsedResult);
        
        await service.ProcessTestResultAsync(rawResults);
        
        repositoryMock.Verify(e => e.StoreTestRunAsync(parsedResult), Times.Once);
    }
}