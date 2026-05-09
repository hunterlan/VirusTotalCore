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
    /// </summary>
    /// <param name="pathToFile">Path to the file on disk.</param>
    /// <param name="password">Optional password for the file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>SHA256 hash of the submitted file.</returns>
    Task<string> PostFile(string pathToFile, string? password, CancellationToken? cancellationToken);

    /// <summary>
    /// Get the analysis report for a file by its hash.
    /// </summary>
    /// <param name="fileHash">MD5, SHA-1, or SHA-256 hash of the file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns cref="AnalysisReport{FileReportAttributes}">Analysis report.</returns>
    Task<AnalysisReport<FileReportAttributes>> GetReport(string fileHash, CancellationToken? cancellationToken);

    /// <summary>
    /// Get objects related to a file (e.g. graphs, network traffic).
    /// </summary>
    /// <param name="fileHash">Hash identifying the file.</param>
    /// <param name="relationship">Relationship name as defined by the VirusTotal API.</param>
    /// <param name="cursor">Continuation cursor for pagination.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="limit">Maximum number of related objects to retrieve. Default is 10.</param>
    /// <returns>JSON string of related objects.</returns>
    Task<string> GetRelatedObjects(string fileHash, string relationship, string? cursor, CancellationToken? cancellationToken, int limit = 10);

    /// <summary>
    /// Get descriptors (IDs) of objects related to a file.
    /// </summary>
    /// <param name="fileHash">Hash identifying the file.</param>
    /// <param name="relationship">Relationship name as defined by the VirusTotal API.</param>
    /// <param name="cursor">Continuation cursor for pagination.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="limit">Maximum number of descriptors to retrieve. Default is 10.</param>
    /// <returns>JSON string of related object descriptors.</returns>
    Task<string> GetRelatedDescriptors(string fileHash, string relationship, string? cursor, CancellationToken? cancellationToken, int limit = 10);
}
