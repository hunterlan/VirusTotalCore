namespace VirusTotalCore.Common.Exceptions;
/// <summary>
/// Exception for WrongCredentialsException response: "Wrong API key"
/// </summary>
/// <param name="message">Error message</param>
public class WrongCredentialsException(string message) : Exception(message);
