using System.Globalization;
using System.Xml.Linq;
using Fluke.Core.Model;

namespace Fluke.Core.Parsers;
public class NunitXmlTestResultParser : ITestResultParser
{
    public TestRun Parse(string rawResult)
    {
        var doc = XDocument.Parse(rawResult);

        var testRunNode = doc.Element("test-run");
        var testRun = new TestRun()
        {
            TestRunName = testRunNode?.Attribute("name")?.Value,
            Total = int.Parse(testRunNode?.Attribute("total")?.Value),
            Passed = int.Parse(testRunNode?.Attribute("passed")?.Value),
            Failed = int.Parse(testRunNode?.Attribute("failed")?.Value),
            StarTime = DateTime.Parse(testRunNode?.Attribute("start-time")?.Value),
            Duration = double.Parse(testRunNode?.Attribute("duration")?.Value, CultureInfo.InvariantCulture)
        };

        var results = new List<TestResult>();
        var testCasesNode = doc.Descendants("test-case");
        foreach (var testCase in testCasesNode)
        {
            var result = new TestResult()
            {
                TestRunId = testRun.Id,
                TestName = testCase.Attribute("name")?.Value,
                ClassName = testCase.Attribute("classname")?.Value,
                Duration = double.Parse(testCase.Attribute("duration")?.Value, CultureInfo.InvariantCulture),
                Status = testCase.Attribute("result")?.Value,
                FailureMessage = testCase.Element("failure")?.Element("message")?.Value,
                StackTrace = testCase.Element("failure")?.Element("stack-trace")?.Value
            };
            results.Add(result);
        }

        testRun.TestResults = results;
        return testRun;
    }
}