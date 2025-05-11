using System.Net.Mime;
using FlukeCollectorAPI.Model;
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
    
    [HttpPost("upload/{format}")]
    [Consumes(MediaTypeNames.Text.Plain)]
    public async Task<IActionResult> UploadRawTestResultAsync(string format, [FromBody] string? rawData)
    {
        if (string.IsNullOrEmpty(rawData)) 
            return BadRequest("Test results are missing!");
        
        var rawTestResult = new RawTestResult(rawData, format);
        try
        {
            await _testResultService.ProcessTestResultAsync(rawTestResult);
        }
        catch (Exception e)
        {
            return BadRequest($"Error processing test results: {e.Message}" ); 
        }
        
        return Ok(new { status = "success" });
    }
}