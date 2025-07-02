using System.Net;
using System.Text;
using System.Text.Json;
using Fluke.Core.Model;
using Fluke.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PactNet;

namespace FlukeTests.IntegrationTests;

public class ProviderStateMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDictionary<string, Func<IServiceProvider, Task>> _providerStates;
    
    public ProviderStateMiddleware(RequestDelegate next)
    {
        _next = next;
        _providerStates = new Dictionary<string, Func<IServiceProvider, Task>>
        {
            { "a valid test result upload", UploadTestResultSuccesAsync }
        };
    }
    
    private async Task UploadTestResultSuccesAsync(IServiceProvider sp)
    {
        var service = sp.GetRequiredService<ITestResultService>();
        var rawData = await File.ReadAllTextAsync("Assets/Nunit/nunit_test_results_one_passed.trx");
        await service.ProcessTestResultAsync(new RawTestResult(rawData, "trx", "commit"));
    }
    
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/provider-states"))
        {
            await HandleProviderStatesRequest(context);
            await context.Response.WriteAsync(String.Empty);
        }
        else
        {
            await _next(context);
        }
    }

    private async Task HandleProviderStatesRequest(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.OK;

        if (context.Request.Method.ToUpper() == HttpMethod.Post.ToString().ToUpper() &&
            context.Request.Body != null)
        {
            string jsonRequestBody = String.Empty;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                jsonRequestBody = await reader.ReadToEndAsync();
            }

            var providerState = JsonSerializer.Deserialize<ProviderState>(jsonRequestBody);

            //A null or empty provider state key must be handled
            if (providerState != null && !String.IsNullOrEmpty(providerState.State))
            {
                var contextRequestServices = context.RequestServices;
                await _providerStates[providerState.State].Invoke(contextRequestServices);
            }
        }
    }
}