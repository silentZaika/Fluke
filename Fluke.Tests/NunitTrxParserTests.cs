using System.Xml;
using Fluke.Core.Parsers;

namespace FlukeTests;

public class NunitTrxParserTests
{
    [Test]
    [TestCase("")]
    [TestCase("<test>someData</wrong_tag>")]
    public void Parse_ThrowsOnInvalidRawData(string testData)
    {
        var parser = new NunitTrxTestResultParser();

        Assert.That(() => parser.Parse(testData),
            Throws.InstanceOf<XmlException>());
    }

    [Test]
    public async Task Parse_TrxReturnsTestRun()
    {
        var parser = new NunitTrxTestResultParser();
        var rawData = await File.ReadAllTextAsync("Assets/Nunit/nunit_test_results.trx");

        var parsedData = parser.Parse(rawData);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(parsedData.TestRunName, Is.EqualTo("SampleTestRun 2025-05-01 19:54:26"));
            Assert.That(parsedData.Duration, Is.EqualTo(2.413d));
            Assert.That(parsedData.Failed, Is.EqualTo(1));
            Assert.That(parsedData.Passed, Is.EqualTo(1));
            Assert.That(parsedData.Total, Is.EqualTo(2));
            Assert.That(parsedData.StarTime, Is.EqualTo(DateTime.Parse("2025-05-01T19:54:24.3363230+02:00").ToUniversalTime()));
            Assert.That(parsedData.TestResults, Has.Count.EqualTo(2));
        }
    }

    [Test]
    public async Task Parse_ReturnsPassedTestResults()
    {
        var parser = new NunitTrxTestResultParser();

        var rawData = await File.ReadAllTextAsync("Assets/Nunit/nunit_test_results_one_passed.trx");
        var parsedData = parser.Parse(rawData);

        Assert.That(parsedData.TestResults, Has.Count.EqualTo(1));
        using (Assert.EnterMultipleScope())
        {
            var testResult = parsedData.TestResults.First();
            Assert.That(testResult.ClassName, Is.EqualTo("SampleNamespace.TestClass"));
            Assert.That(testResult.Duration, Is.EqualTo(0.093d));
            Assert.That(testResult.FailureMessage, Is.Null);
            Assert.That(testResult.StackTrace, Is.Null);
            Assert.That(testResult.Status, Is.EqualTo("Passed"));
            Assert.That(testResult.TestRunId, Is.EqualTo(0));
        }
    }

    [Test]
    public async Task Parse_ReturnsFailedTestResults()
    {
        var parser = new NunitTrxTestResultParser();

        var rawData = await File.ReadAllTextAsync("Assets/Nunit/nunit_test_results_one_failed.trx");
        var parsedData = parser.Parse(rawData);

        Assert.That(parsedData.TestResults, Has.Count.EqualTo(1));
        using (Assert.EnterMultipleScope())
        {
            var testResult = parsedData.TestResults.First();
            Assert.That(testResult.ClassName, Is.EqualTo("SampleNamespace.TestClass"));
            Assert.That(testResult.Duration, Is.EqualTo(0.025d));
            Assert.That(testResult.FailureMessage,
                Is.EqualTo("System.NullReferenceException : Object reference not set to an instance of an object."));
            Assert.That(testResult.StackTrace,
                Is.EqualTo("   at SampleNamespace.TestClass.Test_That_Fails() in /path/to/test/ExampleTests.cs:line 35"));
            Assert.That(testResult.Status, Is.EqualTo("Failed"));
            Assert.That(testResult.TestRunId, Is.EqualTo(0));
        }
    }
}