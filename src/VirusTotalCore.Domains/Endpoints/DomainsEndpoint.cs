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
    internal DomainsEndpoint(HttpClient httpClient) : base(httpClient, "domains") { }

    /// <summary>
    /// Get report about a specific domain.
    /// </summary>
    /// <param name="domain">Domain name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns cref="DomainReportAttributes">Analysis report</returns>
    public async Task<AnalysisReport<DomainReportAttributes>> GetReport(string domain, CancellationToken cancellationToken = default)
    {
        const string rootPropertyName = "data";
        return await GetAsync<AnalysisReport<DomainReportAttributes>>(domain, rootPropertyName, cancellationToken);
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
    public async Task<CommentData> GetComments(string domain, string? cursor, CancellationToken cancellationToken = default,
        int limit = 10)
    {
        var requestUrl = $"{domain}/comments?limit={limit}";
        if (cursor is not null)
        {
            requestUrl += $"&cursor={cursor}";
        }

        return await GetAsync<CommentData>(requestUrl, cancellationToken);
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
    public async Task<Comment> AddComment(string domain, string comment, CancellationToken cancellationToken = default)
    {
        const string rootPropertyName = "data";
        var newComment = new AddComment(comment);
        var requestUrl = $"{domain}/comments";

        return await PostAsync<Comment, AddComment>(requestUrl, rootPropertyName, newComment, cancellationToken);
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
    public async Task<VoteData> GetVotes(string domain, CancellationToken cancellationToken = default)
    {
        var requestUrl = $"{domain}/votes";
        return await GetAsync<VoteData>(requestUrl, cancellationToken);
    }

    /// <summary>
    /// Add a user vote to a specific domain.
    /// </summary>
    /// <param name="domain">Domain name</param>
    /// <param name="verdict">"Harmless" or "Malicious"</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="Exception"></exception>
    public async Task AddVote(string domain, VerdictType verdict, CancellationToken cancellationToken = default)
    {
        var newVote = new AddVote(verdict);
        var requestUrl = $"{domain}/votes";

        await PostAsync(requestUrl, newVote, cancellationToken);
    }

}
