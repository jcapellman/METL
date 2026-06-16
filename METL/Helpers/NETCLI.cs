using System.Diagnostics;

namespace METL.Helpers;

/// <summary>
/// Wrapper for .NET CLI operations.
/// </summary>
public class NETCLI
{
    private void DotnetProcess(string arguments, string? workingPath = null)
    {
        var procInfo = new ProcessStartInfo("dotnet")
        {
            Arguments = arguments,
            WorkingDirectory = workingPath ?? AppContext.BaseDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = procInfo };

        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            var error = process.StandardError.ReadToEnd();
            throw new InvalidOperationException($"dotnet process failed with exit code {process.ExitCode}: {error}");
        }
    }

    private async Task DotnetProcessAsync(string arguments, string? workingPath = null, CancellationToken cancellationToken = default)
    {
        var procInfo = new ProcessStartInfo("dotnet")
        {
            Arguments = arguments,
            WorkingDirectory = workingPath ?? AppContext.BaseDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = procInfo };

        process.Start();

        await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

        if (process.ExitCode != 0)
        {
            var error = await process.StandardError.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
            throw new InvalidOperationException($"dotnet process failed with exit code {process.ExitCode}: {error}");
        }
    }

    /// <summary>
    /// Compiles source code and returns the compiled executable as bytes.
    /// </summary>
    /// <param name="sourceCodeContent">The C# source code to compile.</param>
    /// <param name="projectName">The name for the temporary project.</param>
    /// <returns>The compiled executable as a byte array.</returns>
    public byte[] CompileAndReturnBytes(string sourceCodeContent, string projectName)
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory, projectName);

        try
        {
            DotnetProcess($"new console -n {projectName}");

            File.WriteAllText(Path.Combine(fullPath, "Program.cs"), sourceCodeContent);

            DotnetProcess("publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false", fullPath);

            var outputPath = Path.Combine(fullPath, $"bin{Path.DirectorySeparatorChar}Release{Path.DirectorySeparatorChar}net10.0{Path.DirectorySeparatorChar}win-x64{Path.DirectorySeparatorChar}publish{Path.DirectorySeparatorChar}{projectName}.exe");

            return File.ReadAllBytes(outputPath);
        }
        finally
        {
            // Cleanup temporary project directory
            if (Directory.Exists(fullPath))
            {
                try
                {
                    Directory.Delete(fullPath, true);
                }
                catch
                {
                    // Best effort cleanup
                }
            }
        }
    }

    /// <summary>
    /// Asynchronously compiles source code and returns the compiled executable as bytes.
    /// </summary>
    /// <param name="sourceCodeContent">The C# source code to compile.</param>
    /// <param name="projectName">The name for the temporary project.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the compiled executable as a byte array.</returns>
    public async Task<byte[]> CompileAndReturnBytesAsync(string sourceCodeContent, string projectName, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory, projectName);

        try
        {
            await DotnetProcessAsync($"new console -n {projectName}", cancellationToken: cancellationToken).ConfigureAwait(false);

            await File.WriteAllTextAsync(Path.Combine(fullPath, "Program.cs"), sourceCodeContent, cancellationToken).ConfigureAwait(false);

            await DotnetProcessAsync("publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false", fullPath, cancellationToken).ConfigureAwait(false);

            var outputPath = Path.Combine(fullPath, $"bin{Path.DirectorySeparatorChar}Release{Path.DirectorySeparatorChar}net10.0{Path.DirectorySeparatorChar}win-x64{Path.DirectorySeparatorChar}publish{Path.DirectorySeparatorChar}{projectName}.exe");

            return await File.ReadAllBytesAsync(outputPath, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            // Cleanup temporary project directory
            if (Directory.Exists(fullPath))
            {
                try
                {
                    Directory.Delete(fullPath, true);
                }
                catch
                {
                    // Best effort cleanup
                }
            }
        }
    }
}
