using METL;
using METL.Enums;

namespace Examples;

/// <summary>
/// Basic examples demonstrating METL functionality.
/// </summary>
public static class BasicExamples
{
    /// <summary>
    /// Example 1: Simple byte array appending.
    /// </summary>
    public static void AppendBytesExample()
    {
        Console.WriteLine("=== Example 1: Append Bytes ===");

        // Create source and embed byte arrays
        var source = new byte[] { 0x01, 0x02, 0x03 };
        var embed = new byte[] { 0x04, 0x05 };

        // Append bytes
        var result = METLAppender.AppendBytesFromBytes(source, embed);

        Console.WriteLine($"Source length: {source.Length}");
        Console.WriteLine($"Embed length: {embed.Length}");
        Console.WriteLine($"Result length: {result.Length}");
        Console.WriteLine($"Result: {BitConverter.ToString(result)}");
    }

    /// <summary>
    /// Example 2: Append bytes from a file.
    /// </summary>
    public static void AppendBytesFromFileExample()
    {
        Console.WriteLine("\n=== Example 2: Append Bytes from File ===");

        // Create a temporary file
        var tempFile = Path.GetTempFileName();
        var embedData = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };
        File.WriteAllBytes(tempFile, embedData);

        try
        {
            var source = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // "Hello"
            var result = METLAppender.AppendBytesFromFile(source, tempFile);

            Console.WriteLine($"Appended {result.Length} total bytes");
            Console.WriteLine($"Result: {BitConverter.ToString(result)}");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    /// Example 3: Inject code from a string.
    /// </summary>
    public static void InjectFromStringExample()
    {
        Console.WriteLine("\n=== Example 3: Inject from String ===");

        var sourceCode = @"
using System;

namespace SimplePayload
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello from injected code!"");
            Console.WriteLine(""Generated at: [TIMESTAMP]"");
            Console.WriteLine(""ID: [UNIQUE_ID]"");
        }
    }
}";

        try
        {
            var executable = METLInjector.InjectFromString(sourceCode);
            Console.WriteLine($"Successfully compiled executable: {executable.Length} bytes");

            // Optionally save to disk
            var outputPath = "injected_example.exe";
            File.WriteAllBytes(outputPath, executable);
            Console.WriteLine($"Saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Compilation failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Example 4: Use built-in template.
    /// </summary>
    public static void UseBuiltInTemplateExample()
    {
        Console.WriteLine("\n=== Example 4: Built-in Template ===");

        // Create a sample payload file
        var payloadFile = Path.GetTempFileName();
        var payloadData = System.Text.Encoding.UTF8.GetBytes("This is sample payload data");
        File.WriteAllBytes(payloadFile, payloadData);

        try
        {
            var arguments = new Dictionary<string, string>
            {
                { "MALWARE_EMBED", payloadFile }
            };

            var executable = METLInjector.InjectFromBuiltInTemplate(
                BuiltInTemplates.PE32,
                arguments);

            Console.WriteLine($"Generated PE32 executable: {executable.Length} bytes");

            var outputPath = "template_example.exe";
            File.WriteAllBytes(outputPath, executable);
            Console.WriteLine($"Saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed: {ex.Message}");
        }
        finally
        {
            File.Delete(payloadFile);
        }
    }

    /// <summary>
    /// Example 5: Async operations.
    /// </summary>
    public static async Task AsyncOperationsExample()
    {
        Console.WriteLine("\n=== Example 5: Async Operations ===");

        var source = new byte[1000];
        var embed = new byte[500];
        new Random().NextBytes(source);
        new Random().NextBytes(embed);

        var result = await METLAppender.AppendBytesFromBytesAsync(source, embed);
        Console.WriteLine($"Async append completed: {result.Length} bytes");

        var sourceCode = @"
using System;
class Program { static void Main() => Console.WriteLine(""Async compiled!""); }";

        var executable = await METLInjector.InjectFromStringAsync(sourceCode);
        Console.WriteLine($"Async compilation completed: {executable.Length} bytes");
    }

    /// <summary>
    /// Example 6: Using logging.
    /// </summary>
    public static void LoggingExample()
    {
        Console.WriteLine("\n=== Example 6: Logging ===");

        // Configure logging
        METL.Diagnostics.METLLogger.SetMinimumLevel(METL.Diagnostics.LogLevel.Debug);
        METL.Diagnostics.METLLogger.SetLogHandler((level, message) =>
        {
            var color = level switch
            {
                METL.Diagnostics.LogLevel.Debug => ConsoleColor.Gray,
                METL.Diagnostics.LogLevel.Info => ConsoleColor.White,
                METL.Diagnostics.LogLevel.Warning => ConsoleColor.Yellow,
                METL.Diagnostics.LogLevel.Error => ConsoleColor.Red,
                _ => ConsoleColor.White
            };

            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"[{level}] {message}");
            Console.ForegroundColor = oldColor;
        });

        // Now operations will log
        var source = new byte[] { 0x01, 0x02 };
        var embed = new byte[] { 0x03 };
        var result = METLAppender.AppendBytesFromBytes(source, embed);

        Console.WriteLine($"Operation completed with logging enabled");
    }
}
