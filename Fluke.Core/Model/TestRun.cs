using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fluke.Core.Model;

public class TestRun
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string TestRunName { get; set; }
    public int Passed { get; set; }
    public int Failed { get; set; }
    public int Total { get; set; }
    public DateTime StarTime { get; set; }
    public double Duration { get; set; }
    
    public string CommitHash { get; set; }
    public List<TestResult> TestResults { get; set; }
}