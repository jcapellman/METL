using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace METL.Tests;

[TestClass]
public class EdgeCaseTests
{
    [TestMethod]
    public void AppendBytes_WithEmptySourceArray_ReturnsEmbedBytes()
    {
        // Arrange
        var source = Array.Empty<byte>();
        var embed = new byte[] { 0x01, 0x02, 0x03 };

        // Act
        var result = METLAppender.AppendBytesFromBytes(source, embed);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Length);
        CollectionAssert.AreEqual(embed, result);
    }

    [TestMethod]
    public void AppendBytes_WithEmptyEmbedArray_ReturnsSourceBytes()
    {
        // Arrange
        var source = new byte[] { 0x01, 0x02, 0x03 };
        var embed = Array.Empty<byte>();

        // Act
        var result = METLAppender.AppendBytesFromBytes(source, embed);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Length);
        CollectionAssert.AreEqual(source, result);
    }

    [TestMethod]
    public void AppendBytes_WithBothEmpty_ReturnsEmptyArray()
    {
        // Arrange
        var source = Array.Empty<byte>();
        var embed = Array.Empty<byte>();

        // Act
        var result = METLAppender.AppendBytesFromBytes(source, embed);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Length);
    }

    [TestMethod]
    public void AppendBytes_WithLargeArrays_SuccessfullyAppends()
    {
        // Arrange
        var source = new byte[10_000];
        var embed = new byte[5_000];
        new Random(42).NextBytes(source);
        new Random(84).NextBytes(embed);

        // Act
        var result = METLAppender.AppendBytesFromBytes(source, embed);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(15_000, result.Length);

        // Verify source bytes are intact
        for (int i = 0; i < source.Length; i++)
        {
            Assert.AreEqual(source[i], result[i], $"Source byte mismatch at index {i}");
        }

        // Verify embed bytes are intact
        for (int i = 0; i < embed.Length; i++)
        {
            Assert.AreEqual(embed[i], result[source.Length + i], $"Embed byte mismatch at index {i}");
        }
    }

    [TestMethod]
    public async Task AppendBytesAsync_WithCancellation_ThrowsOperationCanceledException()
    {
        // Arrange
        var source = new byte[] { 0x01, 0x02 };
        var embed = new byte[] { 0x03, 0x04 };
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        try
        {
            await METLAppender.AppendBytesFromBytesAsync(source, embed, cts.Token);
            Assert.Fail("Expected OperationCanceledException was not thrown");
        }
        catch (OperationCanceledException)
        {
            // Expected
        }
    }

    [TestMethod]
    public async Task AppendBytesFromFileAsync_WithCancellation_ThrowsOperationCanceledException()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        File.WriteAllBytes(tempFile, new byte[] { 0x01, 0x02 });

        try
        {
            var source = new byte[] { 0x03, 0x04 };
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            try
            {
                await METLAppender.AppendBytesFromFileAsync(source, tempFile, cts.Token);
                Assert.Fail("Expected OperationCanceledException was not thrown");
            }
            catch (OperationCanceledException)
            {
                // Expected
            }
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    [TestMethod]
    public void InjectFromString_WithEmptyString_ThrowsArgumentNullException()
    {
        // Act & Assert
        try
        {
            METLInjector.InjectFromString(string.Empty);
            Assert.Fail("Expected ArgumentNullException was not thrown");
        }
        catch (ArgumentNullException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void InjectFromFile_WithEmptyFilePath_ThrowsArgumentNullException()
    {
        // Act & Assert
        try
        {
            METLInjector.InjectFromFile(string.Empty);
            Assert.Fail("Expected ArgumentNullException was not thrown");
        }
        catch (ArgumentNullException)
        {
            // Expected
        }
    }

    [TestMethod]
    public async Task InjectFromStringAsync_WithCancellation_ThrowsOperationCanceledException()
    {
        // Arrange
        var sourceCode = "Console.WriteLine(\"Test\");";
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        try
        {
            await METLInjector.InjectFromStringAsync(sourceCode, null, cts.Token);
            Assert.Fail("Expected OperationCanceledException was not thrown");
        }
        catch (OperationCanceledException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void InjectFromBuiltInTemplate_WithInvalidTemplate_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var invalidTemplate = (Enums.BuiltInTemplates)999;

        // Act & Assert
        try
        {
            METLInjector.InjectFromBuiltInTemplate(invalidTemplate);
            Assert.Fail("Expected ArgumentOutOfRangeException was not thrown");
        }
        catch (ArgumentOutOfRangeException)
        {
            // Expected
        }
    }
}
