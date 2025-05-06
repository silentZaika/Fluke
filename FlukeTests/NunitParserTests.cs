using System.Xml;
using FlukeCollectorAPI.Parsers;

namespace FlukeTests;

public class NunitParserTests
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
            Assert.That(parsedData.TestRunName, Is.EqualTo("MyProject.Tests.dll"));
            Assert.That(parsedData.Duration, Is.EqualTo(3.456d));
            Assert.That(parsedData.Failed, Is.EqualTo(1));
            Assert.That(parsedData.Passed, Is.EqualTo(2));
            Assert.That(parsedData.Total, Is.EqualTo(3));
            Assert.That(parsedData.StarTime, Is.EqualTo(DateTime.Parse("2025-04-21 14:00:00Z")));
            Assert.That(parsedData.TestResults, Has.Count.EqualTo(3));
        }
    }

    [Test]
    public async Task Parse_ReturnsTestResults()
    {
        var parser = new NunitXmlTestResultParser();

        var rawData = await File.ReadAllTextAsync("Assets/Nunit/nunit_test_results_one_passed.xml");
        var parsedData = parser.Parse(rawData);
        
        Assert.That(parsedData.TestResults, Has.Count.EqualTo(1));
        using (Assert.EnterMultipleScope())
        {
            var testResult = parsedData.TestResults.First();
            Assert.That(testResult.ClassName, Is.EqualTo("MyProject.Tests.CalculatorTests"));
            Assert.That(testResult.Duration, Is.EqualTo(0.001d));
            Assert.That(testResult.FailureMessage, Is.Null);
            Assert.That(testResult.StackTrace, Is.Null);
            Assert.That(testResult.Status, Is.EqualTo("Passed"));
            Assert.That(testResult.TestRunId, Is.EqualTo(0));
        }
    }
}