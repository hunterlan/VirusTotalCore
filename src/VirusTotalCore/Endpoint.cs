﻿using System.Text.Json;
using RestSharp;
using VirusTotalCore.Exceptions;
using VirusTotalCore.Models;

namespace VirusTotalCore;

public abstract class Endpoint
{
    protected RestClient Client = null!;
    protected string Url = "https://www.virustotal.com/api/v3";
    private readonly string _apiKey = null!;
    protected string ApiKey
    {
        get => _apiKey;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Api key shouldn't be empty.");
            }

            _apiKey = value;
        }
    }

    protected readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = true
    };

    // https://docs.virustotal.com/reference/errors
    protected static Exception ThrowErrorResponseException(ErrorResponse error)
    {
        return error.Code switch
        {
            "AuthenticationRequiredError" => new AuthenticationRequiredException(error.Message),
            "BadRequestError" => new BadRequestException(error.Message),
            "InvalidArgumentError" => new InvalidArgumentException(error.Message),
            "NotAvailableYet" => new NotAvailableYetException(error.Message),
            "UnselectiveContentQueryError" => new UnselectiveContentQueryException(error.Message),
            "UnsupportedContentQueryError" => new UnsupportedContentQueryException(error.Message),
            "UserNotActiveError" => new UserNotActiveException(error.Message),
            "WrongCredentialsError" => new WrongCredentialsException(error.Message),
            "ForbiddenError" => new ForbiddenException(error.Message),
            "AlreadyExistsError" => new AlreadyExistsException(error.Message),
            "FailedDependencyError" => new FailedDependencyException(error.Message),
            "QuotaExceededError" => new QuotaExceededException(error.Message),
            "TooManyRequestsError" => new TooManyRequestsException(error.Message),
            "TransientError" => new TransientException(error.Message),
            "DeadlineExceededError" => new DeadlineExceededException(error.Message),
            "NotFoundError" => new NotFoundException(error.Message),
            _ => new Exception(error.Message)
        };
    }

    protected Exception HandleError(string errorContent)
    {
        var errorJsonDocument = JsonDocument.Parse(errorContent);
        var errorResponse = errorJsonDocument.RootElement.GetProperty("error").Deserialize<ErrorResponse>(JsonSerializerOptions)!;
        return ThrowErrorResponseException(errorResponse);
    }

    protected Task<RestResponse> GetResponse(RestRequest request, CancellationToken? cancellationToken)
    {
        return cancellationToken is not null
            ? Client.ExecuteGetAsync(request, cancellationToken.Value)
            : Client.ExecuteGetAsync(request);
    }

    protected Task<RestResponse> PostResponse(RestRequest request, CancellationToken? cancellationToken)
    {
        return cancellationToken is not null
            ? Client.ExecutePostAsync(request, cancellationToken.Value)
            : Client.ExecutePostAsync(request);
    }
}
