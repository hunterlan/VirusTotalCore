namespace VirusTotalCore.Common.Exceptions;

/// <summary>
/// Base class for all VirusTotal-specific exceptions.
/// </summary>
/// <param name="message">Error message</param>
public abstract class VirusTotalException(string message) : Exception(message);
