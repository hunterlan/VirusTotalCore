using System.Security.Cryptography;

namespace VirusTotalCore.Common.Hashing;

/// <summary>
/// Computes a SHA-256 hash of a stream.
/// </summary>
public sealed class Sha256HashProvider : IFileHashProvider
{
    /// <inheritdoc/>
    public async Task<string> ComputeHashAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = await sha256.ComputeHashAsync(stream, cancellationToken);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}
