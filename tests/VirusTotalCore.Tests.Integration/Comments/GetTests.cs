using VirusTotalCore.Comments.Endpoints;
using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.Comments;

public sealed class GetTests : WireMockEndpointTestBase
{
    private const string CommentId = "f1a2b3c4d5e6";

    private readonly CommentEndpoint _endpoint;

    public GetTests()
    {
        _endpoint = new CommentEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task Get_HappyPath_ReturnsDeserializedComment()
    {
        Server.StubGetCommentSuccess(CommentId);

        var comment = await _endpoint.Get(CommentId);

        Assert.NotNull(comment);
        Assert.Equal(CommentId, comment.Id);
        Assert.Equal("test comment", comment.Attributes.Text);
    }

    [Fact]
    public async Task Get_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/comments/{CommentId}");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.Get(CommentId));
    }
}
