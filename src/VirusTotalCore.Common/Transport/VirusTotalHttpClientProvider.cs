namespace VirusTotalCore.Common.Transport;

internal static class VirusTotalHttpClientProvider
{
    private const string ApiBaseUrl = "https://www.virustotal.com/api/v3/";

    public static HttpClient Create(string apiKey)
    {
        ValidateApiKey(apiKey);

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(ApiBaseUrl)
        };

        httpClient.DefaultRequestHeaders.Add("x-apikey", apiKey);

        return httpClient;
    }

    public static HttpClient Create(IHttpClientFactory httpClientFactory, string apiKey)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ValidateApiKey(apiKey);

        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(ApiBaseUrl);
        httpClient.DefaultRequestHeaders.Add("x-apikey", apiKey);
        return httpClient;
    }

    private static void ValidateApiKey(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException("API key should not be empty.", nameof(apiKey));
        }
    }
}