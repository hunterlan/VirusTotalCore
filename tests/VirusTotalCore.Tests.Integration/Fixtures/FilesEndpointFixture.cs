using VirusTotalCore.Files.Endpoints;

namespace VirusTotalCore.Tests.Integration.Fixtures;

public sealed class FilesEndpointFixture : WireMockFixture
{
    public FilesEndpoint Endpoint { get; }

    public FilesEndpointFixture()
    {
        Endpoint = new FilesEndpoint(new WireMockHttpClientFactory(Server.Url!), FAKE_API_KEY);
    }
}
