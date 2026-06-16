using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using METL.Diagnostics;
using METL.Enums;
using METL.Helpers;
using METL.InjectorMerges.Base;

namespace METL;

/// <summary>
/// Provides methods for injecting and compiling code into executables.
/// </summary>
public static class METLInjector
{
    private static byte[] CompileCodeToBytes(string sourceCode)
    {
        METLLogger.Debug($"CompileCodeToBytes called - Code length: {sourceCode.Length} characters");

        var projectName = Guid.NewGuid().ToString().Replace("-", "");
        METLLogger.Info($"Starting compilation with project name: {projectName}");

        try
        {
            var result = new NETCLI().CompileAndReturnBytes(sourceCode, projectName);
            METLLogger.Info($"Compilation successful - Output size: {result.Length} bytes");
            return result;
        }
        catch (Exception ex)
        {
            METLLogger.Error(ex, "Compilation failed");
            throw;
        }
    }

    private static async Task<byte[]> CompileCodeToBytesAsync(string sourceCode, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var projectName = Guid.NewGuid().ToString().Replace("-", "");
        return await new NETCLI().CompileAndReturnBytesAsync(sourceCode, projectName, cancellationToken).ConfigureAwait(false);
    }

    private static string ParseAndMergeSource(string sourceFile, Dictionary<string, string>? arguments)
    {
        METLLogger.Debug("Starting mail merge parsing");

        var merges = Assembly.GetAssembly(typeof(BaseInjectorMerge))?
            .GetTypes()
            .Where(a => a.BaseType == typeof(BaseInjectorMerge) && !a.IsAbstract)
            .Select(b => (BaseInjectorMerge)Activator.CreateInstance(b)!)
            .ToList();

        if (merges == null)
        {
            METLLogger.Error("Failed to obtain mail merges");
            throw new InvalidOperationException($"Failed to obtain mail merges");
        }

        METLLogger.Info($"Found {merges.Count} mail merge handlers");
        arguments ??= new Dictionary<string, string>();

        var result = merges.Aggregate(sourceFile, (current, merge) =>
        {
            var hasValue = arguments.ContainsKey(merge.FIELD_NAME);
            METLLogger.Debug($"Processing merge field: {merge.FIELD_NAME} - Has value: {hasValue}");
            return current.Replace($"[{merge.FIELD_NAME}]",
                merge.Merge(hasValue ? arguments[merge.FIELD_NAME] : null));
        });

        return result;
    }

    #region Built-in Template Methods

    /// <summary>
    /// Injects code from a built-in template with the specified arguments.
    /// </summary>
    /// <param name="template">The built-in template to use.</param>
    /// <param name="arguments">Dictionary of arguments for mail merge.</param>
    /// <returns>Compiled executable as byte array.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when template is invalid.</exception>
    public static byte[] InjectFromBuiltInTemplate(BuiltInTemplates template, Dictionary<string, string>? arguments = null)
    {
        var assembly = typeof(METLInjector).GetTypeInfo().Assembly;
        var templateName = template.ToString();

        using var resource = assembly.GetManifestResourceStream($"METL.IncludedScripts.{templateName}.cs");

        if (resource == null)
        {
            throw new ArgumentOutOfRangeException(nameof(template), $"{templateName} is an invalid template name");
        }

        using var reader = new StreamReader(resource);
        var text = reader.ReadToEnd();

        return CompileCodeToBytes(ParseAndMergeSource(text, arguments ?? new Dictionary<string, string>()));
    }

    /// <summary>
    /// Asynchronously injects code from a built-in template with the specified arguments.
    /// </summary>
    /// <param name="template">The built-in template to use.</param>
    /// <param name="arguments">Dictionary of arguments for mail merge.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the compiled executable as byte array.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when template is invalid.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public static async Task<byte[]> InjectFromBuiltInTemplateAsync(
        BuiltInTemplates template,
        Dictionary<string, string>? arguments = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var assembly = typeof(METLInjector).GetTypeInfo().Assembly;
        var templateName = template.ToString();

        await using var resource = assembly.GetManifestResourceStream($"METL.IncludedScripts.{templateName}.cs");

        if (resource == null)
        {
            throw new ArgumentOutOfRangeException(nameof(template), $"{templateName} is an invalid template name");
        }

        using var reader = new StreamReader(resource);
        var text = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);

        return await CompileCodeToBytesAsync(ParseAndMergeSource(text, arguments ?? new Dictionary<string, string>()), cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region String-based Methods

    /// <summary>
    /// Injects code from a string with the specified arguments.
    /// </summary>
    /// <param name="sourceCode">The source code to compile.</param>
    /// <param name="arguments">Dictionary of arguments for mail merge.</param>
    /// <returns>Compiled executable as byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when sourceCode is null or empty.</exception>
    public static byte[] InjectFromString([NotNull] string sourceCode, Dictionary<string, string>? arguments = null)
    {
        if (string.IsNullOrEmpty(sourceCode))
        {
            throw new ArgumentNullException(nameof(sourceCode));
        }

        return CompileCodeToBytes(ParseAndMergeSource(sourceCode, arguments ?? new Dictionary<string, string>()));
    }

    /// <summary>
    /// Asynchronously injects code from a string with the specified arguments.
    /// </summary>
    /// <param name="sourceCode">The source code to compile.</param>
    /// <param name="arguments">Dictionary of arguments for mail merge.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the compiled executable as byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when sourceCode is null or empty.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public static async Task<byte[]> InjectFromStringAsync(
        [NotNull] string sourceCode,
        Dictionary<string, string>? arguments = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(sourceCode))
        {
            throw new ArgumentNullException(nameof(sourceCode));
        }

        return await CompileCodeToBytesAsync(ParseAndMergeSource(sourceCode, arguments ?? new Dictionary<string, string>()), cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region File-based Methods

    /// <summary>
    /// Injects code from a file with the specified arguments.
    /// </summary>
    /// <param name="sourceFileName">The path to the source code file.</param>
    /// <param name="arguments">Dictionary of arguments for mail merge.</param>
    /// <returns>Compiled executable as byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when sourceFileName is null or empty.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the source file does not exist.</exception>
    public static byte[] InjectFromFile([NotNull] string sourceFileName, Dictionary<string, string>? arguments = null)
    {
        if (string.IsNullOrEmpty(sourceFileName))
        {
            throw new ArgumentNullException(nameof(sourceFileName));
        }

        if (!File.Exists(sourceFileName))
        {
            throw new FileNotFoundException($"Source file not found: {sourceFileName}", sourceFileName);
        }

        var sourceCodeFileText = File.ReadAllText(sourceFileName);

        return CompileCodeToBytes(ParseAndMergeSource(sourceCodeFileText, arguments ?? new Dictionary<string, string>()));
    }

    /// <summary>
    /// Asynchronously injects code from a file with the specified arguments.
    /// </summary>
    /// <param name="sourceFileName">The path to the source code file.</param>
    /// <param name="arguments">Dictionary of arguments for mail merge.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the compiled executable as byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when sourceFileName is null or empty.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the source file does not exist.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public static async Task<byte[]> InjectFromFileAsync(
        [NotNull] string sourceFileName,
        Dictionary<string, string>? arguments = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(sourceFileName))
        {
            throw new ArgumentNullException(nameof(sourceFileName));
        }

        if (!File.Exists(sourceFileName))
        {
            throw new FileNotFoundException($"Source file not found: {sourceFileName}", sourceFileName);
        }

        var sourceCodeFileText = await File.ReadAllTextAsync(sourceFileName, cancellationToken).ConfigureAwait(false);

        return await CompileCodeToBytesAsync(ParseAndMergeSource(sourceCodeFileText, arguments ?? new Dictionary<string, string>()), cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region Legacy Methods (Deprecated)

    /// <summary>
    /// [Obsolete] Injects malware from a built-in template. Use InjectFromBuiltInTemplate instead.
    /// </summary>
    [Obsolete("Use InjectFromBuiltInTemplate instead. This method will be removed in a future version.")]
    public static byte[] InjectMalwareFromTemplate(BuiltInTemplates template, [NotNull] string malwareFileName)
    {
        var arguments = new Dictionary<string, string>
        {
            { "MALWARE_EMBED", malwareFileName }
        };

        return InjectFromBuiltInTemplate(template, arguments);
    }

    /// <summary>
    /// [Obsolete] Injects malware from a template by name. Use InjectFromBuiltInTemplate instead.
    /// </summary>
    [Obsolete("Use InjectFromBuiltInTemplate instead. This method will be removed in a future version.")]
    public static byte[] InjectMalwareFromTemplate([NotNull] string templateName, [NotNull] string malwareFileName)
    {
        if (Enum.TryParse<BuiltInTemplates>(templateName, out var template))
        {
            return InjectMalwareFromTemplate(template, malwareFileName);
        }

        throw new ArgumentException($"Invalid template name: {templateName}", nameof(templateName));
    }

    /// <summary>
    /// [Obsolete] Injects malware from a file. Use InjectFromFile instead.
    /// </summary>
    [Obsolete("Use InjectFromFile instead. This method will be removed in a future version.")]
    public static byte[] InjectMalwareFromFile([NotNull] string sourceFileName, [NotNull] string malwareFileName)
    {
        var arguments = new Dictionary<string, string>
        {
            { "MALWARE_EMBED", malwareFileName }
        };

        return InjectFromFile(sourceFileName, arguments);
    }

    #endregion
}
