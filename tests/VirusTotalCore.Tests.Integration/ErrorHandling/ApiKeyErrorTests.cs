using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Files.Endpoints;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.ErrorHandling;

/// <summary>
/// Verifies that account-level HTTP error codes (wrong/missing API key, quota exhausted)
/// are correctly mapped to exceptions by the shared <c>VirusTotalErrorHandler</c>.
/// These errors are independent of the specific resource or endpoint method; any representative
/// call is sufficient to exercise the shared transport error path.
/// </summary>
public sealed class ApiKeyErrorTests : WireMockEndpointTestBase
{
    private const string FileHash = "275a021bbfb6489e54d471899f7db9d1663fc695ec2fe2a2c4538aabf651fd0f";

    private readonly FilesEndpoint _endpoint;

    public ApiKeyErrorTests()
    {
        _endpoint = new FilesEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task WhenUnauthorized_ThrowsWrongCredentialsException()
    {
        Server.StubUnauthorized($"/files/{FileHash}");

        await Assert.ThrowsAsync<WrongCredentialsException>(() =>
            _endpoint.GetReport(FileHash));
    }

    [Fact]
    public async Task WhenQuotaExceeded_ThrowsQuotaExceededException()
    {
        Server.StubQuotaExceeded($"/files/{FileHash}");

        await Assert.ThrowsAsync<QuotaExceededException>(() =>
            _endpoint.GetReport(FileHash));
    }
}
