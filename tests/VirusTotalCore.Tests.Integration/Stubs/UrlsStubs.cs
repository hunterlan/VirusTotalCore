using VirusTotalCore.Tests.Integration.ResponseBodies;

namespace VirusTotalCore.Tests.Integration.Stubs;

/// <summary>
/// Extension Object pattern: adds URLs-specific stub methods to <see cref="WireMockServer"/>.
/// </summary>
internal static class UrlsStubs
{
    private const string UrlsPath = "/urls";

    public static void StubGetUrlReportSuccess(this WireMockServer server, string urlId) =>
        server
            .Given(Request.Create().WithPath($"{UrlsPath}/{urlId}").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(UrlsResponseBodies.GetReport(urlId))
                .WithHeader("Content-Type", "application/json"));

    public static void StubScanSuccess(this WireMockServer server) =>
        server
            .Given(Request.Create().WithPath(UrlsPath).UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(UrlsResponseBodies.Scan())
                .WithHeader("Content-Type", "application/json"));

    public static void StubGetUrlRelatedObjectsSuccess(this WireMockServer server, string urlId, string relationship) =>
        server
            .Given(Request.Create().WithPath($"{UrlsPath}/{urlId}/{relationship}").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(UrlsResponseBodies.GetRelated())
                .WithHeader("Content-Type", "application/json"));
}
