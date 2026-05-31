namespace VirusTotalCore.Tests.Integration.Files;

public sealed class GetRelatedDescriptorsTests : FilesEndpointTestBase
{
    public GetRelatedDescriptorsTests(FilesEndpointFixture fixture) : base(fixture) { }

    private const string Relationship = "graphs";
    private const string DescriptorsPath = $"/api/v3/files/{TEST_HASH}/relationships/{Relationship}";

    [Fact]
    public async Task GetRelatedDescriptors_WithNullCursor_ReturnsJsonAndHasLimitParam()
    {
        Server
            .Given(Request.Create().WithPath(DescriptorsPath).UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody("""{"data": []}"""));

        var result = await Endpoint.GetRelatedDescriptors(TEST_HASH, Relationship, null);

        Assert.NotNull(result);
        var requestedUrl = Server.LogEntries.Single().RequestMessage.Url;
        Assert.Contains("limit=", requestedUrl);
        Assert.DoesNotContain("cursor=", requestedUrl);
    }

    [Fact]
    public async Task GetRelatedDescriptors_WithCursor_AppendsCursorToQueryString()
    {
        const string Cursor = "some-cursor-value";
        Server
            .Given(Request.Create().WithPath(DescriptorsPath).UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody("""{"data": []}"""));

        var result = await Endpoint.GetRelatedDescriptors(TEST_HASH, Relationship, Cursor);

        Assert.NotNull(result);
        var requestedUrl = Server.LogEntries.Single().RequestMessage.Url;
        Assert.Contains($"cursor={Cursor}", requestedUrl);
    }

    [Fact]
    public async Task GetRelatedDescriptors_WhenNotFound_ThrowsNotFoundException()
    {
        Server
            .Given(Request.Create().WithPath(DescriptorsPath).UsingGet())
            .RespondWith(WireMockErrorResponses.NotFound());

        await Assert.ThrowsAsync<NotFoundException>(() =>
            Endpoint.GetRelatedDescriptors(TEST_HASH, Relationship, null));
    }
}
