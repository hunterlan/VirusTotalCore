using VirusTotalCore.Common.Enums;
using VirusTotalCore.Common.Models.Analysis;
using VirusTotalCore.Common.Models.Comments;
using VirusTotalCore.Common.Models.Votes;
using VirusTotalCore.IpAddresses.Models;

namespace VirusTotalCore.IpAddresses.Endpoints;

/// <summary>
/// Defines operations for querying VirusTotal IP address resources.
/// </summary>
public interface IAddressIpEndpoint
{
    /// <summary>
    /// Get report on given IP address.
    /// </summary>
    Task<AnalysisReport<AddressReportAttributes>> GetReport(string ipAddress, CancellationToken? cancellationToken);

    /// <summary>
    /// Get comments for given IP address.
    /// </summary>
    Task<CommentData> GetComments(string ipAddress, string? cursor, CancellationToken? cancellationToken, int limit = 10);

    /// <summary>
    /// Post a comment to an IP address.
    /// </summary>
    Task AddComment(string ipAddress, string comment, CancellationToken? cancellationToken);

    /// <summary>
    /// Get community votes for given IP address.
    /// </summary>
    Task<VoteData> GetVotes(string ipAddress, CancellationToken? cancellationToken);

    /// <summary>
    /// Post a vote to an IP address.
    /// </summary>
    Task AddVote(string ipAddress, VerdictType verdict, CancellationToken? cancellationToken);

    /// <summary>
    /// Get related objects for the IP address.
    /// </summary>
    Task<string> GetRelatedObjects(string ipAddress, string relationship, string? cursor, CancellationToken? cancellationToken, int limit = 10);

    /// <summary>
    /// Get related object descriptors for the IP address.
    /// </summary>
    Task<string> GetRelatedDescriptors(string ipAddress, string relationship, string? cursor, CancellationToken? cancellationToken, int limit = 10);
}
