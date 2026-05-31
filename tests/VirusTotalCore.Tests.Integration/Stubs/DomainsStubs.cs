using VirusTotalCore.Tests.Integration.ResponseBodies;

namespace VirusTotalCore.Tests.Integration.Stubs;

/// <summary>
/// Extension Object pattern: adds Domains-specific stub methods to <see cref="WireMockServer"/>.
/// </summary>
internal static class DomainsStubs
{
    private const string DomainsPath = "/domains";

    public static void StubGetDomainReportSuccess(this WireMockServer server, string domain) =>
        server
            .Given(Request.Create().WithPath($"{DomainsPath}/{domain}").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(DomainsResponseBodies.GetReport(domain))
                .WithHeader("Content-Type", "application/json"));

    public static void StubGetDomainRelatedObjectsSuccess(this WireMockServer server, string domain, string relationship) =>
        server
            .Given(Request.Create().WithPath($"{DomainsPath}/{domain}/{relationship}").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(DomainsResponseBodies.GetRelated())
                .WithHeader("Content-Type", "application/json"));

    public static void StubGetDomainRelatedDescriptorsSuccess(this WireMockServer server, string domain, string relationship) =>
        server
            .Given(Request.Create().WithPath($"{DomainsPath}/{domain}/relationships/{relationship}").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(DomainsResponseBodies.GetRelated())
                .WithHeader("Content-Type", "application/json"));
}
