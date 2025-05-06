using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlukeCollectorAPI.Model;

public class TestResult
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int TestRunId { get; set; }
    public string TestName { get; set; }
    public string ClassName { get; set; }
    public string? FailureMessage { get; set; }
    public string? StackTrace { get; set; }
    public string Status { get; set; }
    public double Duration { get; set; }
    // public DateTime ExecutionTimeStamp { get; set; }
}