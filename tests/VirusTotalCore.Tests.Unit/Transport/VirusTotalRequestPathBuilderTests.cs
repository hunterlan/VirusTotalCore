using VirusTotalCore.Common.Transport;

namespace VirusTotalCore.Tests.Unit.Transport;

public sealed class VirusTotalRequestPathBuilderTests
{
    [Fact]
    public void Build_WithQueryOnly_PrefixesEndpointWithoutSlash()
    {
        var result = VirusTotalRequestPathBuilder.Build("ip_addresses", "?limit=10");

        Assert.Equal("ip_addresses?limit=10", result);
    }

    [Fact]
    public void Build_WithRelativePath_PrefixesEndpointWithSlash()
    {
        var result = VirusTotalRequestPathBuilder.Build("ip_addresses", "8.8.8.8");

        Assert.Equal("ip_addresses/8.8.8.8", result);
    }

    [Fact]
    public void Build_WithRelativePathAndQuery_PrefixesEndpointWithSlash()
    {
        var result = VirusTotalRequestPathBuilder.Build("domains", "example.com/comments?limit=10");

        Assert.Equal("domains/example.com/comments?limit=10", result);
    }

    [Fact]
    public void Build_WithEmptyEndpoint_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => VirusTotalRequestPathBuilder.Build("", "8.8.8.8"));
    }

    [Fact]
    public void Build_WithEmptyRequestUrl_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => VirusTotalRequestPathBuilder.Build("ip_addresses", ""));
    }
}
