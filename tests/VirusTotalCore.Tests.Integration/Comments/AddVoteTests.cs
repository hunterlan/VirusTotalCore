using System.Text.Json;
using VirusTotalCore.Comments.Endpoints;
using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Common.Models.Comments.Vote;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.Comments;

public sealed class AddVoteTests : WireMockEndpointTestBase
{
    private const string CommentId = "f1a2b3c4d5e6";

    private readonly CommentEndpoint _endpoint;

    public AddVoteTests()
    {
        _endpoint = new CommentEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task AddVote_PositiveVerdict_RequestBodyContainsVerdict()
    {
        Server.StubAddCommentVoteSuccess(CommentId);

        await _endpoint.AddVote(CommentId, CommentVerdict.Positive);

        var entry = Server.LogEntries.Last();
        var body = JsonDocument.Parse(entry.RequestMessage!.Body!);
        var data = body.RootElement.GetProperty("data").GetString();
        Assert.Equal("positive", data);
    }

    [Fact]
    public async Task AddVote_WhenForbidden_ThrowsForbiddenException()
    {
        Server.StubForbidden($"/comments/{CommentId}/vote", "POST");

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            _endpoint.AddVote(CommentId, CommentVerdict.Positive));
    }

    [Fact]
    public async Task AddVote_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/comments/{CommentId}/vote", "POST");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.AddVote(CommentId, CommentVerdict.Positive));
    }
}
