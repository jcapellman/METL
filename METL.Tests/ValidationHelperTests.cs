using Microsoft.VisualStudio.TestTools.UnitTesting;
using METL.Helpers;

namespace METL.Tests;

[TestClass]
public class ValidationHelperTests
{
    [TestMethod]
    public void ValidateByteArray_WithNullArray_ThrowsArgumentNullException()
    {
        // Act & Assert
        try
        {
            ValidationHelper.ValidateByteArray(null, "testParam");
            Assert.Fail("Expected ArgumentNullException was not thrown");
        }
        catch (ArgumentNullException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void ValidateByteArray_WithValidArray_DoesNotThrow()
    {
        // Arrange
        var bytes = new byte[] { 0x01, 0x02 };

        // Act & Assert (no exception should be thrown)
        ValidationHelper.ValidateByteArray(bytes, "testParam");
    }

    [TestMethod]
    public void ValidateByteArray_WithMinimumSize_EnforcesMinimum()
    {
        // Arrange
        var bytes = new byte[] { 0x01, 0x02 };

        // Act & Assert
        try
        {
            ValidationHelper.ValidateByteArray(bytes, "testParam", minimumSize: 5);
            Assert.Fail("Expected ArgumentException was not thrown");
        }
        catch (ArgumentException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void ValidateFile_WithNullPath_ThrowsArgumentNullException()
    {
        // Act & Assert
        try
        {
            ValidationHelper.ValidateFile(null, "testParam");
            Assert.Fail("Expected ArgumentNullException was not thrown");
        }
        catch (ArgumentNullException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void ValidateFile_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var nonExistentFile = Path.GetRandomFileName();

        // Act & Assert
        try
        {
            ValidationHelper.ValidateFile(nonExistentFile, "testParam");
            Assert.Fail("Expected FileNotFoundException was not thrown");
        }
        catch (FileNotFoundException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void ValidateFile_WithValidFile_DoesNotThrow()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act & Assert (no exception should be thrown)
            ValidationHelper.ValidateFile(tempFile, "testParam");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [TestMethod]
    public void ValidateFile_WithWrongExtension_ThrowsArgumentException()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act & Assert
            try
            {
                ValidationHelper.ValidateFile(tempFile, "testParam", ".exe");
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [TestMethod]
    public void ValidateString_WithNullString_ThrowsArgumentNullException()
    {
        // Act & Assert
        try
        {
            ValidationHelper.ValidateString(null, "testParam");
            Assert.Fail("Expected ArgumentNullException was not thrown");
        }
        catch (ArgumentNullException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void ValidateString_WithEmptyString_ThrowsArgumentNullException()
    {
        // Act & Assert
        try
        {
            ValidationHelper.ValidateString(string.Empty, "testParam");
            Assert.Fail("Expected ArgumentNullException was not thrown");
        }
        catch (ArgumentNullException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void ValidateString_WithValidString_DoesNotThrow()
    {
        // Act & Assert (no exception should be thrown)
        ValidationHelper.ValidateString("valid", "testParam");
    }

    [TestMethod]
    public void ValidateDictionary_WithNullDictionary_ThrowsArgumentNullException()
    {
        // Arrange
        var requiredKeys = new[] { "key1" };

        // Act & Assert
        try
        {
            ValidationHelper.ValidateDictionary(null, requiredKeys, "testParam");
            Assert.Fail("Expected ArgumentNullException was not thrown");
        }
        catch (ArgumentNullException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void ValidateDictionary_WithMissingKeys_ThrowsArgumentException()
    {
        // Arrange
        var dict = new Dictionary<string, string> { { "key1", "value1" } };
        var requiredKeys = new[] { "key1", "key2", "key3" };

        // Act & Assert
        try
        {
            ValidationHelper.ValidateDictionary(dict, requiredKeys, "testParam");
            Assert.Fail("Expected ArgumentException was not thrown");
        }
        catch (ArgumentException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void ValidateDictionary_WithAllRequiredKeys_DoesNotThrow()
    {
        // Arrange
        var dict = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };
        var requiredKeys = new[] { "key1", "key2" };

        // Act & Assert (no exception should be thrown)
        ValidationHelper.ValidateDictionary(dict, requiredKeys, "testParam");
    }
}
