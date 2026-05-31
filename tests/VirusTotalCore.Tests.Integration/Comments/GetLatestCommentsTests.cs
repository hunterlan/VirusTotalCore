using VirusTotalCore.Comments.Endpoints;
using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.Comments;

public sealed class GetLatestCommentsTests : WireMockEndpointTestBase
{
    private readonly CommentEndpoint _endpoint;

    public GetLatestCommentsTests()
    {
        _endpoint = new CommentEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task GetLatestComments_HappyPath_ReturnsDeserializedCommentData()
    {
        Server.StubGetLatestCommentsSuccess();

        var result = await _endpoint.GetLatestComments(null, null);

        Assert.NotNull(result);
        Assert.NotNull(result.Comments);
    }

    [Fact]
    public async Task GetLatestComments_WithoutCursor_QueryStringHasLimitOnly()
    {
        Server.StubGetLatestCommentsSuccess();

        await _endpoint.GetLatestComments(null, null);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.DoesNotContain("cursor", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetLatestComments_WithCursor_QueryStringContainsCursor()
    {
        const string cursor = "next-page-cursor";
        Server.StubGetLatestCommentsSuccess();

        await _endpoint.GetLatestComments(null, cursor);

        var entry = Server.LogEntries.Last();
        Assert.Contains($"cursor={cursor}", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetLatestComments_WhenForbidden_ThrowsForbiddenException()
    {
        Server.StubForbidden("/comments");

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            _endpoint.GetLatestComments(null, null));
    }

    [Fact]
    public async Task GetLatestComments_WhenUnauthorized_ThrowsWrongCredentialsException()
    {
        Server.StubUnauthorized("/comments");

        await Assert.ThrowsAsync<WrongCredentialsException>(() =>
            _endpoint.GetLatestComments(null, null));
    }
}
