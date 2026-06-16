using System.Diagnostics.CodeAnalysis;
using METL.Diagnostics;

namespace METL;

/// <summary>
/// Provides methods for appending bytes to existing byte arrays.
/// </summary>
public static class METLAppender
{
    /// <summary>
    /// Appends bytes from a file to the source byte array.
    /// </summary>
    /// <param name="source">The source byte array.</param>
    /// <param name="embedFileName">The path to the file containing bytes to append.</param>
    /// <returns>A new byte array containing the source bytes followed by the embed bytes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when source is null or embedFileName is null or empty.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the embed file does not exist.</exception>
    public static byte[] AppendBytesFromFile([NotNull] byte[] source, string embedFileName)
    {
        METLLogger.Debug($"AppendBytesFromFile called - Source size: {source?.Length ?? 0} bytes, File: {embedFileName}");

        if (source == null)
        {
            METLLogger.Error("Source byte array is null");
            throw new ArgumentNullException(nameof(source));
        }

        if (string.IsNullOrEmpty(embedFileName))
        {
            METLLogger.Error("Embed filename is null or empty");
            throw new ArgumentNullException(nameof(embedFileName));
        }

        if (!File.Exists(embedFileName))
        {
            METLLogger.Error($"Embed file not found: {embedFileName}");
            throw new FileNotFoundException($"Embed file not found: {embedFileName}", embedFileName);
        }

        var embedBytes = File.ReadAllBytes(embedFileName);
        METLLogger.Info($"Successfully loaded embed file: {embedFileName} ({embedBytes.Length} bytes)");

        return AppendBytesFromBytes(source, embedBytes);
    }

    /// <summary>
    /// Asynchronously appends bytes from a file to the source byte array.
    /// </summary>
    /// <param name="source">The source byte array.</param>
    /// <param name="embedFileName">The path to the file containing bytes to append.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing a new byte array with appended bytes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when source is null or embedFileName is null or empty.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the embed file does not exist.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public static async Task<byte[]> AppendBytesFromFileAsync(
        [NotNull] byte[] source,
        string embedFileName,
        CancellationToken cancellationToken = default)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (string.IsNullOrEmpty(embedFileName))
        {
            throw new ArgumentNullException(nameof(embedFileName));
        }

        if (!File.Exists(embedFileName))
        {
            throw new FileNotFoundException($"Embed file not found: {embedFileName}", embedFileName);
        }

        var embedBytes = await File.ReadAllBytesAsync(embedFileName, cancellationToken).ConfigureAwait(false);

        return await AppendBytesFromBytesAsync(source, embedBytes, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Appends embed bytes to the source byte array.
    /// </summary>
    /// <param name="source">The source byte array.</param>
    /// <param name="embedBytes">The bytes to append.</param>
    /// <returns>A new byte array containing the source bytes followed by the embed bytes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when source or embedBytes is null.</exception>
    public static byte[] AppendBytesFromBytes([NotNull] byte[] source, [NotNull] byte[] embedBytes)
    {
        METLLogger.Debug($"AppendBytesFromBytes called - Source: {source?.Length ?? 0} bytes, Embed: {embedBytes?.Length ?? 0} bytes");

        if (source == null)
        {
            METLLogger.Error("Source byte array is null");
            throw new ArgumentNullException(nameof(source));
        }

        if (embedBytes == null)
        {
            METLLogger.Error("Embed byte array is null");
            throw new ArgumentNullException(nameof(embedBytes));
        }

        var result = new byte[source.Length + embedBytes.Length];
        Buffer.BlockCopy(source, 0, result, 0, source.Length);
        Buffer.BlockCopy(embedBytes, 0, result, source.Length, embedBytes.Length);

        METLLogger.Info($"Successfully appended bytes - Total size: {result.Length} bytes");

        return result;
    }

    /// <summary>
    /// Asynchronously appends embed bytes to the source byte array.
    /// </summary>
    /// <param name="source">The source byte array.</param>
    /// <param name="embedBytes">The bytes to append.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing a new byte array with appended bytes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when source or embedBytes is null.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public static Task<byte[]> AppendBytesFromBytesAsync(
        [NotNull] byte[] source,
        [NotNull] byte[] embedBytes,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (embedBytes == null)
        {
            throw new ArgumentNullException(nameof(embedBytes));
        }

        return Task.FromResult(AppendBytesFromBytes(source, embedBytes));
    }
}
