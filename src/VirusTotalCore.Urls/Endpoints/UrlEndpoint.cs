using VirusTotalCore.Common;
using VirusTotalCore.Common.Enums;
using VirusTotalCore.Common.Models.Analysis;
using VirusTotalCore.Common.Models.Comments;
using VirusTotalCore.Common.Models.Votes;
using VirusTotalCore.Urls.Models;

namespace VirusTotalCore.Urls.Endpoints;

/// <summary>
/// Analyse URLs, get reports, comments and votes about it and owners.
/// </summary>
/// <param name="apiKey">User's API key.</param>
public sealed class UrlEndpoint : BaseEndpoint, IUrlEndpoint
{
    public UrlEndpoint(string apiKey) : base(apiKey, "urls") { }
    public UrlEndpoint(IHttpClientFactory customHttpClient, string apiKey) : base(customHttpClient, apiKey, "urls") { }

    /// <summary>
    /// Request to scan a URL and submit it for analysis.
    /// </summary>
    /// <param name="url">URL to scan</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task<string> Scan(string url, CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(url), "url");
        return await PostMultipartAsync(null, content, cancellationToken);
    }

    /// <summary>
    /// Get report about a URL. Submits for scanning if not already cached.
    /// </summary>
    /// <param name="url">URL to scan</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns cref="UrlReportAttributes">Report analysis</returns>
    /// <exception cref="Exception"></exception>
    public async Task<AnalysisReport<UrlReportAttributes>> GetReport(string url, CancellationToken cancellationToken = default)
    {
        const string rootPropertyName = "data";
        return await GetAsync<AnalysisReport<UrlReportAttributes>>(ToBase64String(url), rootPropertyName, cancellationToken);
    }

    /// <summary>
    /// Get comments about a URL.
    /// </summary>
    /// <param name="id">URL identifier</param>
    /// <param name="cursor">Continuation cursor</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="limit">Maximum number of comments to retrieve. By default is 10.</param>
    /// <returns>List of comments with metadata.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<CommentData> GetComments(string id, string? cursor, CancellationToken cancellationToken = default, int limit = 10)
    {
        var requestUrl = $"{id}/comments?limit={limit}";
        if (cursor is not null)
        {
            requestUrl += $"&cursor={cursor}";
        }

        return await GetAsync<CommentData>(requestUrl, cancellationToken);
    }

    /// <summary>
    /// Add user's comment to a URL.
    /// </summary>
    /// <param name="id">URL identifier</param>
    /// <param name="comment">Comment content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Comment data</returns>
    /// <exception cref="Exception"></exception>
    public async Task<Comment> AddComment(string id, string comment, CancellationToken cancellationToken = default)
    {
        const string rootPropertyName = "data";
        var newComment = new AddComment(comment);
        var requestUrl = $"{id}/comments";

        return await PostAsync<Comment>(requestUrl, rootPropertyName, newComment, cancellationToken);
    }

    /// <summary>
    /// Get votes on a URL.
    /// </summary>
    /// <param name="id">URL identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Votes with metadata</returns>
    /// <exception cref="Exception"></exception>
    public async Task<VoteData> GetVotes(string id, CancellationToken cancellationToken = default)
    {
        var requestUrl = $"{id}/votes";
        return await GetAsync<VoteData>(requestUrl, cancellationToken);
    }

    /// <summary>
    /// Add user's vote to a URL.
    /// </summary>
    /// <param name="id">URL identifier</param>
    /// <param name="verdict">"Harmless" or "Malicious"</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="Exception"></exception>
    public async Task AddVote(string id, VerdictType verdict, CancellationToken cancellationToken = default)
    {
        var newVote = new AddVote(verdict);
        var requestUrl = $"{id}/votes";

        await PostAsync(requestUrl, newVote, cancellationToken);
    }

    public override async Task<string> GetRelatedObjects(string id, string relationship, string? cursor,
        CancellationToken cancellationToken = default, int limit = 10)
    {
        return await base.GetRelatedObjects(id, relationship, cursor, cancellationToken, limit);
    }

    private static string ToBase64String(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}
