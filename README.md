# METL - Malware Embedding and Testing Library

[![.NET 10](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

METL is a powerful .NET library for security research, malware analysis, and penetration testing. It provides tools for embedding payloads, compiling code dynamically, and testing malware detection capabilities.

> **⚠️ Warning:** This tool is intended for authorized security research, penetration testing, and educational purposes only. Misuse of this software for malicious purposes is illegal and unethical.

## Features

### 🔧 Core Capabilities

- **Byte Array Manipulation**: Append and merge binary data efficiently
- **Dynamic Code Compilation**: Compile C# code at runtime into executables
- **Template System**: Built-in templates for PE32, PE64, ELF, and Mach-O formats
- **Mail Merge System**: Replace placeholders in code with dynamic content
- **Async Support**: Full async/await support for all operations

### 🎯 Injection Merges

- **MalwareEmbedder**: Embed binary payloads as base64-encoded strings
- **Timestamp**: Insert current timestamp
- **EncryptionMerge**: AES encryption
- **ObfuscationMerge**: XOR obfuscation
- **UniqueIdMerge**: GUID generation
- **ConfigurationMerge**: Key-value configuration

## Quick Start

```csharp
using METL;

// Append bytes
var result = METLAppender.AppendBytesFromBytes(source, embed);

// Inject code with template
var executable = METLInjector.InjectFromBuiltInTemplate(
    BuiltInTemplates.PE32, 
    new Dictionary<string, string> { { "MALWARE_EMBED", "payload.bin" } });
```

## License

MIT License - see LICENSE file for details.
