using System.Text.Json;
using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Common.Models;

// https://docs.virustotal.com/reference/errors
namespace VirusTotalCore.Common;

internal static class VirusTotalErrorHandler
{
    public static VirusTotalException Handle(string errorContent)
    {
        using var errorJsonDocument = JsonDocument.Parse(errorContent);
        var errorResponse = errorJsonDocument.RootElement.GetProperty("error").Deserialize<ErrorResponse>(VirusTotalJsonOptions.Default)!;
        return ThrowByResponse(errorResponse);
    }

    private static VirusTotalException ThrowByResponse(ErrorResponse error)
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
            _ => new VirusTotalException(error.Message)
        };
    }
}