using System.Net.Mime;
using Fluke.Core.Model;
using Fluke.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace Fluke.CollectorAPI.Controllers;

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
    public async Task<IActionResult> UploadTestResultJsonAsync([FromBody] object rawData)
    {
        var contentType = Request.ContentType;
        var jsonData = rawData.ToString();
        Console.WriteLine(jsonData);
        return Ok(new { status = "success", contentType });
    }

    [HttpPost("upload")]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> UploadRawTestResultAsync([FromBody] RawTestResult request)
    {
        if (string.IsNullOrEmpty(request.RawTestData))
            return BadRequest("Test results are missing!");

        var rawTestResult = new RawTestResult(request.RawTestData, request.Format, request.Commit);
        try
        {
            await _testResultService.ProcessTestResultAsync(rawTestResult);
        }
        catch (Exception e)
        {
            return BadRequest($"Error processing test results: {e.Message}");
        }

        return Ok(new { status = "success" });
    }
}