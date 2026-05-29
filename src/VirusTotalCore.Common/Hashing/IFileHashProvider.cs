namespace VirusTotalCore.Common.Hashing;

/// <summary>
/// Computes a hash of a file stream for use with VirusTotal file submissions and lookups.
/// </summary>
public interface IFileHashProvider
{
    /// <summary>
    /// Computes the hash of the given stream and returns it as a lowercase hexadecimal string.
    /// </summary>
    /// <param name="stream">The stream to hash. The stream's position advances to the end after this call.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Lowercase hexadecimal hash string (e.g. SHA-256, SHA-1, or MD5).</returns>
    Task<string> ComputeHashAsync(Stream stream, CancellationToken cancellationToken = default);
}
