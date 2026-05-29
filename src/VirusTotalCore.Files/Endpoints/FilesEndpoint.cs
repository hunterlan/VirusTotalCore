using System.Security.Cryptography;
using VirusTotalCore.Common;
using VirusTotalCore.Common.Models.Analysis;
using VirusTotalCore.Files.Models;

namespace VirusTotalCore.Files.Endpoints;

/// <summary>
/// Send files for scanning and get reports about them.
/// </summary>
public sealed class FilesEndpoint : BaseEndpoint, IFilesEndpoint
{
    public FilesEndpoint(string apiKey) : base(apiKey, "files") { }
    public FilesEndpoint(IHttpClientFactory customHttpClient, string apiKey) : base(customHttpClient, apiKey, "files") { }

    private const int MaxSmallSizeBytes = 33554432;

    /// <summary>
    /// Allows sending a file for scanning.
    /// If the file is less than 32 MB, it uses the default URL.
    /// If larger, it calls <see cref="GetUrlForPost"/> to obtain an upload URL for the large file.
    /// </summary>
    /// <param name="pathToFile">File path.</param>
    /// <param name="password">Optional password for the file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>SHA256 hash of the submitted file.</returns>
    /// <exception cref="FileNotFoundException">File does not exist.</exception>
    public async Task<string> PostFile(string pathToFile, string? password, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(pathToFile))
        {
            throw new FileNotFoundException($"Unable to find the specified file. Path is {pathToFile}");
        }

        string? uploadRequestUrl = null;
        var fileInfo = new FileInfo(pathToFile);

        if (fileInfo.Length > MaxSmallSizeBytes)
        {
            uploadRequestUrl = await GetUrlForPost(cancellationToken);
        }

        await using var sendStream = File.OpenRead(pathToFile);
        using var content = new MultipartFormDataContent();

        content.Add(new StreamContent(sendStream), "file", Path.GetFileName(sendStream.Name));
        if (password is not null)
        {
            content.Add(new StringContent(password), "password");
        }

        await PostMultipartAsync(uploadRequestUrl, content, cancellationToken);

        var sha256 = SHA256.Create();
        byte[] hashBytes;
        await using (var localStream = File.OpenRead(pathToFile))
        {
            hashBytes = await sha256.ComputeHashAsync(localStream);
        }

        return System.Text.Encoding.Default.GetString(hashBytes);
    }

    /// <summary>
    /// Get an upload URL for files larger than 32 MB.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>URL to use for the large file upload.</returns>
    private async Task<string> GetUrlForPost(CancellationToken cancellationToken)
    {
        const string rootPropertyName = "data";
        return await GetAsync<string>("upload_url", rootPropertyName, cancellationToken);
    }

    /// <summary>
    /// Get the analysis report for a file.
    /// </summary>
    /// <param name="fileHash">MD5, SHA-1, or SHA-256 hash of the file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns cref="AnalysisReport{FileReportAttributes}">Analysis report.</returns>
    public async Task<AnalysisReport<FileReportAttributes>> GetReport(string fileHash, CancellationToken cancellationToken = default)
    {
        const string rootPropertyName = "data";
        return await GetAsync<AnalysisReport<FileReportAttributes>>(fileHash, rootPropertyName, cancellationToken);
    }
}
