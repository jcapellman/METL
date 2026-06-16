# Security Policy

## Supported Versions

Currently supported versions for security updates:

| Version | Supported          |
| ------- | ------------------ |
| 0.1.6   | :white_check_mark: |
| 0.1.5   | :x:                |
| < 0.1.5 | :x:                |

## Responsible Use

### ⚠️ Important Notice

METL is designed **exclusively** for:
- Malware research
- Security analysis and testing
- Educational purposes
- Authorized penetration testing

**This tool must NOT be used for:**
- Creating or distributing actual malware
- Unauthorized access to systems
- Circumventing security measures without permission
- Any illegal activities

## Reporting a Vulnerability

We take the security of METL seriously. If you discover a security vulnerability, please follow these guidelines:

### What to Report

- Security vulnerabilities in the library code
- Potential for misuse or abuse
- Dependency vulnerabilities
- Documentation that could facilitate malicious use

### How to Report

1. **DO NOT** open a public GitHub issue for security vulnerabilities
2. Email the maintainer directly at: [Your security email - needs to be added]
3. Provide as much detail as possible:
   - Description of the vulnerability
   - Steps to reproduce
   - Potential impact
   - Suggested remediation (if any)

### What to Expect

- **Acknowledgment**: Within 48 hours of report
- **Initial Assessment**: Within 7 days
- **Status Update**: Every 7 days until resolution
- **Resolution**: Critical issues within 30 days, others as soon as practical

### Disclosure Policy

- We practice **responsible disclosure**
- Security advisories will be published after a fix is available
- You will be credited for the discovery (unless you prefer anonymity)
- We ask for 90 days before public disclosure

## Security Best Practices for Users

### Safe Development Environment

1. **Isolated Environment**: Always test in isolated VMs or containers
2. **No Production Systems**: Never test on production or sensitive systems
3. **Network Isolation**: Use air-gapped or isolated networks
4. **Proper Disposal**: Securely wipe test environments after use

### Code Safety

```csharp
// Good: Validate and sanitize inputs
public byte[] ProcessInput(byte[] input)
{
    if (input == null || input.Length == 0)
    {
        throw new ArgumentException("Input cannot be null or empty");
    }

    // Process safely
}

// Bad: No validation
public byte[] ProcessInput(byte[] input)
{
    // Directly processing untrusted input
}
```

### Authentication and Authorization

- Always require explicit user authorization before executing operations
- Log all operations for audit purposes
- Implement rate limiting for API operations
- Use proper error handling to avoid information disclosure

### Dependency Management

- Keep all dependencies up to date
- Monitor for security advisories
- Use `dotnet list package --vulnerable` regularly
- Enable Dependabot or similar automated dependency scanning

```bash
# Check for vulnerable packages
dotnet list package --vulnerable

# Update packages
dotnet outdated
```

## Known Security Considerations

### 1. .NET CLI Execution

METL uses the .NET CLI to compile code, which:
- Executes external processes
- Requires appropriate system permissions
- Could be exploited if source code is not validated

**Mitigation**: Always validate and sanitize source code before compilation.

### 2. File System Access

Operations require file system access for:
- Reading source files
- Writing compiled binaries
- Accessing embedded resources

**Mitigation**: Implement proper path validation and use least-privilege principles.

### 3. Binary Manipulation

The library manipulates binary executables which:
- Could be detected as malware by AV/EDR
- Requires careful handling to avoid corruption
- Should only be performed in controlled environments

**Mitigation**: Use in isolated test environments only.

## Vulnerability Assessment

### Regular Security Audits

We perform regular security assessments including:
- Static code analysis using Microsoft.CodeAnalysis.NetAnalyzers
- Dependency vulnerability scanning
- Code reviews for security implications
- Penetration testing of the library itself

### Automated Security Checks

Our CI/CD pipeline includes:
- CodeQL analysis
- Dependency vulnerability scanning
- SAST (Static Application Security Testing)
- License compliance checking

## Legal Compliance

### United States

This tool may be subject to export control regulations. Users must comply with:
- Export Administration Regulations (EAR)
- International Traffic in Arms Regulations (ITAR)
- Computer Fraud and Abuse Act (CFAA)

### International

Users must comply with local laws regarding:
- Computer security research
- Cryptography
- Export controls
- Cybersecurity testing

## Incident Response

If METL is misused or involved in a security incident:

1. **Document**: Collect all relevant information
2. **Report**: Contact the maintainers immediately
3. **Cooperate**: Work with law enforcement if required
4. **Learn**: Help us improve to prevent future incidents

## Contact

For security concerns:
- **Security Issues**: [Your security email]
- **General Issues**: [GitHub Issues](https://github.com/jcapellman/METL/issues)
- **Maintainer**: Jarred Capellman

## Acknowledgments

We thank the security research community for helping keep METL secure and ethical.

### Hall of Fame

Contributors who have helped improve METL's security:
- *Your name could be here!*

---

**Remember**: With great power comes great responsibility. Use METL ethically and legally.

Last Updated: 2025
