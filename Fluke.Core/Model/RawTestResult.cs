namespace Fluke.Core.Model;

public class RawTestResult(string rawResult, string format)
{
    public string RawResult { get; } = rawResult;
    public string Format { get; } = format;
}