namespace VirusTotalCore.Tests.Integration.Fixtures;

public abstract class WireMockFixture : IDisposable
{
    protected const string FAKE_API_KEY = "test-api-key";

    public WireMockServer Server { get; } = WireMockServer.Start();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Server.Stop();
        }
    }
}
