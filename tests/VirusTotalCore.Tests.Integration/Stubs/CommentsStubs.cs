using VirusTotalCore.Tests.Integration.ResponseBodies;

namespace VirusTotalCore.Tests.Integration.Stubs;

/// <summary>
/// Extension Object pattern: adds Comments-specific stub methods to <see cref="WireMockServer"/>.
/// </summary>
internal static class CommentsStubs
{
    private const string CommentsPath = "/comments";

    public static void StubGetLatestCommentsSuccess(this WireMockServer server) =>
        server
            .Given(Request.Create().WithPath(CommentsPath).UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(CommentsResponseBodies.GetLatestComments())
                .WithHeader("Content-Type", "application/json"));

    public static void StubGetCommentSuccess(this WireMockServer server, string commentId) =>
        server
            .Given(Request.Create().WithPath($"{CommentsPath}/{commentId}").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(CommentsResponseBodies.GetComment(commentId))
                .WithHeader("Content-Type", "application/json"));

    public static void StubDeleteSuccess(this WireMockServer server, string commentId) =>
        server
            .Given(Request.Create().WithPath($"{CommentsPath}/{commentId}").UsingDelete())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody("{}")
                .WithHeader("Content-Type", "application/json"));

    public static void StubAddCommentVoteSuccess(this WireMockServer server, string commentId) =>
        server
            .Given(Request.Create().WithPath($"{CommentsPath}/{commentId}/vote").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody("{}")
                .WithHeader("Content-Type", "application/json"));
}
