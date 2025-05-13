using System.Xml;
using Fluke.Core.Model;
using Fluke.Core.Service;
using FlukeCollectorAPI.Controllers;
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
        
        var result = await controller.UploadRawTestResultAsync("xml", rawData) as OkObjectResult;

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

        var result = await controller.UploadRawTestResultAsync("xml",emptyXml) as BadRequestObjectResult;

        Assert.That(result!.Value, Is.EqualTo("Test results are missing!"));
    }
    
    [Test]
    public async Task UploadRawTestResultAsync_OnInvalid_ReturnsBadRequest()
    {
        var mockService = new Mock<ITestResultService>();
        mockService.Setup(s => s.ProcessTestResultAsync(It.IsAny<RawTestResult>()))
            .ThrowsAsync(new XmlException());
        var controller = new TestResultController(mockService.Object);
        
        var result = await controller.UploadRawTestResultAsync("xml","<test><result>fail") as BadRequestObjectResult;
        
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

        var result = await controller.UploadRawTestResultAsync(format, rawData) as OkObjectResult;

        mockService.Verify(v => v.ProcessTestResultAsync(
            It.Is<RawTestResult>(r =>
                r.Format == format &&
                r.RawResult == rawData)), Times.Once);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }
}