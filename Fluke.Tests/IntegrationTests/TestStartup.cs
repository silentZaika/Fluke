using Fluke.CollectorAPI.Controllers;
using Fluke.Core.Parsers;
using Fluke.Core.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlukeTests.IntegrationTests;

public class TestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<ITestResultService, TestResultService>();
        services.AddScoped<ITestResultRepository, TestResultRepository>();
        services.AddTransient<NunitXmlTestResultParser>();
        services.AddTransient<NunitTrxTestResultParser>();
        services.AddScoped<IParserResolver, ParserResolver>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));
        
        services.AddControllers()
            .AddApplicationPart(typeof(TestResultController).Assembly);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ProviderStateMiddleware>();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
             endpoints.MapControllers();
        });
    }
}