namespace VirusTotalCore.Tests.Integration.Helpers;

internal static class WireMockErrorResponses
{
    public static IResponseBuilder Forbidden() => Response.Create()
        .WithStatusCode(403)
        .WithBody("""{"error": {"code": "ForbiddenError", "message": "Access forbidden."}}""");

    public static IResponseBuilder WrongCredentials() => Response.Create()
        .WithStatusCode(401)
        .WithBody("""{"error": {"code": "WrongCredentialsError", "message": "Wrong API key."}}""");

    public static IResponseBuilder QuotaExceeded() => Response.Create()
        .WithStatusCode(429)
        .WithBody("""{"error": {"code": "QuotaExceededError", "message": "API quota exceeded."}}""");

    public static IResponseBuilder NotFound() => Response.Create()
        .WithStatusCode(404)
        .WithBody("""{"error": {"code": "NotFoundError", "message": "File not found."}}""");
}
