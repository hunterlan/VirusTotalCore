namespace VirusTotalCore.Tests.Integration.ResponseBodies;

/// <summary>
/// Object Mother providing canonical VirusTotal JSON response bodies for Domains endpoint stubs.
/// </summary>
internal static class DomainsResponseBodies
{
    public static string GetReport(string domain) =>
        $$"""
        {
          "data": {
            "type": "domain",
            "id": "{{domain}}",
            "attributes": {
              "categories": {},
              "last_analysis_date": 0,
              "last_analysis_stats": {
                "harmless": 0,
                "malicious": 0,
                "suspicious": 0,
                "undetected": 0,
                "timeout": 0
              },
              "last_analysis_results": {},
              "total_votes": { "harmless": 0, "malicious": 0 }
            }
          }
        }
        """;

    public static string GetRelated() => """{"data":[]}""";
}
