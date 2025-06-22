using System.Xml;
using Fluke.Core.Model;
using Fluke.Core.Service;
using Fluke.CollectorAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FlukeTests;

public class TestResultControllerTests
{
    [Test]
    public async Task UploadRawTestResultAsync_ReturnsOk()
    {
        var mockService = new Mock<ITestResultService>();
        var controller = new TestResultController(mockService.Object);

        const string rawData = "<test><result>pass</result></test>";
        var requestData = new RawTestResult(rawData, "xml", "commit");
        
        var result = await controller.UploadRawTestResultAsync(requestData) as OkObjectResult;

        Assert.That(result!.StatusCode, Is.EqualTo(200));
        Assert.That(result!.Value!.ToString(), 
            Is.EqualTo("{ status = success }"));
    }

    private static readonly string?[] EmptyXmlInputs = [string.Empty, null];
    [Test]
    [TestCaseSource(nameof(EmptyXmlInputs))]
    public async Task UploadRawTestResultAsync_OnEmptyString_ReturnsBadRequest(string emptyXml)
    {
        var mockService = new Mock<ITestResultService>();
        var controller = new TestResultController(mockService.Object);
        var requestData = new RawTestResult(emptyXml, "xml", "commit");
        
        var result = await controller.UploadRawTestResultAsync(requestData) as BadRequestObjectResult;

        Assert.That(result!.Value, Is.EqualTo("Test results are missing!"));
    }
    
    [Test]
    public async Task UploadRawTestResultAsync_OnInvalid_ReturnsBadRequest()
    {
        var mockService = new Mock<ITestResultService>();
        mockService.Setup(s => s.ProcessTestResultAsync(It.IsAny<RawTestResult>()))
            .ThrowsAsync(new XmlException());
        var controller = new TestResultController(mockService.Object);
        var requestData = new RawTestResult("<test><result>fail", "xml", "commit");
        
        var result = await controller.UploadRawTestResultAsync(requestData) as BadRequestObjectResult;
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result!.StatusCode, Is.EqualTo(400));
            Assert.That(result!.Value, Is.EqualTo("Error processing test results: An XML error has occurred."));
        }
    }

    [Test]
    [TestCase("xml")]
    [TestCase("trx")]
    public async Task UploadRawTestResultXmlAsync(string format)
    {
        var mockService = new Mock<ITestResultService>();
        var controller = new TestResultController(mockService.Object);

        const string rawData = "some raw test result data";
        const string commitHash = "someCommit";
        var requestData = new RawTestResult(rawData, format, commitHash);

        var result = await controller.UploadRawTestResultAsync(requestData) as OkObjectResult;

        mockService.Verify(v => v.ProcessTestResultAsync(
            It.Is<RawTestResult>(r =>
                r.Format == format &&
                r.RawTestData == rawData && 
                r.Commit == commitHash)), Times.Once);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }
}