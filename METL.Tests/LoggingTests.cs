using Microsoft.VisualStudio.TestTools.UnitTesting;
using METL.Diagnostics;

namespace METL.Tests;

[TestClass]
public class LoggingTests
{
    [TestCleanup]
    public void Cleanup()
    {
        // Reset logger after each test
        METLLogger.SetLogHandler(null);
        METLLogger.SetMinimumLevel(LogLevel.Info);
    }

    [TestMethod]
    public void METLLogger_WithCustomHandler_CallsHandler()
    {
        // Arrange
        var logMessages = new List<(LogLevel, string)>();
        METLLogger.SetLogHandler((level, message) => logMessages.Add((level, message)));
        METLLogger.SetMinimumLevel(LogLevel.Debug); // Allow all messages

        // Act
        METLLogger.Info("Test message");
        METLLogger.Debug("Debug message");
        METLLogger.Warning("Warning message");
        METLLogger.Error("Error message");

        // Assert
        Assert.AreEqual(4, logMessages.Count);
        Assert.IsTrue(logMessages.Any(m => m.Item2 == "Test message"));
    }

    [TestMethod]
    public void METLLogger_WithMinimumLevel_FiltersMessages()
    {
        // Arrange
        var logMessages = new List<(LogLevel, string)>();
        METLLogger.SetLogHandler((level, message) => logMessages.Add((level, message)));
        METLLogger.SetMinimumLevel(LogLevel.Warning);

        // Act
        METLLogger.Debug("Debug message");
        METLLogger.Info("Info message");
        METLLogger.Warning("Warning message");
        METLLogger.Error("Error message");

        // Assert
        Assert.AreEqual(2, logMessages.Count);
        Assert.IsTrue(logMessages.All(m => m.Item1 >= LogLevel.Warning));
    }

    [TestMethod]
    public void METLLogger_ErrorWithException_LogsExceptionDetails()
    {
        // Arrange
        var logMessages = new List<string>();
        METLLogger.SetLogHandler((level, message) => logMessages.Add(message));
        var exception = new InvalidOperationException("Test exception");

        // Act
        METLLogger.Error(exception, "Operation failed");

        // Assert
        Assert.AreEqual(1, logMessages.Count);
        Assert.IsTrue(logMessages[0].Contains("Operation failed"));
        Assert.IsTrue(logMessages[0].Contains("InvalidOperationException"));
        Assert.IsTrue(logMessages[0].Contains("Test exception"));
    }

    [TestMethod]
    public void METLLogger_WithNullHandler_DoesNotThrow()
    {
        // Arrange
        METLLogger.SetLogHandler(null);

        // Act & Assert (should not throw)
        METLLogger.Info("Test message");
        METLLogger.Debug("Debug message");
        METLLogger.Error(new Exception("Test"));
    }
}
