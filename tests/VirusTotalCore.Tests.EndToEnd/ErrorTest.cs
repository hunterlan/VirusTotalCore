using Microsoft.Extensions.Configuration;
using VirusTotalCore.IpAddresses.Endpoints;

namespace VirusTotalCore.Tests.EndToEnd;

public class ErrorTest
{
    private string ApiKey { get; }
    private readonly AddressIpEndpoint _endpoint;

    public ErrorTest()
    {
        var settings = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        ApiKey = settings["apiKey"]!;
        _endpoint = new AddressIpEndpoint(ApiKey);
    }
}
