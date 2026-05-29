using VirusTotalCore.Common.Models.Analysis;
using VirusTotalCore.Files.Models;

namespace VirusTotalCore.Files.Endpoints;

/// <summary>
/// Defines operations for submitting files and querying VirusTotal file analysis resources.
/// </summary>
public interface IFilesEndpoint
{
    /// <summary>
    /// Send a file for scanning.
    /// If the file is less than 32 MB, it uses the default URL.
    /// If larger, it requests a special upload URL first.
    /// The stream must be seekable; <see cref="ArgumentException"/> is thrown otherwise.
    /// </summary>
    /// <param name="fileStream">Seekable stream of the file content. The caller is responsible for opening and disposing the stream.</param>
    /// <param name="fileName">File name reported to VirusTotal (e.g. "sample.exe").</param>
    /// <param name="password">Optional password for password-protected files.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Hash of the submitted file as a lowercase hexadecimal string, computed by the configured <see cref="VirusTotalCore.Common.Hashing.IFileHashProvider"/>.</returns>
    Task<string> PostFile(Stream fileStream, string fileName, string? password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the analysis report for a file by its hash.
    /// </summary>
    /// <param name="fileHash">MD5, SHA-1, or SHA-256 hash of the file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns cref="AnalysisReport{FileReportAttributes}">Analysis report.</returns>
    Task<AnalysisReport<FileReportAttributes>> GetReport(string fileHash, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get objects related to a file (e.g. graphs, network traffic).
    /// </summary>
    /// <param name="fileHash">Hash identifying the file.</param>
    /// <param name="relationship">Relationship name as defined by the VirusTotal API.</param>
    /// <param name="cursor">Continuation cursor for pagination.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="limit">Maximum number of related objects to retrieve. Default is 10.</param>
    /// <returns>JSON string of related objects.</returns>
    Task<string> GetRelatedObjects(string fileHash, string relationship, string? cursor, CancellationToken cancellationToken = default, int limit = 10);

    /// <summary>
    /// Get descriptors (IDs) of objects related to a file.
    /// </summary>
    /// <param name="fileHash">Hash identifying the file.</param>
    /// <param name="relationship">Relationship name as defined by the VirusTotal API.</param>
    /// <param name="cursor">Continuation cursor for pagination.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="limit">Maximum number of descriptors to retrieve. Default is 10.</param>
    /// <returns>JSON string of related object descriptors.</returns>
    Task<string> GetRelatedDescriptors(string fileHash, string relationship, string? cursor, CancellationToken cancellationToken = default, int limit = 10);
}
