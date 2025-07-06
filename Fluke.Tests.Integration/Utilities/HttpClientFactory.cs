namespace Fluke.Tests.Integration.Utilities;

public class HttpClientFactory
{
    public static HttpClient Create()
    {
        return new HttpClient(new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
    }
}