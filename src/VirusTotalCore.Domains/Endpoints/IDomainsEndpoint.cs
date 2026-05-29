using VirusTotalCore.Common.Enums;
using VirusTotalCore.Common.Models.Analysis;
using VirusTotalCore.Common.Models.Comments;
using VirusTotalCore.Common.Models.Votes;
using VirusTotalCore.Domains.Models;

namespace VirusTotalCore.Domains.Endpoints;

/// <summary>
/// Defines operations for querying VirusTotal domain resources.
/// </summary>
public interface IDomainsEndpoint
{
    /// <summary>
    /// Get report on the given domain.
    /// </summary>
    Task<AnalysisReport<DomainReportAttributes>> GetReport(string domain, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get comments for the given domain.
    /// </summary>
    Task<CommentData> GetComments(string domain, string? cursor, CancellationToken cancellationToken = default, int limit = 10);

    /// <summary>
    /// Add a comment to the given domain.
    /// </summary>
    Task<Comment> AddComment(string domain, string comment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get DNS resolution data for the given domain.
    /// </summary>
    void GetDnsResolution();

    /// <summary>
    /// Get community votes for the given domain.
    /// </summary>
    Task<VoteData> GetVotes(string domain, CancellationToken cancellationToken = default);

    /// <summary>
    /// Post a vote to the given domain.
    /// </summary>
    Task AddVote(string domain, VerdictType verdict, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get objects related to the given domain.
    /// </summary>
    Task<string> GetRelatedObjects(string domain, string relationship, string? cursor, CancellationToken cancellationToken = default, int limit = 10);

    /// <summary>
    /// Get related object descriptors for the given domain.
    /// </summary>
    Task<string> GetRelatedDescriptors(string domain, string relationship, string? cursor, CancellationToken cancellationToken = default, int limit = 10);
}
