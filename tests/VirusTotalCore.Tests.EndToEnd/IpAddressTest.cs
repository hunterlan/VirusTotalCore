using Microsoft.Extensions.Configuration;
using VirusTotalCore.Comments.Endpoints;
using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Common.Models.Comments;
using VirusTotalCore.IpAddresses.Endpoints;

namespace VirusTotalCore.Tests.EndToEnd;

public class IpAddressTest
{
    private const string IpAddress = "8.8.8.8";
    private const string GraphRelationship = "graphs";
    private string ApiKey { get; }
    private readonly VirusTotalCore.IpAddresses.Endpoints.AddressIpEndpoint _endpoint;

    public IpAddressTest()
    {
        var settings = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        ApiKey = settings["apiKey"]!;
        _endpoint = new AddressIpEndpoint(ApiKey);
    }

    [Fact]
    public async Task IncorrectIpAddressReport()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _endpoint.GetReport("", default));
    }

    [Fact]
    public async Task IpAddressReport()
    {
        var report = await _endpoint.GetReport(IpAddress);
        Assert.True(report is { Id: IpAddress, Type: "ip_address" });
    }

    [Fact]
    public async Task IpAddressComments()
    {
        var ipComment = await _endpoint.GetComments(IpAddress, null);
        Assert.True(ipComment.Comments.Count() is 10);
    }

    [Fact]
    public async Task IpAddressVotes()
    {
        var ipVotes = await _endpoint.GetVotes(IpAddress);
        Assert.True(ipVotes.Data.Any() && ipVotes.Data.First().Attributes is not null);
    }

    [Fact]
    public async Task CatchErrorOnIncorrectPostComment()
    {
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _endpoint.AddComment("8.8.8.8", ""));
    }

    [Fact]
    public async Task DuplicateErrorPostComment()
    {
        await Assert.ThrowsAsync<AlreadyExistsException>(() =>
            _endpoint.AddComment(IpAddress, "Lorem ipsum dolor sit ..."));
    }

    [Fact]
    public async Task AddCommentTest()
    {
        var comment = "This is test comment for VirusTotalCore library";
        Comment? publishedComment = null;
        var cancellationToken = CancellationToken.None;
        var commentEndpoint = new CommentEndpoint(ApiKey);

        await _endpoint.AddComment(IpAddress, comment, cancellationToken);
        try
        {
            var commentData = await _endpoint.GetComments(IpAddress, null, cancellationToken);
            publishedComment = commentData.Comments.First();
            Assert.Equal(comment, publishedComment.Attributes.Text);
        }
        finally
        {
            if (publishedComment is not null)
            {
                var commentId = publishedComment.Id;
                await commentEndpoint.Delete(commentId, cancellationToken);
            }
        }
    }

    [Fact]
    public async Task GetRelationshipsTest()
    {
        var relatedObjectsJson = await _endpoint.GetRelatedObjects(IpAddress, GraphRelationship, null);
        Assert.True(!string.IsNullOrEmpty(relatedObjectsJson));
    }

    [Fact]
    public async Task GetDescriptorsTest()
    {
        var descriptorsJson = await _endpoint.GetRelatedDescriptors(IpAddress, GraphRelationship, null);
        Assert.True(!string.IsNullOrEmpty(descriptorsJson));
    }

    /*
     * TODO: Write test for posting vote
     * Find a way to delete the vote
     */
}
