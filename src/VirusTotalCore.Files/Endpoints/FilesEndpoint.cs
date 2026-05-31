using VirusTotalCore.Common;
using VirusTotalCore.Common.Hashing;
using VirusTotalCore.Common.Models.Analysis;
using VirusTotalCore.Files.Models;

namespace VirusTotalCore.Files.Endpoints;

/// <summary>
/// Send files for scanning and get reports about them.
/// </summary>
public sealed class FilesEndpoint : BaseEndpoint, IFilesEndpoint
{
    private readonly IFileHashProvider _hashProvider;

    public FilesEndpoint(string apiKey, IFileHashProvider? hashProvider = null)
        : base(apiKey, "files")
    {
        _hashProvider = hashProvider ?? new Sha256HashProvider();
    }

    public FilesEndpoint(IHttpClientFactory customHttpClient, string apiKey, IFileHashProvider? hashProvider = null)
        : base(customHttpClient, apiKey, "files")
    {
        _hashProvider = hashProvider ?? new Sha256HashProvider();
    }

    internal FilesEndpoint(HttpClient httpClient, IFileHashProvider? hashProvider = null)
        : base(httpClient, "files")
    {
        _hashProvider = hashProvider ?? new Sha256HashProvider();
    }

    private const int MaxSmallSizeBytes = 33554432;

    /// <summary>
    /// Allows sending a file for scanning.
    /// If the file is less than 32 MB, it uses the default URL.
    /// If larger, it calls <see cref="GetUrlForPost"/> to obtain an upload URL for the large file.
    /// The stream must be seekable; the hash is computed before upload, then the stream is rewound.
    /// </summary>
    /// <param name="fileStream">Seekable stream of the file content. The caller is responsible for opening and disposing the stream.</param>
    /// <param name="fileName">File name reported to VirusTotal (e.g. "sample.exe").</param>
    /// <param name="password">Optional password for password-protected files.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Hash of the submitted file as a lowercase hexadecimal string.</returns>
    /// <exception cref="ArgumentException">The stream is not seekable.</exception>
    public async Task<string> PostFile(Stream fileStream, string fileName, string? password, CancellationToken cancellationToken = default)
    {
        if (!fileStream.CanSeek)
        {
            throw new ArgumentException("Stream must be seekable.", nameof(fileStream));
        }

        string? uploadRequestUrl = null;
        if (fileStream.Length > MaxSmallSizeBytes)
        {
            uploadRequestUrl = await GetUrlForPost(cancellationToken);
        }

        var hash = await _hashProvider.ComputeHashAsync(fileStream, cancellationToken);
        fileStream.Seek(0, SeekOrigin.Begin);

        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", fileName);
        if (password is not null)
        {
            content.Add(new StringContent(password), "password");
        }

        await PostMultipartAsync(uploadRequestUrl, content, cancellationToken);
        return hash;
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
