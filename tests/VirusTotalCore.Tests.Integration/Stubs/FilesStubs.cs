using VirusTotalCore.Tests.Integration.ResponseBodies;

namespace VirusTotalCore.Tests.Integration.Stubs;

/// <summary>
/// Extension Object pattern: adds Files-specific stub methods to <see cref="WireMockServer"/>.
/// </summary>
internal static class FilesStubs
{
    private const string FilesPath = "/files";

    public static void StubPostFileSuccess(this WireMockServer server) =>
        server
            .Given(Request.Create().WithPath(FilesPath).UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody("{}")
                .WithHeader("Content-Type", "application/json"));

    public static void StubGetReportSuccess(this WireMockServer server, string hash) =>
        server
            .Given(Request.Create().WithPath($"{FilesPath}/{hash}").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(FilesResponseBodies.GetReport(hash))
                .WithHeader("Content-Type", "application/json"));

    public static void StubGetRelatedSuccess(this WireMockServer server, string hash, string relationship) =>
        server
            .Given(Request.Create().WithPath($"{FilesPath}/{hash}/{relationship}").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(FilesResponseBodies.GetRelated())
                .WithHeader("Content-Type", "application/json"));

    public static void StubGetDescriptorsSuccess(this WireMockServer server, string hash, string relationship) =>
        server
            .Given(Request.Create().WithPath($"{FilesPath}/{hash}/relationships/{relationship}").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(FilesResponseBodies.GetRelated())
                .WithHeader("Content-Type", "application/json"));
}
