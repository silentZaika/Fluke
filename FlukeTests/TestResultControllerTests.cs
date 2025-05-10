using System.Xml;
using FlukeCollectorAPI.Controllers;
using FlukeCollectorAPI.Model;
using FlukeCollectorAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FlukeTests;

public class TestResultControllerTests
{
    [Test]
    public async Task UploadRawTestResultXmlAsync_ReturnsOk()
    {
        var mockService = new Mock<ITestResultService>();
        var controller = new TestResultController(mockService.Object);

        const string rawData = "<test><result>pass</result></test>"; 
        
        var result = await controller.UploadRawTestResultXmlAsync(rawData) as OkObjectResult;

        Assert.That(result!.StatusCode, Is.EqualTo(200));
        Assert.That(result!.Value!.ToString(), 
            Is.EqualTo("{ status = success }"));
    }

    private static readonly string?[] EmptyXmlInputs = [string.Empty, null];
    [Test]
    [TestCaseSource(nameof(EmptyXmlInputs))]
    public async Task UploadRawTestResultXmlAsync_OnEmptyString_ReturnsBadRequest(string emptyXml)
    {
        var mockService = new Mock<ITestResultService>();
        var controller = new TestResultController(mockService.Object);

        var result = await controller.UploadRawTestResultXmlAsync(emptyXml) as BadRequestObjectResult;

        Assert.That(result!.Value, Is.EqualTo("Test results are missing!"));
    }
    
    [Test]
    public async Task UploadRawTestResultXmlAsync_OnInvalid_ReturnsBadRequest()
    {
        var mockService = new Mock<ITestResultService>();
        mockService.Setup(s => s.ProcessTestResultAsync(It.IsAny<RawTestResult>()))
            .ThrowsAsync(new XmlException());
        var controller = new TestResultController(mockService.Object);
        
        var result = await controller.UploadRawTestResultXmlAsync("<test><result>fail") as BadRequestObjectResult;
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result!.StatusCode, Is.EqualTo(400));
            Assert.That(result!.Value, Is.EqualTo("Error processing test results: An XML error has occurred."));
        }
    }
}