namespace METL.Diagnostics;

/// <summary>
/// Logging levels for METL operations.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Verbose debugging information.
    /// </summary>
    Debug,

    /// <summary>
    /// General informational messages.
    /// </summary>
    Info,

    /// <summary>
    /// Warning messages for potential issues.
    /// </summary>
    Warning,

    /// <summary>
    /// Error messages for failures.
    /// </summary>
    Error
}

/// <summary>
/// Simple logger for METL operations with configurable output.
/// </summary>
public static class METLLogger
{
    private static Action<LogLevel, string>? _logHandler;
    private static LogLevel _minimumLevel = LogLevel.Info;

    /// <summary>
    /// Sets a custom log handler for processing log messages.
    /// </summary>
    /// <param name="handler">Action to handle log messages. Receives log level and message.</param>
    public static void SetLogHandler(Action<LogLevel, string>? handler)
    {
        _logHandler = handler;
    }

    /// <summary>
    /// Sets the minimum log level that will be output.
    /// </summary>
    /// <param name="level">Minimum level to log.</param>
    public static void SetMinimumLevel(LogLevel level)
    {
        _minimumLevel = level;
    }

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Debug(string message) => Log(LogLevel.Debug, message);

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Info(string message) => Log(LogLevel.Info, message);

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Warning(string message) => Log(LogLevel.Warning, message);

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Error(string message) => Log(LogLevel.Error, message);

    /// <summary>
    /// Logs an exception with error level.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Optional message to include with the exception.</param>
    public static void Error(Exception exception, string? message = null)
    {
        var logMessage = string.IsNullOrEmpty(message)
            ? $"Exception: {exception.GetType().Name} - {exception.Message}"
            : $"{message} | Exception: {exception.GetType().Name} - {exception.Message}";

        Log(LogLevel.Error, logMessage);
    }

    private static void Log(LogLevel level, string message)
    {
        if (level < _minimumLevel)
        {
            return;
        }

        if (_logHandler != null)
        {
            _logHandler(level, message);
        }
        else
        {
            // Default: write to console
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Console.WriteLine($"[{timestamp}] [{level}] {message}");
        }
    }
}
