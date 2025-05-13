using Fluke.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Fluke.Core.Service;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TestResult> TestResults { get; set; }
    public DbSet<TestRun> TestRuns { get; set; }

}