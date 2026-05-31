namespace VirusTotalCore.Tests.Integration.Files;

public sealed class PostFileTests : FilesEndpointTestBase
{
    private const string PostFilesPath = "/api/v3/files";

    public PostFileTests(FilesEndpointFixture fixture) : base(fixture) { }

    [Fact]
    public async Task PostFile_WhenSmallFile_ReturnsComputedSha256Hash()
    {
        Server
            .Given(Request.Create().WithPath(PostFilesPath).UsingPost())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody("{}"));

        await using var stream = new MemoryStream("test"u8.ToArray());
        var hash = await Endpoint.PostFile(stream, "test.txt", null);

        Assert.Equal(TEST_HASH, hash);
    }

    [Fact]
    public async Task PostFile_SendsMultipartFormData()
    {
        Server
            .Given(Request.Create().WithPath(PostFilesPath).UsingPost())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody("{}"));

        await using var stream = new MemoryStream("test"u8.ToArray());
        await Endpoint.PostFile(stream, "test.txt", null);

        var entry = Server.LogEntries.Single();
        var contentTypeHeader = entry.RequestMessage.Headers["Content-Type"].First();
        Assert.Contains("multipart/form-data", contentTypeHeader);
    }

    [Fact]
    public async Task PostFile_WhenForbidden_ThrowsForbiddenException()
    {
        Server
            .Given(Request.Create().WithPath(PostFilesPath).UsingPost())
            .RespondWith(WireMockErrorResponses.Forbidden());

        await using var stream = new MemoryStream("test"u8.ToArray());
        await Assert.ThrowsAsync<ForbiddenException>(() => Endpoint.PostFile(stream, "test.txt", null));
    }

    [Fact]
    public async Task PostFile_WhenUnauthorized_ThrowsWrongCredentialsException()
    {
        Server
            .Given(Request.Create().WithPath(PostFilesPath).UsingPost())
            .RespondWith(WireMockErrorResponses.WrongCredentials());

        await using var stream = new MemoryStream("test"u8.ToArray());
        await Assert.ThrowsAsync<WrongCredentialsException>(() => Endpoint.PostFile(stream, "test.txt", null));
    }

    [Fact]
    public async Task PostFile_WhenQuotaExceeded_ThrowsQuotaExceededException()
    {
        Server
            .Given(Request.Create().WithPath(PostFilesPath).UsingPost())
            .RespondWith(WireMockErrorResponses.QuotaExceeded());

        await using var stream = new MemoryStream("test"u8.ToArray());
        await Assert.ThrowsAsync<QuotaExceededException>(() => Endpoint.PostFile(stream, "test.txt", null));
    }
}
