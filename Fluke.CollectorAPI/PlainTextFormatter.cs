using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace FlukeCollectorAPI;

public class PlainTextFormatter : TextInputFormatter
{
    public PlainTextFormatter()
    {
        SupportedMediaTypes.Add(new MediaTypeHeaderValue(MediaTypeNames.Text.Plain));

        SupportedEncodings.Add(Encoding.UTF8);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context,
        Encoding encoding)
    {
        using var reader = new StreamReader(context.HttpContext.Request.Body, encoding);
        var plainText = await reader.ReadToEndAsync();

        return await InputFormatterResult.SuccessAsync(plainText);
    }
}