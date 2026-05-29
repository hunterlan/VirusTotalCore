using System.Security.Cryptography;

namespace VirusTotalCore.Common.Hashing;

/// <summary>
/// Computes a SHA-1 hash of a stream.
/// </summary>
public sealed class Sha1HashProvider : IFileHashProvider
{
    /// <inheritdoc/>
    public async Task<string> ComputeHashAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using var sha1 = SHA1.Create();
        var hashBytes = await sha1.ComputeHashAsync(stream, cancellationToken);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}
