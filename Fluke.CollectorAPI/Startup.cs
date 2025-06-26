using Fluke.Core.Parsers;
using Fluke.Core.Service;
using FlukeCollectorAPI;
using Microsoft.EntityFrameworkCore;

namespace Fluke.CollectorAPI;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<ITestResultService, TestResultService>();
        services.AddScoped<ITestResultRepository, TestResultRepository>();
        services.AddTransient<NunitXmlTestResultParser>();
        services.AddTransient<NunitTrxTestResultParser>();
        services.AddScoped<IParserResolver, ParserResolver>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(
                Configuration.GetConnectionString("DefaultConnection"))
        );


        services.AddControllers(options =>
            options.InputFormatters.Insert(0, new PlainTextFormatter())
        );
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        // app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}