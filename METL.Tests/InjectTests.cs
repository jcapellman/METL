using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace METL.Tests;

[TestClass]
public class InjectTests
{
    [TestMethod]
    public void NullCode_ThrowsArgumentNullException()
    {
        // Act & Assert
        try
        {
            METLInjector.InjectFromFile(sourceFileName: null!, arguments: null);
            Assert.Fail("Expected ArgumentNullException was not thrown");
        }
        catch (ArgumentNullException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void CodeNotFound_ThrowsFileNotFoundException()
    {
        // Arrange
        var nonExistentFile = Path.GetRandomFileName();

        // Act & Assert
        try
        {
            METLInjector.InjectFromFile(nonExistentFile);
            Assert.Fail("Expected FileNotFoundException was not thrown");
        }
        catch (FileNotFoundException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void TemplateNotFound_ThrowsArgumentOutOfRangeException()
    {
        // Act & Assert
        try
        {
#pragma warning disable CS0618 // Type or member is obsolete
            METLInjector.InjectMalwareFromTemplate("WICK", Path.GetRandomFileName());
#pragma warning restore CS0618
            Assert.Fail("Expected ArgumentException was not thrown");
        }
        catch (ArgumentException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void TemplatePE32_ReturnsBytes()
    {
        // Arrange
        var malFile = Path.Combine(AppContext.BaseDirectory, "Samples/sourcePE");

        // Act
        var arguments = new Dictionary<string, string>
        {
            { "MALWARE_EMBED", malFile }
        };
        var result = METLInjector.InjectFromBuiltInTemplate(Enums.BuiltInTemplates.PE32, arguments);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
    }

    [TestMethod]
    public void PE32BadCode_ThrowsException()
    {
        // Arrange
        var malFile = Path.Combine(AppContext.BaseDirectory, "Samples/sourcePE");
        var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");

        // Act & Assert - Bad code should throw during compilation
        try
        {
            METLInjector.InjectFromFile(sourceFile, new Dictionary<string, string> { { "MALWARE_EMBED", malFile } });
            Assert.Fail("Expected InvalidOperationException was not thrown");
        }
        catch (InvalidOperationException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void PE32BadMalwareFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var malFile = Path.Combine(AppContext.BaseDirectory, DateTime.Now.ToString(CultureInfo.InvariantCulture));
        var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32.cs");

        // Act & Assert
        try
        {
            METLInjector.InjectFromFile(sourceFile, new Dictionary<string, string> { { "MALWARE_EMBED", malFile } });
            Assert.Fail("Expected FileNotFoundException was not thrown");
        }
        catch (FileNotFoundException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void PE32NullMalwareFile_ThrowsArgumentNullException()
    {
        // Arrange
        var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32.cs");

        // Act & Assert
        try
        {
            METLInjector.InjectFromFile(sourceFile, new Dictionary<string, string> { { "MALWARE_EMBED", "" } });
            Assert.Fail("Expected FileNotFoundException was not thrown");
        }
        catch (FileNotFoundException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void PEInjector_ProducesValidExecutable()
    {
        // Arrange
        var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32.cs");
        var malFile = Path.Combine(AppContext.BaseDirectory, "Samples/sourcePE");

        // Act
        var arguments = new Dictionary<string, string>
        {
            { "MALWARE_EMBED", malFile }
        };
        var injectedBytes = METLInjector.InjectFromFile(sourceFile, arguments);

        // Assert
        Assert.IsNotNull(injectedBytes);
        Assert.IsTrue(injectedBytes.Length > 0);

        // Write output for manual inspection
        var outputPath = Path.Combine(AppContext.BaseDirectory, "injected.exe");
        File.WriteAllBytes(outputPath, injectedBytes);
        Assert.IsTrue(File.Exists(outputPath));
    }

    [TestMethod]
    public async Task InjectFromBuiltInTemplateAsync_WithValidInput_ReturnsBytes()
    {
        // Arrange
        var malFile = Path.Combine(AppContext.BaseDirectory, "Samples/sourcePE");
        var arguments = new Dictionary<string, string>
        {
            { "MALWARE_EMBED", malFile }
        };

        // Act
        var result = await METLInjector.InjectFromBuiltInTemplateAsync(Enums.BuiltInTemplates.PE32, arguments);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
    }

    [TestMethod]
    public async Task InjectFromFileAsync_WithValidInput_ReturnsBytes()
    {
        // Arrange
        var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32.cs");
        var malFile = Path.Combine(AppContext.BaseDirectory, "Samples/sourcePE");
        var arguments = new Dictionary<string, string>
        {
            { "MALWARE_EMBED", malFile }
        };

        // Act
        var result = await METLInjector.InjectFromFileAsync(sourceFile, arguments);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
    }

    [TestMethod]
    public async Task InjectFromStringAsync_WithValidInput_ReturnsBytes()
    {
        // Arrange
        var sourceCode = @"
using System;

namespace TestPayload
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Test payload"");
        }
    }
}";

        // Act
        var result = await METLInjector.InjectFromStringAsync(sourceCode);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
    }
}
