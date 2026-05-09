using System.Text.Json.Serialization;
using VirusTotalCore.Common.Models.Shared;

namespace VirusTotalCore.Common.Models.Comments;

public class CommentData
{
    [JsonPropertyName("Data")]
    public required IEnumerable<Comment> Comments { get; set; }
    public required Meta Meta { get; set; }
    public required LinkData Links { get; set; }
}
