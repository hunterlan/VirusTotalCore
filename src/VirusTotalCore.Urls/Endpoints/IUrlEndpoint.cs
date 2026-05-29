using VirusTotalCore.Common.Enums;
using VirusTotalCore.Common.Models.Analysis;
using VirusTotalCore.Common.Models.Comments;
using VirusTotalCore.Common.Models.Votes;
using VirusTotalCore.Urls.Models;

namespace VirusTotalCore.Urls.Endpoints;

/// <summary>
/// Defines operations for querying VirusTotal URL resources.
/// </summary>
public interface IUrlEndpoint
{
    /// <summary>
    /// Request to scan a URL and submit it for analysis.
    /// </summary>
    /// <param name="url">URL to scan</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<string> Scan(string url, CancellationToken? cancellationToken);
    /// <summary>
    /// Scan a URL and get the analysis report.
    /// </summary>
    Task<AnalysisReport<UrlReportAttributes>> GetReport(string url, CancellationToken? cancellationToken);

    /// <summary>
    /// Get comments for the given URL identifier.
    /// </summary>
    Task<CommentData> GetComments(string id, string? cursor, CancellationToken? cancellationToken, int limit = 10);

    /// <summary>
    /// Add a comment to the given URL identifier.
    /// </summary>
    Task<Comment> AddComment(string id, string comment, CancellationToken? cancellationToken);

    /// <summary>
    /// Get community votes for the given URL identifier.
    /// </summary>
    Task<VoteData> GetVotes(string id, CancellationToken? cancellationToken);

    /// <summary>
    /// Post a vote to the given URL identifier.
    /// </summary>
    Task AddVote(string id, VerdictType verdict, CancellationToken? cancellationToken);

    /// <summary>
    /// Get objects related to the given URL identifier.
    /// </summary>
    Task<string> GetRelatedObjects(string id, string relationship, string? cursor, CancellationToken? cancellationToken, int limit = 10);

    /// <summary>
    /// Get related object descriptors for the given URL identifier.
    /// </summary>
    Task<string> GetRelatedDescriptors(string id, string relationship, string? cursor, CancellationToken? cancellationToken, int limit = 10);
}
