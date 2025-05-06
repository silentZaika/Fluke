using FlukeCollectorAPI.Model;
using FlukeCollectorAPI.Service;
using Microsoft.EntityFrameworkCore;

namespace FlukeTests;

[TestFixture]
public class TestResultRepositoryTests
{
    private ApplicationDbContext _context = null!;
    private TestResultRepository _repository = null!;
    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureDeleted();

        _repository = new TestResultRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task StoreTestRunAsync_AddsTestResult()
    {
        var testResult = new TestResult {ClassName = "test", Duration = 777f, Status = "passed", TestName = "testname"};
        var testRun = new TestRun { TestRunName = "testRunName", TestResults = [testResult] }; 
        
        await _repository.StoreTestRunAsync(testRun);
            
        Assert.That( _context.TestResults, Has.One.Items);
        Assert.That(_context.TestResults.First(), Is.EqualTo(testResult));
    }
    
    [Test]
    public async Task StoreTestRunAsync_AddsMultipleTestResult()
    {
        var testResultA = new TestResult {ClassName = "test", Duration = 777f, Status = "passed", TestName = "testname"};
        var testResultB = new TestResult {ClassName = "test2", Duration = 77, Status = "failed", TestName = "testname2"};
        var testResultC = new TestResult {ClassName = "test3", Duration = 7, Status = "passed", TestName = "testname3"};
        var testRun = new TestRun { TestRunName = "testRunName", TestResults = [testResultA, testResultB, testResultC] };

        await _repository.StoreTestRunAsync(testRun);
            
        Assert.That( await _context.TestResults.CountAsync(), Is.EqualTo(3));
        Assert.That(_context.TestResults, Is.EquivalentTo(testRun.TestResults));
    }
}