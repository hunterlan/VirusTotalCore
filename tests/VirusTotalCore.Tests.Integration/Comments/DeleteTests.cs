using VirusTotalCore.Comments.Endpoints;
using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.Comments;

public sealed class DeleteTests : WireMockEndpointTestBase
{
    private const string CommentId = "f1a2b3c4d5e6";

    private readonly CommentEndpoint _endpoint;

    public DeleteTests()
    {
        _endpoint = new CommentEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task Delete_HappyPath_SendsDeleteRequest()
    {
        Server.StubDeleteSuccess(CommentId);

        await _endpoint.Delete(CommentId);

        var entry = Server.LogEntries.Last();
        Assert.Equal("DELETE", entry.RequestMessage!.Method);
        Assert.Contains($"/comments/{CommentId}", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task Delete_WhenForbidden_ThrowsForbiddenException()
    {
        Server.StubForbidden($"/comments/{CommentId}", "DELETE");

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            _endpoint.Delete(CommentId));
    }

    [Fact]
    public async Task Delete_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/comments/{CommentId}", "DELETE");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.Delete(CommentId));
    }
}
