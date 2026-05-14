namespace VirusTotalCore.Common.Transport;

internal static class VirusTotalRelationshipRequestBuilder
{
    public static string BuildObjects(string objectId, string relationship, string? cursor, int limit)
    {
        var requestUrl = $"{objectId}/{relationship}?limit={limit}";

        if (!string.IsNullOrWhiteSpace(cursor))
        {
            requestUrl += $"&cursor={cursor}";
        }

        return requestUrl;
    }

    public static string BuildDescriptors(string objectId, string relationship, string? cursor, int limit)
    {
        var requestUrl = $"{objectId}/relationships/{relationship}?limit={limit}";

        if (!string.IsNullOrWhiteSpace(cursor))
        {
            requestUrl += $"&cursor={cursor}";
        }

        return requestUrl;
    }
}
