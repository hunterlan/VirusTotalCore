using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Files.Endpoints;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.Files;

public sealed class PostFileTests : WireMockEndpointTestBase
{
    private readonly FilesEndpoint _endpoint;

    public PostFileTests()
    {
        _endpoint = new FilesEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task PostFile_HappyPath_ReturnsNonEmptyHash()
    {
        Server.StubPostFileSuccess();
        using var stream = new MemoryStream([1, 2, 3]);

        var hash = await _endpoint.PostFile(stream, "test.bin", null);

        Assert.False(string.IsNullOrEmpty(hash));
    }

    [Fact]
    public async Task PostFile_SentAsMultipartFormData_WithFileFieldName()
    {
        Server.StubPostFileSuccess();
        using var stream = new MemoryStream([1, 2, 3]);

        await _endpoint.PostFile(stream, "test.bin", null);

        var entry = Server.LogEntries.Last();
        var contentType = entry.RequestMessage!.Headers!["Content-Type"].First();
        Assert.Contains("multipart/form-data", contentType);
        Assert.Contains("name=file", entry.RequestMessage!.Body!);
    }

    [Fact]
    public async Task PostFile_WhenForbidden_ThrowsForbiddenException()
    {
        Server.StubForbidden("/files", "POST");
        using var stream = new MemoryStream([1, 2, 3]);

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            _endpoint.PostFile(stream, "test.bin", null));
    }
}
