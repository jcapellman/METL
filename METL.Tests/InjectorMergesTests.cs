using Microsoft.VisualStudio.TestTools.UnitTesting;
using METL.InjectorMerges;
using METL.InjectorMerges.Base;

namespace METL.Tests;

[TestClass]
public class InjectorMergesTests
{
    [TestMethod]
    public void MalwareEmbedder_WithValidFile_ReturnsBase64()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var testBytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        File.WriteAllBytes(tempFile, testBytes);

        try
        {
            var merger = new MalwareEmbedder();

            // Act
            var result = merger.Merge(tempFile);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("malSource"));
            Assert.IsTrue(result.Contains(Convert.ToBase64String(testBytes)));
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
    public void MalwareEmbedder_WithNullArgument_ReturnsEmptyString()
    {
        // Arrange
        var merger = new MalwareEmbedder();

        // Act
        var result = merger.Merge(null);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("malSource"));
        Assert.IsTrue(result.Contains("string.Empty"));
    }

    [TestMethod]
    public void MalwareEmbedder_WithEmptyArgument_ReturnsEmptyString()
    {
        // Arrange
        var merger = new MalwareEmbedder();

        // Act
        var result = merger.Merge(string.Empty);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("malSource"));
        Assert.IsTrue(result.Contains("string.Empty"));
    }

    [TestMethod]
    public void MalwareEmbedder_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var merger = new MalwareEmbedder();
        var nonExistentFile = Path.GetRandomFileName();

        // Act & Assert
        try
        {
            merger.Merge(nonExistentFile);
            Assert.Fail("Expected FileNotFoundException was not thrown");
        }
        catch (FileNotFoundException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void MalwareEmbedder_FieldName_IsCorrect()
    {
        // Arrange
        var merger = new MalwareEmbedder();

        // Act & Assert
        Assert.AreEqual("MALWARE_EMBED", merger.FIELD_NAME);
    }

    [TestMethod]
    public void Timestamp_Merge_ReturnsTimestamp()
    {
        // Arrange
        var merger = new Timestamp();
        var beforeTime = DateTime.Now.AddSeconds(-1);

        // Act
        var result = merger.Merge();
        var timestamp = DateTime.Parse(result!);
        var afterTime = DateTime.Now.AddSeconds(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(timestamp >= beforeTime && timestamp <= afterTime);
    }

    [TestMethod]
    public void Timestamp_FieldName_IsCorrect()
    {
        // Arrange
        var merger = new Timestamp();

        // Act & Assert
        Assert.AreEqual("TIMESTAMP", merger.FIELD_NAME);
    }

    [TestMethod]
    public void Timestamp_WithArgument_IgnoresArgumentAndReturnsTimestamp()
    {
        // Arrange
        var merger = new Timestamp();

        // Act
        var result = merger.Merge("ignored_argument");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(DateTime.TryParse(result, out _));
    }

    [TestMethod]
    public void AllMergers_InheritFromBaseInjectorMerge()
    {
        // Arrange & Act
        var malwareEmbedder = new MalwareEmbedder();
        var timestamp = new Timestamp();

        // Assert
        Assert.IsInstanceOfType(malwareEmbedder, typeof(BaseInjectorMerge));
        Assert.IsInstanceOfType(timestamp, typeof(BaseInjectorMerge));
    }
}
