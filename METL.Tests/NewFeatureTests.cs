using Microsoft.VisualStudio.TestTools.UnitTesting;
using METL.InjectorMerges;

namespace METL.Tests;

[TestClass]
public class NewFeatureTests
{
    [TestMethod]
    public void EncryptionMerge_WithValidString_ReturnsEncryptedCode()
    {
        // Arrange
        var merger = new EncryptionMerge();
        var testData = "SecretData123";

        // Act
        var result = merger.Merge(testData);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("encryptedData"));
        Assert.IsTrue(result.Contains("encryptionKey"));
        Assert.IsTrue(result.Contains("encryptionIV"));
        Assert.IsTrue(result.Contains("Convert.FromBase64String"));
    }

    [TestMethod]
    public void EncryptionMerge_WithNullArgument_ReturnsComment()
    {
        // Arrange
        var merger = new EncryptionMerge();

        // Act
        var result = merger.Merge(null);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("No encryption data provided"));
    }

    [TestMethod]
    public void ObfuscationMerge_WithValidString_ReturnsObfuscatedCode()
    {
        // Arrange
        var merger = new ObfuscationMerge();
        var testData = "TestString";

        // Act
        var result = merger.Merge(testData);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("obfuscatedBytes"));
        Assert.IsTrue(result.Contains("obfuscationKey"));
        Assert.IsTrue(result.Contains("deobfuscatedBytes"));
    }

    [TestMethod]
    public void ObfuscationMerge_WithEmptyString_ReturnsDefault()
    {
        // Arrange
        var merger = new ObfuscationMerge();

        // Act
        var result = merger.Merge(string.Empty);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("obfuscatedValue = string.Empty"));
    }

    [TestMethod]
    public void UniqueIdMerge_WithNoArgument_GeneratesGuid()
    {
        // Arrange
        var merger = new UniqueIdMerge();

        // Act
        var result = merger.Merge(null);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("uniqueId"));
        Assert.IsTrue(Guid.TryParse(
            result.Split('"')[1],
            out _));
    }

    [TestMethod]
    public void UniqueIdMerge_WithCustomGuid_UsesProvidedGuid()
    {
        // Arrange
        var merger = new UniqueIdMerge();
        var customGuid = Guid.NewGuid().ToString();

        // Act
        var result = merger.Merge(customGuid);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains(customGuid));
    }

    [TestMethod]
    public void ConfigurationMerge_WithKeyValuePairs_ReturnsConfigurationCode()
    {
        // Arrange
        var merger = new ConfigurationMerge();
        var config = "key1=value1,key2=value2,key3=value3";

        // Act
        var result = merger.Merge(config);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("configuration"));
        Assert.IsTrue(result.Contains("Dictionary<string, string>"));
        Assert.IsTrue(result.Contains("key1"));
        Assert.IsTrue(result.Contains("value1"));
        Assert.IsTrue(result.Contains("key2"));
        Assert.IsTrue(result.Contains("value2"));
    }

    [TestMethod]
    public void ConfigurationMerge_WithEmptyString_ReturnsEmptyDictionary()
    {
        // Arrange
        var merger = new ConfigurationMerge();

        // Act
        var result = merger.Merge(string.Empty);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("var configuration = new Dictionary<string, string>()"));
    }

    [TestMethod]
    public void AllNewMergers_HaveCorrectFieldNames()
    {
        // Arrange & Act
        var encryption = new EncryptionMerge();
        var obfuscation = new ObfuscationMerge();
        var uniqueId = new UniqueIdMerge();
        var config = new ConfigurationMerge();

        // Assert
        Assert.AreEqual("ENCRYPT_DATA", encryption.FIELD_NAME);
        Assert.AreEqual("OBFUSCATE_STRING", obfuscation.FIELD_NAME);
        Assert.AreEqual("UNIQUE_ID", uniqueId.FIELD_NAME);
        Assert.AreEqual("CONFIGURATION", config.FIELD_NAME);
    }
}
