using System.Text;
using VirusTotalCore.Common.Hashing;

namespace VirusTotalCore.Tests;

public sealed class HashProviderTests
{
    // "hello" — well-known hashes for verification
    private static Stream ToStream(string text) =>
        new MemoryStream(Encoding.UTF8.GetBytes(text));

    [Fact]
    public async Task Sha256HashProvider_ReturnsCorrectLowercaseHex()
    {
        var provider = new Sha256HashProvider();
        using var stream = ToStream("hello");

        var hash = await provider.ComputeHashAsync(stream);

        Assert.Equal("2cf24dba5fb0a30e26e83b2ac5b9e29e1b161e5c1fa7425e73043362938b9824", hash);
    }

    [Fact]
    public async Task Sha1HashProvider_ReturnsCorrectLowercaseHex()
    {
        var provider = new Sha1HashProvider();
        using var stream = ToStream("hello");

        var hash = await provider.ComputeHashAsync(stream);

        Assert.Equal("aaf4c61ddcc5e8a2dabede0f3b482cd9aea9434d", hash);
    }

    [Fact]
    public async Task Md5HashProvider_ReturnsCorrectLowercaseHex()
    {
        var provider = new Md5HashProvider();
        using var stream = ToStream("hello");

        var hash = await provider.ComputeHashAsync(stream);

        Assert.Equal("5d41402abc4b2a76b9719d911017c592", hash);
    }
}
