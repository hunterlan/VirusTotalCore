using System.Net;
using VirusTotalCore.Tests.Integration.Helpers;

namespace VirusTotalCore.Tests.Integration.Stubs;

/// <summary>
/// Extension Object pattern: adds reusable error-stub methods to <see cref="WireMockServer"/>
/// for HTTP status codes shared by all endpoint test classes.
/// </summary>
internal static class WireMockServerExtensions
{
    public static void StubForbidden(this WireMockServer server, string path, string httpMethod = "GET") =>
        server
            .Given(Request.Create().WithPath(path).UsingMethod(httpMethod))
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.Forbidden)
                .WithBody(ErrorResponseBody.Forbidden)
                .WithHeader("Content-Type", "application/json"));

    public static void StubUnauthorized(this WireMockServer server, string path, string httpMethod = "GET") =>
        server
            .Given(Request.Create().WithPath(path).UsingMethod(httpMethod))
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.Unauthorized)
                .WithBody(ErrorResponseBody.Unauthorized)
                .WithHeader("Content-Type", "application/json"));

    public static void StubNotFound(this WireMockServer server, string path, string httpMethod = "GET") =>
        server
            .Given(Request.Create().WithPath(path).UsingMethod(httpMethod))
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.NotFound)
                .WithBody(ErrorResponseBody.NotFound)
                .WithHeader("Content-Type", "application/json"));

    public static void StubQuotaExceeded(this WireMockServer server, string path, string httpMethod = "GET") =>
        server
            .Given(Request.Create().WithPath(path).UsingMethod(httpMethod))
            .RespondWith(Response.Create()
                .WithStatusCode(429)
                .WithBody(ErrorResponseBody.QuotaExceeded)
                .WithHeader("Content-Type", "application/json"));
}
