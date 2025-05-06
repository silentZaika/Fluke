using System.Net.Mime;
using System.Text;
using FlukeCollectorAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FlukeTests;

public class PlainTextFormatterTests
{
    [Test]
    public void PlainTextFormatter_SupportsPlainTextMediaAndUTF8()
    {
        var formatter = new PlainTextFormatter();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(formatter.SupportedEncodings, Contains.Item(Encoding.UTF8));
            Assert.That(formatter.SupportedMediaTypes, Contains.Item(MediaTypeNames.Text.Plain));
        }
    }

    [Test]
    public async Task ReadRequestBodyAsync_ReturnsModel()
    {
        var formatter = new PlainTextFormatter();
        
        var httpContext = new DefaultHttpContext
        {
            Request =
            {
                Body = new MemoryStream(Encoding.UTF8.GetBytes("Hello World"))
            }
        };
        var model = new EmptyModelMetadataProvider().GetMetadataForType(typeof(string));
        var contextMock = new InputFormatterContext(
            httpContext, "testModel", 
            new ModelStateDictionary(), model, 
            readerFactory: (stream, enc) => new StreamReader(stream, enc));
            
        var result = await formatter.ReadRequestBodyAsync(contextMock, Encoding.UTF8);

        Assert.That(result.Model, Is.EqualTo("Hello World"));
        Assert.That(result.IsModelSet, Is.True);
        Assert.That(result.HasError, Is.False);
    }
}