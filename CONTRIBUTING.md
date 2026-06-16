# Contributing to METL

First off, thank you for considering contributing to METL! It's people like you that make METL a great tool for malware research and security analysis.

## Code of Conduct

This project is intended for **educational and research purposes only**. By contributing, you agree to use this tool responsibly and ethically.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the existing issues to avoid duplicates. When you create a bug report, include as many details as possible:

- **Use a clear and descriptive title**
- **Describe the exact steps to reproduce the problem**
- **Provide specific examples**
- **Describe the behavior you observed and what you expected**
- **Include code samples and error messages**
- **Specify your environment** (.NET version, OS, etc.)

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion:

- **Use a clear and descriptive title**
- **Provide a detailed description of the suggested enhancement**
- **Explain why this enhancement would be useful**
- **List any alternatives you've considered**

### Pull Requests

1. **Fork the repository** and create your branch from `main`
2. **Follow the coding standards** defined in `.editorconfig`
3. **Add tests** for any new functionality
4. **Ensure all tests pass** by running `dotnet test`
5. **Update documentation** if needed
6. **Write clear commit messages**

## Development Setup

### Prerequisites

- .NET 10 SDK
- Visual Studio 2026 or VS Code (recommended)
- Git

### Getting Started

```bash
# Clone your fork
git clone https://github.com/YOUR-USERNAME/METL.git
cd METL

# Add upstream remote
git remote add upstream https://github.com/jcapellman/METL.git

# Create a feature branch
git checkout -b feature/your-feature-name

# Install dependencies and build
dotnet restore
dotnet build

# Run tests
dotnet test
```

## Coding Standards

### General Guidelines

- Follow the C# coding conventions defined in `.editorconfig`
- Use meaningful variable and method names
- Keep methods focused and single-purpose
- Add XML documentation comments for public APIs
- Use async/await for I/O operations
- Support cancellation tokens for long-running operations

### Code Style

```csharp
// Good: Clear, descriptive naming
public async Task<byte[]> CompileSourceCodeAsync(
    string sourceCode, 
    CancellationToken cancellationToken = default)
{
    if (string.IsNullOrEmpty(sourceCode))
    {
        throw new ArgumentNullException(nameof(sourceCode));
    }

    // Implementation
}

// Bad: Unclear naming, no validation
public byte[] Compile(string s)
{
    // Implementation
}
```

### Testing

- Write unit tests for all new functionality
- Aim for high code coverage (>80%)
- Use descriptive test names that explain the scenario
- Follow AAA pattern (Arrange, Act, Assert)

```csharp
[TestMethod]
public async Task AppendBytesFromFileAsync_WithValidInput_ReturnsCorrectResult()
{
    // Arrange
    var sourceBytes = new byte[] { 0x01, 0x02 };
    var embedFile = "test-embed.bin";

    // Act
    var result = await METLAppender.AppendBytesFromFileAsync(sourceBytes, embedFile);

    // Assert
    Assert.IsNotNull(result);
    Assert.IsTrue(result.Length > sourceBytes.Length);
}
```

### Commit Messages

- Use the present tense ("Add feature" not "Added feature")
- Use the imperative mood ("Move cursor to..." not "Moves cursor to...")
- Limit the first line to 72 characters
- Reference issues and pull requests when relevant

```
Add async support for byte appending operations

- Implement AppendBytesFromFileAsync method
- Add cancellation token support
- Update tests for async operations

Fixes #123
```

## Project Structure

```
METL/
├── METL/                      # Main library
│   ├── Enums/                 # Enumerations
│   ├── Helpers/               # Helper classes
│   ├── InjectorMerges/        # Merge strategies
│   ├── IncludedScripts/       # Built-in templates
│   ├── METLAppender.cs        # Byte appending functionality
│   └── METLInjector.cs        # Code injection functionality
├── METL.Tests/                # Unit and integration tests
│   ├── AppendTests.cs
│   └── InjectTests.cs
└── README.md
```

## Adding New Features

### Adding a New Built-in Template

1. Create the template C# file in `METL/IncludedScripts/`
2. Add the template to `BuiltInTemplates` enum
3. Update the template loader in `METLInjector`
4. Add tests for the new template
5. Update documentation

### Adding New Merge Strategies

1. Create a new class in `METL/InjectorMerges/`
2. Inherit from `BaseInjectorMerge`
3. Implement the required merge logic
4. Add unit tests
5. Document the merge strategy

## Review Process

1. All pull requests require at least one review
2. All tests must pass
3. Code coverage should not decrease
4. Documentation must be updated
5. Follow semantic versioning for version bumps

## Community

- Be respectful and constructive
- Help others learn and grow
- Focus on malware research and security education
- Report any misuse or unethical behavior

## Questions?

Feel free to open an issue with the `question` label if you need clarification on anything.

## License

By contributing to METL, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to METL! 🛡️
