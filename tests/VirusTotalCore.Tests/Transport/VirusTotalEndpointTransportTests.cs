using System.Net;
using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Common.Transport;

namespace VirusTotalCore.Tests.Transport;

public sealed class VirusTotalEndpointTransportTests
{
    [Fact]
    public async Task GetAsync_WhenResponseIsForbidden_ThrowsForbiddenException()
    {
        using var httpClient = TestHttpClientFactory.Create(
            HttpStatusCode.Forbidden,
            """
            {
              "error": {
                "code": "ForbiddenError",
                "message": "Forbidden"
              }
            }
            """);
        var transport = new VirusTotalEndpointTransport(httpClient);

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            transport.GetAsync<object>("ip_addresses", "8.8.8.8", CancellationToken.None));
    }

    [Fact]
    public async Task GetAsync_WhenResponseIsNotFound_ThrowsNotFoundException()
    {
        using var httpClient = TestHttpClientFactory.Create(
            HttpStatusCode.NotFound,
            """
            {
              "error": {
                "code": "NotFoundError",
                "message": "Not found"
              }
            }
            """);
        var transport = new VirusTotalEndpointTransport(httpClient);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            transport.GetAsync<object>("ip_addresses", "0.0.0.0", CancellationToken.None));
    }

    [Fact]
    public async Task GetAsync_WhenUnauthorized_ThrowsWrongCredentialsException()
    {
        using var httpClient = TestHttpClientFactory.Create(
            HttpStatusCode.Unauthorized,
            """
            {
              "error": {
                "code": "WrongCredentialsError",
                "message": "Wrong API key"
              }
            }
            """);
        var transport = new VirusTotalEndpointTransport(httpClient);

        await Assert.ThrowsAsync<WrongCredentialsException>(() =>
            transport.GetAsync<object>("ip_addresses", "8.8.8.8", CancellationToken.None));
    }

    [Fact]
    public async Task GetRawAsync_WhenResponseIsForbidden_ThrowsForbiddenException()
    {
        using var httpClient = TestHttpClientFactory.Create(
            HttpStatusCode.Forbidden,
            """
            {
              "error": {
                "code": "ForbiddenError",
                "message": "Forbidden"
              }
            }
            """);
        var transport = new VirusTotalEndpointTransport(httpClient);

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            transport.GetRawAsync("ip_addresses", "8.8.8.8/comments?limit=10", CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAsync_WhenResponseIsNotFound_ThrowsNotFoundException()
    {
        using var httpClient = TestHttpClientFactory.Create(
            HttpStatusCode.NotFound,
            """
            {
              "error": {
                "code": "NotFoundError",
                "message": "Not found"
              }
            }
            """);
        var transport = new VirusTotalEndpointTransport(httpClient);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            transport.DeleteAsync("comments", "some-id", CancellationToken.None));
    }

    [Fact]
    public async Task PostAsync_WhenResponseIsForbidden_ThrowsForbiddenException()
    {
        using var httpClient = TestHttpClientFactory.Create(
            HttpStatusCode.Forbidden,
            """
            {
              "error": {
                "code": "ForbiddenError",
                "message": "Forbidden"
              }
            }
            """);
        var transport = new VirusTotalEndpointTransport(httpClient);

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            transport.PostAsync("ip_addresses", "8.8.8.8/votes", new { verdict = "harmless" }, CancellationToken.None));
    }

    [Fact]
    public async Task PostAsync_WhenResponseIsNotFound_ThrowsNotFoundException()
    {
        using var httpClient = TestHttpClientFactory.Create(
            HttpStatusCode.NotFound,
            """
            {
              "error": {
                "code": "NotFoundError",
                "message": "Not found"
              }
            }
            """);
        var transport = new VirusTotalEndpointTransport(httpClient);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            transport.PostAsync("ip_addresses", "0.0.0.0/votes", new { verdict = "harmless" }, CancellationToken.None));
    }

    [Fact]
    public async Task PostAsync_WhenUnauthorized_ThrowsWrongCredentialsException()
    {
        using var httpClient = TestHttpClientFactory.Create(
            HttpStatusCode.Unauthorized,
            """
            {
              "error": {
                "code": "WrongCredentialsError",
                "message": "Wrong API key"
              }
            }
            """);
        var transport = new VirusTotalEndpointTransport(httpClient);

        await Assert.ThrowsAsync<WrongCredentialsException>(() =>
            transport.PostAsync("ip_addresses", "8.8.8.8/votes", new { verdict = "harmless" }, CancellationToken.None));
    }
}
