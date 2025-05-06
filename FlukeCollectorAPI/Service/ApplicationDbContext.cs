using FlukeCollectorAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace FlukeCollectorAPI.Service;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Model.TestResult> TestResults { get; set; }
    public DbSet<TestRun> TestRuns { get; set; }

}