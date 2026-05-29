using VirusTotalCore.Common;
using VirusTotalCore.Common.Models.Comments;
using VirusTotalCore.Common.Models.Comments.Vote;

namespace VirusTotalCore.Comments.Endpoints;

/// <summary>
/// Get comments, delete own comments or add votes to comment.
/// </summary>
/// <param name="apiKey">User's API key</param>
public class CommentEndpoint : BaseEndpoint, ICommentEndpoint
{
    public CommentEndpoint(string apiKey) : base(apiKey, "comments") { }
    public CommentEndpoint(IHttpClientFactory customHttpClient, string apiKey) : base(customHttpClient, apiKey, "comments") { }

    /// <summary>
    /// This endpoint retrieves information about the latest comments added to VirusTotal.
    /// </summary>
    /// <param name="filter"> Optional. Do some filtering over those comments, and get only those that contains a certain tag inside
    /// (e.g. filter=tag:malware).</param>
    /// <param name="cursor">Continuation cursor</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="limit">Number of items to retrieve. Default value is 10.</param>
    /// <returns>Latest comments.</returns>
    public async Task<CommentData> GetLatest(string? filter, string? cursor, CancellationToken cancellationToken = default,
        int limit = 10)
    {
        var requestUrl = $"?limit={limit}";
        if (filter is not null)
        {
            requestUrl += $"&filter={filter}";
        }

        if (cursor is not null)
        {
            requestUrl += $"&cursor={cursor}";
        }

        return await GetAsync<CommentData>(requestUrl, cancellationToken);
    }

    /// <summary>
    /// Get specific comment by ID.
    /// </summary>
    /// <param name="commentId">Comment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Specified comment.</returns>
    public async Task<Comment> Get(string commentId, CancellationToken cancellationToken = default)
    {
        const string rootPropertyName = "data";
        var requestUrl = $"{commentId}";
        return await GetAsync<Comment>(requestUrl, rootPropertyName, cancellationToken);
    }

    /// <summary>
    /// Delete comment by ID.
    /// </summary>
    /// <param name="commentId">Comment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task Delete(string commentId, CancellationToken cancellationToken = default)
    {
        var requestUrl = $"{commentId}";
        await DeleteAsync(requestUrl, cancellationToken);
    }

    /// <summary>
    /// Add vote to comment
    /// </summary>
    /// <param name="commentId">Comment ID</param>
    /// <param name="verdict">"Positive", "Negative" or "Abuse"</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task AddVote(string commentId, CommentVerdict verdict, CancellationToken cancellationToken = default)
    {
        var newVote = new { Data = verdict.ToString().ToLower() };
        var requestUrl = $"{commentId}/vote";

        await PostAsync(requestUrl, newVote, cancellationToken);
    }

    public override async Task<string> GetRelatedObjects(string commentId, string relationship, string? cursor,
        CancellationToken cancellationToken = default, int limit = 10)
    {
        return await base.GetRelatedObjects(commentId, relationship, cursor, cancellationToken, limit);
    }
}
