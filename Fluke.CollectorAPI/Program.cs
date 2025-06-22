using Fluke.Core.Parsers;
using Fluke.Core.Service;
using FlukeCollectorAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITestResultService, TestResultService>();
builder.Services.AddScoped<ITestResultRepository, TestResultRepository>();
builder.Services.AddTransient<NunitXmlTestResultParser>();
builder.Services.AddTransient<NunitTrxTestResultParser>();
builder.Services.AddScoped<IParserResolver, ParserResolver>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddControllers(options =>
    options.InputFormatters.Insert(0, new PlainTextFormatter())
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseAuthorization();
app.MapControllers();

app.Run();