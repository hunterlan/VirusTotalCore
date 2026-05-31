namespace VirusTotalCore.Tests.Integration.ResponseBodies;

/// <summary>
/// Object Mother providing canonical VirusTotal JSON response bodies for Files endpoint stubs.
/// </summary>
internal static class FilesResponseBodies
{
    public static string GetReport(string sha256) =>
        $$"""
        {
          "data": {
            "type": "file",
            "id": "{{sha256}}",
            "attributes": {
              "type_description": "Win32 EXE",
              "tlsh": "T1234",
              "vhash": "abc",
              "type_tags": ["peexe"],
              "names": ["sample.exe"],
              "type_tag": "peexe",
              "total_votes": { "harmless": 1, "malicious": 0 },
              "last_analysis_results": {},
              "sha256": "{{sha256}}",
              "tags": ["peexe"],
              "ssdeep": "3:abc",
              "md5": "44d88612fea8a8f36de82e1278abb02f",
              "sha1": "3395856ce81f2b7382dee72602f798b642f14d4",
              "magic": "PE32 executable",
              "meaningful_name": "sample.exe",
              "last_analysis_stats": {
                "harmless": 0,
                "malicious": 0,
                "suspicious": 0,
                "undetected": 5,
                "timeout": 0,
                "type_unsupported": 0,
                "confirmed_timeout": 0,
                "failure": 0
              }
            }
          }
        }
        """;

    public static string GetRelated() => """{"data":[]}""";
}
