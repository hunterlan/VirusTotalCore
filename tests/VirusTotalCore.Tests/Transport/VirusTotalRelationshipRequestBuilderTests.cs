using VirusTotalCore.Common.Transport;

namespace VirusTotalCore.Tests.Transport;

public sealed class VirusTotalRelationshipRequestBuilderTests
{
    [Fact]
    public void BuildObjects_WithoutCursor_BuildsCorrectUrl()
    {
        var result = VirusTotalRelationshipRequestBuilder.BuildObjects("8.8.8.8", "comments", null, 10);

        Assert.Equal("8.8.8.8/comments?limit=10", result);
    }

    [Fact]
    public void BuildObjects_WithCursor_AppendsCursorToUrl()
    {
        var result = VirusTotalRelationshipRequestBuilder.BuildObjects("8.8.8.8", "comments", "cursor-value", 10);

        Assert.Equal("8.8.8.8/comments?limit=10&cursor=cursor-value", result);
    }

    [Fact]
    public void BuildObjects_UsesRelationshipParameter()
    {
        var result = VirusTotalRelationshipRequestBuilder.BuildObjects("8.8.8.8", "resolutions", null, 5);

        Assert.Equal("8.8.8.8/resolutions?limit=5", result);
    }

    [Fact]
    public void BuildDescriptors_WithoutCursor_BuildsCorrectUrl()
    {
        var result = VirusTotalRelationshipRequestBuilder.BuildDescriptors("example.com", "communicating_files", null, 10);

        Assert.Equal("example.com/relationships/communicating_files?limit=10", result);
    }

    [Fact]
    public void BuildDescriptors_WithCursor_AppendsCursorToUrl()
    {
        var result = VirusTotalRelationshipRequestBuilder.BuildDescriptors("example.com", "communicating_files", "next-page", 20);

        Assert.Equal("example.com/relationships/communicating_files?limit=20&cursor=next-page", result);
    }
}
