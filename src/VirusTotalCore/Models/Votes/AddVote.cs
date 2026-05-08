using System.Diagnostics.CodeAnalysis;
using VirusTotalCore.Common.Enums;
using VirusTotalCore.Common.Models.Shared;

namespace VirusTotalCore.Models.Votes;

public class AddVote
{
    public required AddData<AddVoteAttribute> Data { get; set; }

    [SetsRequiredMembers]
    public AddVote(VerdictType verdict)
    {
        Data = new AddData<AddVoteAttribute>
        {
            Type = "vote",
            Attributes = new AddVoteAttribute
            {
                Verdict = verdict.ToString().ToLower()
            }
        };
        
    }
}