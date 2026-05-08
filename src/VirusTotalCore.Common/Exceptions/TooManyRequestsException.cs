namespace VirusTotalCore.Common.Exceptions;

public class TooManyRequestsException(string message) : Exception(message);
