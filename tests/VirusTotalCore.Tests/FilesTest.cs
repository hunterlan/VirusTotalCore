using Microsoft.Extensions.Configuration;
using VirusTotalCore.Files.Endpoints;
using Xunit.Abstractions;

namespace VirusTotalCore.Tests;

public class FilesTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private string ApiKey { get; }
    private FilesEndpoint Endpoint { get; }
    private const string TestFileHashId = "80e211f190a08c4a28da7c85fbd26b82";
    private const string GraphRelationship = "graphs";
    public FilesTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var settings = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        ApiKey = settings["apiKey"]!;
        Endpoint = new FilesEndpoint(ApiKey);
    }

    [Fact]
    public async Task PostSmallFile()
    {
        _testOutputHelper.WriteLine(Directory.GetCurrentDirectory());
        var filePath = $"test_files{Path.DirectorySeparatorChar}123.txt";
        await using var stream = File.OpenRead(filePath);
        var hash = await Endpoint.PostFile(stream, Path.GetFileName(filePath), null);
        Assert.True(!string.IsNullOrEmpty(hash));
    }

    [Fact]
    public async Task GetReportTest()
    {
        var report = await Endpoint.GetReport(TestFileHashId);
        Assert.True(report is not null);
    }

    [Fact]
    public async Task GetRelationshipsTest()
    {
        var relatedObjectsJson = await Endpoint.GetRelatedObjects(TestFileHashId, GraphRelationship, null);
        Assert.True(!string.IsNullOrEmpty(relatedObjectsJson));
    }

    [Fact]
    public async Task GetDescriptorsTest()
    {
        var descriptorsJson = await Endpoint.GetRelatedDescriptors(TestFileHashId, GraphRelationship, null);
        Assert.True(!string.IsNullOrEmpty(descriptorsJson));
    }
}