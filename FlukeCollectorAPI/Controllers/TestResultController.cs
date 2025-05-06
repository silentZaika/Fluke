using System.Net.Mime;
using System.Xml.Linq;
using FlukeCollectorAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace FlukeCollectorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestResultController : ControllerBase
{
    private readonly ITestResultService _testResultService;

    public TestResultController(ITestResultService testResultService)
    {
        _testResultService = testResultService;
    }
    
    [HttpPost("uploadJsonResult")]
    [Consumes("application/json")]
    public async Task<IActionResult> UploadTestResultJsonAsync([FromBody]object rawData)
    {
        var contentType = Request.ContentType;
        var jsonData = rawData.ToString();
        Console.WriteLine(jsonData);
        return Ok(new { status = "success", contentType });
    }
    
    [HttpPost("uploadXmlResult")]
    [Consumes(MediaTypeNames.Text.Plain)]
    public async Task<IActionResult> UploadRawTestResultXmlAsync([FromBody] string? rawData)
    {
        if (string.IsNullOrEmpty(rawData))
        {
            return BadRequest("Test results are missing!");
        }

        var rawTestResult = new RawTestResult(rawData);
        try
        {
            await _testResultService.ProcessTestResultAsync(rawTestResult);
        }
        catch (Exception e)
        {
            // throw;
            return BadRequest($"Error processing test results: {e.Message}" ); 
        }
        // await _testResultService.ProcessTestResultAsync(rawTestResult);
        
        // var xDoc = XDocument.Parse(rawData);
        
        return Ok(new { status = "success" });
    }
}