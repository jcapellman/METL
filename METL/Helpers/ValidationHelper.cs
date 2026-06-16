namespace METL.Helpers;

/// <summary>
/// Helper methods for validating inputs.
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Validates that a byte array is not null and optionally checks minimum size.
    /// </summary>
    /// <param name="bytes">The byte array to validate.</param>
    /// <param name="paramName">The parameter name for exception messages.</param>
    /// <param name="minimumSize">Optional minimum size requirement.</param>
    /// <exception cref="ArgumentNullException">Thrown when bytes is null.</exception>
    /// <exception cref="ArgumentException">Thrown when bytes doesn't meet minimum size.</exception>
    public static void ValidateByteArray(byte[]? bytes, string paramName, int minimumSize = 0)
    {
        if (bytes == null)
        {
            throw new ArgumentNullException(paramName, "Byte array cannot be null");
        }

        if (minimumSize > 0 && bytes.Length < minimumSize)
        {
            throw new ArgumentException($"Byte array must be at least {minimumSize} bytes", paramName);
        }
    }

    /// <summary>
    /// Validates that a file exists and optionally checks extension.
    /// </summary>
    /// <param name="filePath">The file path to validate.</param>
    /// <param name="paramName">The parameter name for exception messages.</param>
    /// <param name="requiredExtension">Optional required file extension (e.g., ".exe").</param>
    /// <exception cref="ArgumentNullException">Thrown when filePath is null or empty.</exception>
    /// <exception cref="FileNotFoundException">Thrown when file doesn't exist.</exception>
    /// <exception cref="ArgumentException">Thrown when file extension doesn't match.</exception>
    public static void ValidateFile(string? filePath, string paramName, string? requiredExtension = null)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(paramName, "File path cannot be null or empty");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}", filePath);
        }

        if (!string.IsNullOrEmpty(requiredExtension))
        {
            var extension = Path.GetExtension(filePath);
            if (!extension.Equals(requiredExtension, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    $"File must have {requiredExtension} extension, got {extension}",
                    paramName);
            }
        }
    }

    /// <summary>
    /// Validates that a string is not null or empty.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">The parameter name for exception messages.</param>
    /// <exception cref="ArgumentNullException">Thrown when value is null or empty.</exception>
    public static void ValidateString(string? value, string paramName)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(paramName, "Value cannot be null or empty");
        }
    }

    /// <summary>
    /// Validates that a dictionary contains required keys.
    /// </summary>
    /// <param name="dictionary">The dictionary to validate.</param>
    /// <param name="requiredKeys">The keys that must be present.</param>
    /// <param name="paramName">The parameter name for exception messages.</param>
    /// <exception cref="ArgumentNullException">Thrown when dictionary is null.</exception>
    /// <exception cref="ArgumentException">Thrown when required keys are missing.</exception>
    public static void ValidateDictionary(
        Dictionary<string, string>? dictionary,
        string[] requiredKeys,
        string paramName)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException(paramName, "Dictionary cannot be null");
        }

        var missingKeys = requiredKeys.Where(key => !dictionary.ContainsKey(key)).ToArray();

        if (missingKeys.Any())
        {
            throw new ArgumentException(
                $"Dictionary is missing required keys: {string.Join(", ", missingKeys)}",
                paramName);
        }
    }
}
