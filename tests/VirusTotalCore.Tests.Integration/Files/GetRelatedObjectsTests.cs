namespace VirusTotalCore.Tests.Integration.Files;

public sealed class GetRelatedObjectsTests : FilesEndpointTestBase
{
    public GetRelatedObjectsTests(FilesEndpointFixture fixture) : base(fixture) { }

    private const string Relationship = "graphs";
    private const string ObjectsPath = $"/api/v3/files/{TEST_HASH}/{Relationship}";

    [Fact]
    public async Task GetRelatedObjects_WithNullCursor_ReturnsJsonAndHasLimitParam()
    {
        Server
            .Given(Request.Create().WithPath(ObjectsPath).UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody("""{"data": []}"""));

        var result = await Endpoint.GetRelatedObjects(TEST_HASH, Relationship, null);

        Assert.NotNull(result);
        var requestedUrl = Server.LogEntries.Single().RequestMessage.Url;
        Assert.Contains("limit=", requestedUrl);
        Assert.DoesNotContain("cursor=", requestedUrl);
    }

    [Fact]
    public async Task GetRelatedObjects_WithCursor_AppendsCursorToQueryString()
    {
        const string Cursor = "some-cursor-value";
        Server
            .Given(Request.Create().WithPath(ObjectsPath).UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody("""{"data": []}"""));

        var result = await Endpoint.GetRelatedObjects(TEST_HASH, Relationship, Cursor);

        Assert.NotNull(result);
        var requestedUrl = Server.LogEntries.Single().RequestMessage.Url;
        Assert.Contains($"cursor={Cursor}", requestedUrl);
    }

    [Fact]
    public async Task GetRelatedObjects_WhenNotFound_ThrowsNotFoundException()
    {
        Server
            .Given(Request.Create().WithPath(ObjectsPath).UsingGet())
            .RespondWith(WireMockErrorResponses.NotFound());

        await Assert.ThrowsAsync<NotFoundException>(() =>
            Endpoint.GetRelatedObjects(TEST_HASH, Relationship, null));
    }
}
