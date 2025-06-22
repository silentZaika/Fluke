// See https://aka.ms/new-console-template for more information
using System.CommandLine;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

var fileOption = new Option<FileInfo>("--file", 
    "The file with the test execution results")
    { IsRequired = true };

var formatOption = new Option<string>("--format", 
    "The test execution results format. Supported formats: xml, trx")
    { IsRequired = true };

var commitOption = new Option<string>("--commit", 
    "The commit hash on which the tests were run")
    { IsRequired = true };

var endpointOption = new Option<string>("--endpoint",
    "The endpoint to which upload the results to")
    { IsRequired = true };

var rootCmd = new RootCommand("Fluke client SDK to upload test execution results");
rootCmd.AddOption(fileOption);
rootCmd.AddOption(formatOption);
rootCmd.AddOption(commitOption);
rootCmd.AddOption(endpointOption);

rootCmd.SetHandler(ReadFile, fileOption, formatOption, commitOption, endpointOption);

return await rootCmd.InvokeAsync(args);

static async Task ReadFile(FileInfo file, string format, string commit, string endpoint)
{
    var rawTestData = await File.ReadAllTextAsync(file.FullName);
    Console.WriteLine(rawTestData);
    Console.WriteLine(format);
    Console.WriteLine(commit);
    Console.WriteLine(endpoint);

    var payload = new { commit, format, rawTestData };
    var json = JsonSerializer.Serialize(payload);

    var httpContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
    var client = new HttpClient();
    var url = $"{endpoint}/api/TestResult/upload";
    Console.WriteLine(url);
    var response = await client.PostAsync(url, httpContent);

    if (response.IsSuccessStatusCode)
        Console.WriteLine("Test results uploaded successfully");
    else
    {
        Console.Error.WriteLine(
            $"Test upload failed with: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
    }
}
