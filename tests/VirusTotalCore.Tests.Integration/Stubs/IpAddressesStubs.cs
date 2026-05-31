using VirusTotalCore.Tests.Integration.ResponseBodies;

namespace VirusTotalCore.Tests.Integration.Stubs;

/// <summary>
/// Extension Object pattern: adds IP Addresses-specific stub methods to <see cref="WireMockServer"/>.
/// </summary>
internal static class IpAddressesStubs
{
    private const string IpAddressesPath = "/ip_addresses";

    public static void StubGetIpReportSuccess(this WireMockServer server, string ipAddress) =>
        server
            .Given(Request.Create().WithPath($"{IpAddressesPath}/{ipAddress}").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(IpAddressesResponseBodies.GetReport(ipAddress))
                .WithHeader("Content-Type", "application/json"));

    public static void StubGetIpRelatedObjectsSuccess(this WireMockServer server, string ipAddress, string relationship) =>
        server
            .Given(Request.Create().WithPath($"{IpAddressesPath}/{ipAddress}/{relationship}").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(IpAddressesResponseBodies.GetRelated())
                .WithHeader("Content-Type", "application/json"));

    public static void StubGetIpRelatedDescriptorsSuccess(this WireMockServer server, string ipAddress, string relationship) =>
        server
            .Given(Request.Create().WithPath($"{IpAddressesPath}/{ipAddress}/relationships/{relationship}").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(IpAddressesResponseBodies.GetRelated())
                .WithHeader("Content-Type", "application/json"));

    public static void StubAddIpVoteSuccess(this WireMockServer server, string ipAddress) =>
        server
            .Given(Request.Create().WithPath($"{IpAddressesPath}/{ipAddress}/votes").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody("{}")
                .WithHeader("Content-Type", "application/json"));
}
