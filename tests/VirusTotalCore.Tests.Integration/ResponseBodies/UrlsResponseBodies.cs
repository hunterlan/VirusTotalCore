namespace VirusTotalCore.Tests.Integration.ResponseBodies;

/// <summary>
/// Object Mother providing canonical VirusTotal JSON response bodies for URLs endpoint stubs.
/// </summary>
internal static class UrlsResponseBodies
{
    public static string GetReport(string urlId) =>
        $$"""
        {
          "data": {
            "type": "url",
            "id": "{{urlId}}",
            "attributes": {
              "categories": {},
              "first_submission_date": 0,
              "last_analysis_date": 0,
              "last_analysis_stats": {
                "harmless": 0,
                "malicious": 0,
                "suspicious": 0,
                "undetected": 0,
                "timeout": 0
              },
              "last_analysis_results": {},
              "last_final_url": "https://example.com",
              "last_http_response_content_length": 0,
              "last_http_response_content_sha256": "abc123",
              "last_http_response_headers": {},
              "outgoing_links": [],
              "html_meta": {},
              "reputation": 0,
              "tags": [],
              "threat_names": [],
              "total_votes": { "harmless": 0, "malicious": 0 }
            }
          }
        }
        """;

    public static string GetRelated() => """{"data":[]}""";

    public static string Scan() => """{"data":{"id":"u-abc123","type":"analysis"}}""";
}
