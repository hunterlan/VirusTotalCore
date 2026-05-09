using VirusTotalCore.Common.Models.Shared;

namespace VirusTotalCore.Common.Models.Comments;

public class Comment
{
    public required Attributes Attributes { get; set; }
    public required string Id { get; set; }
    public required AttributeLinks Links { get; set; }
}
