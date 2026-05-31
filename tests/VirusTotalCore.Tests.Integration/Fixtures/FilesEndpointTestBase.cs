using VirusTotalCore.Files.Endpoints;

namespace VirusTotalCore.Tests.Integration.Fixtures;

public abstract class FilesEndpointTestBase : IClassFixture<FilesEndpointFixture>
{
    protected const string TEST_HASH = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08";

    protected WireMockServer Server { get; }
    protected FilesEndpoint Endpoint { get; }

    protected FilesEndpointTestBase(FilesEndpointFixture fixture)
    {
        Server = fixture.Server;
        Endpoint = fixture.Endpoint;
        Server.Reset();
    }
}

