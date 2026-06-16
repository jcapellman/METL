using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace METL.Tests;

[TestClass]
public class AppendTests
{
    [TestMethod]
    public void FileNotFound_ThrowsFileNotFoundException()
    {
        // Arrange
        var nonExistentFile = Path.GetRandomFileName();
        var sourceBytes = new byte[] { 0x01, 0x02 };

        // Act & Assert
        try
        {
            METLAppender.AppendBytesFromFile(sourceBytes, nonExistentFile);
            Assert.Fail("Expected FileNotFoundException was not thrown");
        }
        catch (FileNotFoundException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void NullSourceBytes_ThrowsArgumentNullException()
    {
        // Act & Assert
        try
        {
            METLAppender.AppendBytesFromBytes(null!, null!);
            Assert.Fail("Expected ArgumentNullException was not thrown");
        }
        catch (ArgumentNullException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void ValidSourceBytes_ReturnsAppendedBytes()
    {
        // Arrange
        var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");
        var destFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");

        // Act
        var result = METLAppender.AppendBytesFromBytes(
            File.ReadAllBytes(sourceFile),
            File.ReadAllBytes(destFile));

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
    }

    [TestMethod]
    public void ValidSourceFiles_ReturnsAppendedBytes()
    {
        // Arrange
        var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");
        var destFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");

        // Act
        var result = METLAppender.AppendBytesFromFile(
            File.ReadAllBytes(sourceFile),
            destFile);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
    }

    [TestMethod]
    public void NullEmbedFileName_ThrowsArgumentNullException()
    {
        // Act & Assert
        try
        {
            METLAppender.AppendBytesFromFile(null!, null!);
            Assert.Fail("Expected ArgumentNullException was not thrown");
        }
        catch (ArgumentNullException)
        {
            // Expected
        }
    }

    [TestMethod]
    public async Task AppendBytesFromFileAsync_WithValidInput_ReturnsResult()
    {
        // Arrange
        var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");
        var destFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");
        var sourceBytes = await File.ReadAllBytesAsync(sourceFile);

        // Act
        var result = await METLAppender.AppendBytesFromFileAsync(sourceBytes, destFile);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > sourceBytes.Length);
    }

    [TestMethod]
    public async Task AppendBytesFromBytesAsync_WithValidInput_ReturnsResult()
    {
        // Arrange
        var sourceBytes = new byte[] { 0x01, 0x02, 0x03 };
        var embedBytes = new byte[] { 0x04, 0x05 };

        // Act
        var result = await METLAppender.AppendBytesFromBytesAsync(sourceBytes, embedBytes);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Length);
        Assert.AreEqual(0x01, result[0]);
        Assert.AreEqual(0x04, result[3]);
    }
}
