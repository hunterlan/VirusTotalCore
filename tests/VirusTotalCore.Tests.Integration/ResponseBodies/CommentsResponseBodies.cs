namespace VirusTotalCore.Tests.Integration.ResponseBodies;

/// <summary>
/// Object Mother providing canonical VirusTotal JSON response bodies for Comments endpoint stubs.
/// </summary>
internal static class CommentsResponseBodies
{
    public static string GetLatestComments() =>
        """
        {
          "Data": [],
          "meta": { "count": 0 },
          "links": { "self": "https://www.virustotal.com/api/v3/comments" }
        }
        """;

    public static string GetComment(string commentId) =>
        $$"""
        {
          "data": {
            "id": "{{commentId}}",
            "attributes": {
              "date": 0,
              "text": "test comment",
              "votes": { "positive": 0, "negative": 0, "abuse": 0 },
              "html": "<p>test comment</p>",
              "tags": []
            },
            "links": {
              "self": "https://www.virustotal.com/api/v3/comments/{{commentId}}"
            }
          }
        }
        """;
}
