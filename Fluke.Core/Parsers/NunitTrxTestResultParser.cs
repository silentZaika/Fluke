using System.Xml.Linq;
using Fluke.Core.Model;

namespace Fluke.Core.Parsers;

public class NunitTrxTestResultParser : ITestResultParser
{
    public TestRun Parse(string rawResult)
    {
        var doc = XDocument.Parse(rawResult);
        var ns = doc.Root?.Name.Namespace;
        
        var testRunNode = doc.Element(ns + "TestRun");
        var counterNode = testRunNode?.Descendants(ns + "Counters").FirstOrDefault();
        var timesNode = testRunNode?.Descendants(ns + "Times").FirstOrDefault();
        
        var start = DateTime.Parse(timesNode?.Attribute("start")?.Value);
        var finish = DateTime.Parse(timesNode?.Attribute("finish")?.Value);
        var duration = Math.Round((finish - start).TotalSeconds, 3);
        var testRun = new TestRun() 
        {
            TestRunName = testRunNode?.Attribute("name")?.Value,
            Total = int.Parse(counterNode?.Attribute("total")?.Value),
            Passed = int.Parse(counterNode?.Attribute("passed")?.Value),
            Failed = int.Parse(counterNode?.Attribute("failed")?.Value),
            StarTime = start,
            Duration = duration
        };

        var results = new List<TestResult>();
        var testCasesNode = doc.Descendants(ns + "UnitTestResult");
        var testMethods = doc.Descendants(ns + "TestMethod");
        
        foreach (var testCase in testCasesNode)
        {
            var testName = testCase.Attribute("testName")?.Value;
      
            var result = new TestResult()
            {
                TestRunId = testRun.Id,
                TestName = testName,
                ClassName = testMethods.First(tm => (tm.Attribute("name").Value == testName))
                    .Attribute("className")?.Value,
                Duration = Math.Round(TimeSpan.Parse(testCase.Attribute("duration")?.Value).TotalSeconds, 3),
                Status = testCase.Attribute("outcome")?.Value,
                FailureMessage = testCase.Descendants(ns + "Message").FirstOrDefault()?.Value,
                StackTrace = testCase.Descendants(ns + "StackTrace").FirstOrDefault()?.Value
            };
            
            results.Add(result);
        }
        
        testRun.TestResults = results;
        return testRun;
    }
}