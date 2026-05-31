namespace VirusTotalCore.Tests.Integration.TestData.Files;

internal static class FileReportTestData
{
    public const string ReportJson = """
        {
          "data": {
            "type": "file",
            "id": "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08",
            "attributes": {
              "type_description": "ASCII text",
              "tlsh": "T1A4A002B34",
              "vhash": "000000000",
              "type_tags": ["text"],
              "names": ["test.txt"],
              "type_tag": "text",
              "total_votes": { "harmless": 0, "malicious": 0 },
              "last_analysis_results": {
                "SomeEngine": {
                  "category": "harmless",
                  "result": "clean",
                  "method": "blacklist"
                }
              },
              "sha256": "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08",
              "tags": [],
              "ssdeep": "3:a:a",
              "md5": "d41d8cd98f00b204e9800998ecf8427e",
              "sha1": "da39a3ee5e6b4b0d3255bfef95601890afd80709",
              "magic": "ASCII text",
              "last_analysis_stats": {
                "harmless": 70,
                "malicious": 0,
                "suspicious": 0,
                "undetected": 5,
                "timeout": 0,
                "type_unsupported": 0,
                "confirmed_timeout": 0,
                "failure": 0
              },
              "meaningful_name": "test.txt"
            }
          }
        }
        """;
}
