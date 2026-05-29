namespace VirusTotalCore.Common.Transport;

internal static class VirusTotalRequestPathBuilder
{
    public static string Build(string endpointName, string requestUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(endpointName);
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUrl);

        return requestUrl.StartsWith('?') ? $"{endpointName}{requestUrl}" : $"{endpointName}/{requestUrl}";
    }
}