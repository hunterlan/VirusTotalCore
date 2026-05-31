namespace VirusTotalCore.Tests.Integration.ResponseBodies;

/// <summary>
/// Object Mother providing canonical VirusTotal JSON response bodies for IP Addresses endpoint stubs.
/// </summary>
internal static class IpAddressesResponseBodies
{
    public static string GetReport(string ipAddress) =>
        $$"""
        {
          "data": {
            "type": "ip_address",
            "id": "{{ipAddress}}",
            "attributes": {
              "network": "8.8.8.0/24",
              "as_owner": "Google LLC",
              "asn": 15169,
              "country": "US",
              "continent": "NA",
              "reputation": 0,
              "regional_internet_registry": "ARIN",
              "total_votes": { "harmless": 0, "malicious": 0 },
              "last_analysis_date": 0,
              "last_modification_date": 0,
              "last_analysis_stats": {
                "harmless": 0,
                "malicious": 0,
                "suspicious": 0,
                "undetected": 0,
                "timeout": 0
              },
              "last_analysis_results": {}
            }
          }
        }
        """;

    public static string GetRelated() => """{"data":[]}""";
}
