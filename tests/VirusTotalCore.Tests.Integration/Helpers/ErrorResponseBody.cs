namespace VirusTotalCore.Tests.Integration.Helpers;

/// <summary>
/// Object Mother providing canonical VirusTotal error JSON bodies for WireMock stubs.
/// </summary>
internal static class ErrorResponseBody
{
    public const string Forbidden =
        """{"error":{"code":"ForbiddenError","message":"You are not allowed to perform this action."}}""";

    public const string Unauthorized =
        """{"error":{"code":"WrongCredentialsError","message":"Wrong API key."}}""";

    public const string NotFound =
        """{"error":{"code":"NotFoundError","message":"Resource not found."}}""";

    public const string QuotaExceeded =
        """{"error":{"code":"QuotaExceededError","message":"You have exceeded your API quota."}}""";
}
