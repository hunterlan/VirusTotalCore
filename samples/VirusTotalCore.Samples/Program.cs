using VirusTotalCore.Comments.Endpoints;
using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Common.Models.Comments.Vote;
using VirusTotalCore.Domains.Endpoints;
using VirusTotalCore.Files.Endpoints;
using VirusTotalCore.IpAddresses.Endpoints;
using VirusTotalCore.Urls.Endpoints;

var apiKey = Environment.GetEnvironmentVariable("VIRUSTOTAL_API_KEY");
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.Error.WriteLine("Error: VIRUSTOTAL_API_KEY environment variable is not set.");
    Console.Error.WriteLine("Set it with: export VIRUSTOTAL_API_KEY=<your-api-key>");
    return 1;
}

await DemoFilesAsync(apiKey);
await DemoUrlsAsync(apiKey);
await DemoIpAddressesAsync(apiKey);
await DemoDomainsAsync(apiKey);
await DemoCommentsAsync(apiKey);

return 0;

// --- Files ---

async Task DemoFilesAsync(string key)
{
    Console.WriteLine("=== Files ===");
    try
    {
        var endpoint = new FilesEndpoint(key);

        var tempPath = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempPath, "VirusTotalCore sample file for scanning.");

        string fileHash;
        await using (var stream = File.OpenRead(tempPath))
        {
            fileHash = await endpoint.PostFile(stream, Path.GetFileName(tempPath), null);
        }

        File.Delete(tempPath);
        Console.WriteLine($"File submitted. Hash: {fileHash}");

        var report = await endpoint.GetReport(fileHash);
        Console.WriteLine($"File report — id: {report.Id}, type: {report.Type}");
    }
    catch (VirusTotalException ex)
    {
        Console.Error.WriteLine($"[Files] VirusTotal error: {ex.Message}");
    }

    Console.WriteLine();
}

// --- URLs ---

async Task DemoUrlsAsync(string key)
{
    Console.WriteLine("=== URLs ===");
    const string sampleUrl = "https://www.virustotal.com";
    try
    {
        var endpoint = new UrlEndpoint(key);

        var analysisId = await endpoint.Scan(sampleUrl);
        Console.WriteLine($"URL submitted for scanning. Analysis id: {analysisId}");

        var report = await endpoint.GetReport(sampleUrl);
        Console.WriteLine($"URL report — id: {report.Id}, type: {report.Type}");
    }
    catch (VirusTotalException ex)
    {
        Console.Error.WriteLine($"[URLs] VirusTotal error: {ex.Message}");
    }

    Console.WriteLine();
}

// --- IP Addresses ---

async Task DemoIpAddressesAsync(string key)
{
    Console.WriteLine("=== IP Addresses ===");
    const string sampleIp = "8.8.8.8";
    try
    {
        var endpoint = new AddressIpEndpoint(key);

        var report = await endpoint.GetReport(sampleIp);
        Console.WriteLine($"IP report for {sampleIp} — id: {report.Id}, type: {report.Type}");
    }
    catch (VirusTotalException ex)
    {
        Console.Error.WriteLine($"[IpAddresses] VirusTotal error: {ex.Message}");
    }

    Console.WriteLine();
}

// --- Domains ---

async Task DemoDomainsAsync(string key)
{
    Console.WriteLine("=== Domains ===");
    const string sampleDomain = "google.com";
    try
    {
        var endpoint = new DomainsEndpoint(key);

        var report = await endpoint.GetReport(sampleDomain);
        Console.WriteLine($"Domain report for {sampleDomain} — id: {report.Id}, type: {report.Type}");
    }
    catch (VirusTotalException ex)
    {
        Console.Error.WriteLine($"[Domains] VirusTotal error: {ex.Message}");
    }

    Console.WriteLine();
}

// --- Comments ---

async Task DemoCommentsAsync(string key)
{
    Console.WriteLine("=== Comments ===");
    try
    {
        var commentEndpoint = new CommentEndpoint(key);
        var ipEndpoint = new AddressIpEndpoint(key);

        var latestComments = await commentEndpoint.GetLatestComments(null, null);
        var firstComment = latestComments.Comments.FirstOrDefault();
        Console.WriteLine($"Latest comments count: {latestComments.Comments.Count()}");

        await ipEndpoint.AddComment("8.8.8.8", "Test comment from VirusTotalCore sample. #virustotalsample");
        Console.WriteLine("Comment added to 8.8.8.8.");

        if (firstComment is not null)
        {
            await commentEndpoint.AddVote(firstComment.Id, CommentVerdict.Positive);
            Console.WriteLine($"Upvoted comment {firstComment.Id}.");
        }
    }
    catch (VirusTotalException ex)
    {
        Console.Error.WriteLine($"[Comments] VirusTotal error: {ex.Message}");
    }

    Console.WriteLine();
}
