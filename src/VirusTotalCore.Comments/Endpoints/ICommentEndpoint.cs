using VirusTotalCore.Common.Models.Comments;
using VirusTotalCore.Common.Models.Comments.Vote;

namespace VirusTotalCore.Comments.Endpoints;

/// <summary>
/// Defines operations for querying VirusTotal comment resources.
/// </summary>
public interface ICommentEndpoint
{
    /// <summary>
    /// Retrieves information about the latest comments added to VirusTotal.
    /// </summary>
    /// <param name="filter">Optional tag filter (e.g. filter=tag:malware).</param>
    /// <param name="cursor">Continuation cursor.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="limit">Number of items to retrieve. Default value is 10.</param>
    /// <returns>Latest comments.</returns>
    Task<CommentData> GetLatestComments(string? filter, string? cursor, CancellationToken cancellationToken = default, int limit = 10);

    /// <summary>
    /// Get specific comment by ID.
    /// </summary>
    /// <param name="commentId">Comment ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Specified comment.</returns>
    Task<Comment> Get(string commentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete comment by ID.
    /// </summary>
    /// <param name="commentId">Comment ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task Delete(string commentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add vote to a comment.
    /// </summary>
    /// <param name="commentId">Comment ID.</param>
    /// <param name="verdict">"Positive", "Negative" or "Abuse".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddVote(string commentId, CommentVerdict verdict, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get related objects for the comment.
    /// </summary>
    /// <param name="commentId">Comment ID.</param>
    /// <param name="relationship">Relationship type.</param>
    /// <param name="cursor">Continuation cursor.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="limit">Number of items to retrieve. Default value is 10.</param>
    Task<string> GetRelatedObjects(string commentId, string relationship, string? cursor, CancellationToken cancellationToken = default, int limit = 10);
}
