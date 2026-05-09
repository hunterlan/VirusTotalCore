using VirusTotalCore.Common;
using VirusTotalCore.Common.Enums;
using VirusTotalCore.Common.Models.Analysis;
using VirusTotalCore.Common.Models.Comments;
using VirusTotalCore.Common.Models.Votes;
using VirusTotalCore.Domains.Models;

namespace VirusTotalCore.Domains.Endpoints;

/// <summary>
/// Analyse domains, get reports, comments and votes about them.
/// </summary>
public sealed class DomainsEndpoint : BaseEndpoint, IDomainsEndpoint
{
    public DomainsEndpoint(string apiKey) : base(apiKey, "domains") { }
    public DomainsEndpoint(IHttpClientFactory customHttpClient, string apiKey) : base(customHttpClient, apiKey, "domains") { }

    /// <summary>
    /// Get report about a specific domain.
    /// </summary>
    /// <param name="domain">Domain name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns cref="DomainReportAttributes">Analysis report</returns>
    public async Task<AnalysisReport<DomainReportAttributes>> GetReport(string domain, CancellationToken? cancellationToken)
    {
        const string rootPropertyName = "data";
        return await GetAsync<AnalysisReport<DomainReportAttributes>>(domain, rootPropertyName, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Get comments about a domain.
    /// </summary>
    /// <param name="domain">Domain name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="cursor">Continuation cursor</param>
    /// <param name="limit">Number of items to retrieve. Default value is 10.</param>
    /// <returns>List of comments with metadata</returns>
    /// <exception cref="Exception"></exception>
    public async Task<CommentData> GetComments(string domain, CancellationToken? cancellationToken, string? cursor,
        int limit = 10)
    {
        var requestUrl = $"{domain}/comments?limit={limit}";
        if (cursor is not null)
        {
            requestUrl += $"&cursor={cursor}";
        }

        return await GetAsync<CommentData>(requestUrl, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Add a new comment about a domain.
    /// Any word starting with # in your comment's text will be considered a tag,
    /// and added to the comment's tag attribute.
    /// </summary>
    /// <param name="domain">Domain name</param>
    /// <param name="comment">Comment content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Comment data</returns>
    /// <exception cref="Exception"></exception>
    public async Task<Comment> AddComment(string domain, string comment, CancellationToken? cancellationToken)
    {
        const string rootPropertyName = "data";
        var newComment = new AddComment(comment);
        var requestUrl = $"{domain}/comments";

        return await PostAsync<Comment>(requestUrl, rootPropertyName, newComment, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Get DNS resolution data for the given domain.
    /// </summary>
    public void GetDnsResolution()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get votes data on a domain.
    /// </summary>
    /// <param name="domain">Domain name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Votes with metadata</returns>
    /// <exception cref="Exception"></exception>
    public async Task<VoteData> GetVotes(string domain, CancellationToken? cancellationToken)
    {
        var requestUrl = $"{domain}/votes";
        return await GetAsync<VoteData>(requestUrl, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    /// Add a user vote to a specific domain.
    /// </summary>
    /// <param name="domain">Domain name</param>
    /// <param name="verdict">"Harmless" or "Malicious"</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="Exception"></exception>
    public async Task AddVote(string domain, VerdictType verdict, CancellationToken? cancellationToken)
    {
        var newVote = new AddVote(verdict);
        var requestUrl = $"{domain}/votes";

        await PostAsync(requestUrl, newVote, cancellationToken ?? new CancellationToken());
    }

    public override async Task<string> GetRelatedObjects(string domain, string relationship, string? cursor,
        CancellationToken? cancellationToken, int limit = 10)
    {
        return await base.GetRelatedObjects(domain, relationship, cursor, cancellationToken, limit);
    }

    public new async Task<string> GetRelatedDescriptors(string domain, string relationship, string? cursor,
        CancellationToken? cancellationToken, int limit = 10)
    {
        return await base.GetRelatedDescriptors(domain, relationship, cursor, cancellationToken, limit);
    }
}
