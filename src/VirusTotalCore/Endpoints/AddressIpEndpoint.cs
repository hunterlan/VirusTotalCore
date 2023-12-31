using System.Text.Json;
using RestSharp;
using VirusTotalCore.Enums;
using VirusTotalCore.Exceptions;
using VirusTotalCore.Models.Analysis.IP;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Comments.IP;
using VirusTotalCore.Models.Shared;
using VirusTotalCore.Models.Votes;

namespace VirusTotalCore.Endpoints;

public class AddressIpEndpoint : Endpoint
{
    /// <summary>
    /// Constructor of IP Address endpoint
    /// </summary>
    /// <param name="apiKey">Your API key from VirusTotal</param>
    public AddressIpEndpoint(string apiKey)
    {
        Url += "/ip_addresses";
        ApiKey = apiKey;
        var options = new RestClientOptions(Url);
        Client = new RestClient(options);
    }

    /// <summary>
    /// Get report on given ip address
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="cancellationToken"></param>
    /// <returns cref="AddressAnalysisReport">Analysis report</returns>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    public async Task<AddressAnalysisReport> GetReport(string ipAddress, CancellationToken? cancellationToken)
    {
        var request = new RestRequest($"/{ipAddress}").AddHeader("x-apikey", ApiKey);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data").Deserialize<AddressAnalysisReport>(JsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    /// Get comments for given IP address
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="cursor"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="limits">Maximum number of comments to retrieve. Default is 10.</param>
    /// <returns cref="IpComment">Comments</returns>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    public async Task<IpComment> GetComments(string ipAddress, string? cursor, CancellationToken? cancellationToken, int limits = 10)
    {
        var requestUrl = $"/{ipAddress}/comments?limit={limits}";

        if (cursor is not null)
        {
            requestUrl += $"&cursor={cursor}";
        }

        var request = new RestRequest(requestUrl).AddHeader("x-apikey", ApiKey);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<IpComment>(JsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    /// Post your comment to IP address.
    /// Any word starting with # in your comment's text will be considered a tag,
    /// and added to the comment's tag attribute.
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="comment">Your comment for given IP address</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    /// <exception cref="AlreadyExistsException">Comment with given content is already exists.</exception>
    public async Task PostComment(string ipAddress, string comment, CancellationToken? cancellationToken)
    {
        var newComment = new AddComment
        {
            Data = new AddData<AddCommentAttribute>
            {
                Type = "comment",
                Attributes = new AddCommentAttribute
                {
                    Text = comment
                }
            }
        };
        var requestUrl = $"/{ipAddress}/comments";

        var serializedJson = JsonSerializer.Serialize(newComment, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddHeader("x-apikey", ApiKey)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }

    //TODO: Get objects related to an IP Address
    public void GetRelatedObjects(string ipAddress)
    {
        throw new NotImplementedException();
    }

    //TODO: Get objects descriptors related to an IP Address
    public void GetRelatedDescriptors(string ipAddress)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get community votes for given IP address.
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="cancellationToken"></param>
    /// <returns cref="Vote">IP address community votes</returns>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    public async Task<Vote> GetVotes(string ipAddress, CancellationToken? cancellationToken)
    {
        var requestUrl = $"/{ipAddress}/votes";

        var request = new RestRequest(requestUrl).AddHeader("x-apikey", ApiKey);
        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<Vote>(JsonSerializerOptions)!;
        return result;
    }
    
    /// <summary>
    /// Post your vote to IP Address.
    /// The verdict attribute must have be either harmless or malicious.
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param cref="VerdictType" name="verdict">Harmless or malicious</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentOutOfRangeException">Only harmless or malicious verdict is available.</exception>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    public async Task PostVote(string ipAddress, VerdictType verdict, CancellationToken? cancellationToken)
    {
        var userVerdict = verdict switch
        {
            VerdictType.Harmless => "harmless",
            VerdictType.Malicious => "malicious",
            _ => throw new ArgumentOutOfRangeException(nameof(verdict), verdict,
                "The verdict attribute must have be either harmless or malicious.")
        };

        var newVote = new AddVote
        {
            Data = new AddData<AddVoteAttribute>
            {
                Type = "vote",
                Attributes = new AddVoteAttribute
                {
                    Verdict = userVerdict
                }
            }
        };

        var requestUrl = $"/{ipAddress}/votes";
        var serializedJson = JsonSerializer.Serialize(newVote, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddHeader("x-apikey", ApiKey)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }
}