using System.Globalization;
using System.Xml;
using Fluke.Core.Parsers;

namespace FlukeTests;

public class NunitXmlParserTests
{
    [Test]
    [TestCase("")]
    [TestCase("<test>someData</wrong_tag>")]
    public void Parse_ThrowsOnInvalidRawData(string testData)
    {
        var parser = new NunitXmlTestResultParser();

        Assert.That(() => parser.Parse(testData), 
            Throws.InstanceOf<XmlException>());
    }

    [Test]
    public async Task Parse_ReturnsTestRun()
    {
        var parser = new NunitXmlTestResultParser();

        var rawData = await File.ReadAllTextAsync("Assets/Nunit/nunit_test_results.xml");
        var parsedData = parser.Parse(rawData);
  
        using (Assert.EnterMultipleScope())
        {
            Assert.That(parsedData.TestRunName, Is.EqualTo("Fluke.Tests.dll"));
            Assert.That(parsedData.Duration, Is.EqualTo(0.087d).Within(0.001d));
            Assert.That(parsedData.Failed, Is.EqualTo(1));
            Assert.That(parsedData.Passed, Is.EqualTo(4));
            Assert.That(parsedData.Total, Is.EqualTo(5));
            Assert.That(parsedData.StarTime, Is.EqualTo(DateTime.Parse("2025-06-30T21:52:56Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)));
            Assert.That(parsedData.TestResults, Has.Count.EqualTo(5));
        }
    }

    [Test]
    public async Task Parse_ReturnsPassedTestResults()
    {
        var parser = new NunitXmlTestResultParser();

        var rawData = await File.ReadAllTextAsync("Assets/Nunit/nunit_test_results_one_passed.xml");
        var parsedData = parser.Parse(rawData);
        
        Assert.That(parsedData.TestResults, Has.Count.EqualTo(2));
        using (Assert.EnterMultipleScope())
        {
            var testResult = parsedData.TestResults.First();
            Assert.That(testResult.ClassName, Is.EqualTo("NunitXmlParserTests"));
            Assert.That(testResult.Duration, Is.EqualTo(0.017d).Within(0.001d));
            Assert.That(testResult.FailureMessage, Is.Null);
            Assert.That(testResult.StackTrace, Is.Null);
            Assert.That(testResult.Status, Is.EqualTo("Passed"));
            Assert.That(testResult.TestRunId, Is.EqualTo(0));
        }
    }
    
    [Test]
    public async Task Parse_ReturnsFailedTestResults()
    {
        var parser = new NunitXmlTestResultParser();

        var rawData = await File.ReadAllTextAsync("Assets/Nunit/nunit_test_results_one_failed.xml");
        var parsedData = parser.Parse(rawData);
        
        Assert.That(parsedData.TestResults, Has.Count.EqualTo(1));
        using (Assert.EnterMultipleScope())
        {
            var testResult = parsedData.TestResults.First();
            Assert.That(testResult.ClassName, Is.EqualTo("NunitXmlParserTests"));
            Assert.That(testResult.Duration, Is.EqualTo(0.065d).Within(0.001d));
            Assert.That(testResult.FailureMessage,
                Contains.Substring(
                        "Assert.That(parsedData.StarTime, Is.EqualTo(DateTime.Parse(\"2025-04-21 14:00:00Z\")))")
                        .And.Contains("Expected: 2025-04-21 16:00:00")
                        .And.Contains("But was:  2025-04-21 14:00:00"));
            Assert.That(testResult.StackTrace,
                Contains.Substring(
                    "at FlukeTests.NunitXmlParserTests.Parse_ReturnsTestRun() in /Users/elena/dev/Fluke/Fluke.Tests/NunitXMLParserTests.cs:line 36")
                            .And.Contains("at FlukeTests.NunitXmlParserTests.Parse_ReturnsTestRun()"));
            Assert.That(testResult.Status, Is.EqualTo("Failed"));
            Assert.That(testResult.TestRunId, Is.EqualTo(0));
        }
    }
}