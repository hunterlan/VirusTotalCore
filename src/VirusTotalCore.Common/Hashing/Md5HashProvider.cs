using System.Security.Cryptography;

namespace VirusTotalCore.Common.Hashing;

/// <summary>
/// Computes an MD5 hash of a stream.
/// </summary>
public sealed class Md5HashProvider : IFileHashProvider
{
    /// <inheritdoc/>
    public async Task<string> ComputeHashAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using var md5 = MD5.Create();
        var hashBytes = await md5.ComputeHashAsync(stream, cancellationToken);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}
