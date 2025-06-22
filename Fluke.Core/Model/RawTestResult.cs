namespace Fluke.Core.Model;

public class RawTestResult(string rawTestData, string format, string commit)
{
    public string RawTestData { get; } = rawTestData;
    public string Format { get; } = format;

    public string Commit { get; } = commit;
}