# METL - Malware Executable Transformation Library

[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/METL.svg)](https://www.nuget.org/packages/METL/)
[![License](https://img.shields.io/github/license/jcapellman/METL.svg)](LICENSE)

METL is a .NET library for binary injection and byte appending, designed for malware research and information security analysis.

## ⚠️ Disclaimer

This tool is intended **exclusively for malware research, security analysis, and educational purposes**. Users are responsible for ensuring compliance with all applicable laws and regulations. The authors assume no liability for misuse.

## About
The purpose of this library is to provide a quick and easy way to test various methods of malware injection and embedding, similar to techniques observed by malware researchers for circumventing signature-based and next-generation endpoint protection products.

## Installation

### NuGet Package Manager

```bash
dotnet add package METL
```

### Package Manager Console

```powershell
Install-Package METL
```

## Usage
### Appending
Appending benign bytes to a file to circumvent both signature and next generation endpoint protection products.

Two methods are available (both with async versions):

#### Append from a file path
```csharp
// Synchronous
byte[] result = METLAppender.AppendBytesFromFile(byte[] source, string embedFileName);

// Asynchronous
byte[] result = await METLAppender.AppendBytesFromFileAsync(byte[] source, string embedFileName, CancellationToken cancellationToken = default);
```

#### Append from bytes
```csharp
// Synchronous
byte[] result = METLAppender.AppendBytesFromBytes(byte[] source, byte[] embedBytes);

// Asynchronous
byte[] result = await METLAppender.AppendBytesFromBytesAsync(byte[] source, byte[] embedBytes, CancellationToken cancellationToken = default);
```

### Injection
A more advanced method using templates with mail merge support for dynamic code generation.

Multiple methods are available (all with async versions):

#### Inject with a built-in template
```csharp
// Synchronous
byte[] result = METLInjector.InjectFromBuiltInTemplate(BuiltInTemplates.PE32, Dictionary<string, string> arguments);

// Asynchronous
byte[] result = await METLInjector.InjectFromBuiltInTemplateAsync(BuiltInTemplates.PE32, Dictionary<string, string> arguments, CancellationToken cancellationToken = default);
```

#### Inject from a source file with arguments (mail merge)
```csharp
// Synchronous
byte[] result = METLInjector.InjectFromFile(string sourceFileName, Dictionary<string, string> arguments);

// Asynchronous
byte[] result = await METLInjector.InjectFromFileAsync(string sourceFileName, Dictionary<string, string> arguments, CancellationToken cancellationToken = default);
```

#### Inject from string with arguments
```csharp
// Synchronous
byte[] result = METLInjector.InjectFromString(string sourceCode, Dictionary<string, string> arguments);

// Asynchronous
byte[] result = await METLInjector.InjectFromStringAsync(string sourceCode, Dictionary<string, string> arguments, CancellationToken cancellationToken = default);
```

## Features

- **Async/Await Support**: All operations support asynchronous execution
- **Cancellation Tokens**: Long-running operations can be cancelled
- **Built-in Templates**: Pre-configured templates for PE32 executables (more coming soon)
- **Mail Merge**: Dynamic argument replacement in templates
- **.NET CLI Integration**: Leverages the .NET CLI for code compilation
- **Modern .NET**: Built with .NET 10 and modern C# features

## Building from Source

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2026 or later (optional)

### Build Steps
```bash
git clone https://github.com/jcapellman/METL.git
cd METL
dotnet build
dotnet test
```

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

- **Author**: Jarred Capellman
- **GitHub**: [@jcapellman](https://github.com/jcapellman)
- **Project**: [METL](https://github.com/jcapellman/METL)

---

**Remember**: Always use responsibly and ethically. This tool is for educational and research purposes only.
