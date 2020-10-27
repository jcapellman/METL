# MET&L
Malware Embedding Tool & Library

## About
The purpose of this library is to provide a quick and easy way to test various methods of malware injection.

## Usage
### Appending
Similar to what was performed by Malware Researchers back in 2019 appending benign bytes to a malicious file to circumvent both signature and next generation endpoint protection products.

Two methods are available:

#### Append from a file path
<code>
  AppendBytesFromFile([NotNull]byte[] source, string embedFileName)
</code>

#### Append from bytes
<code>
   AppendBytesFromBytes([NotNull]byte[] source, [NotNull]byte[] embedBytes)
</code>

### Injection
A more advanced method is to convert the bytes to a base64 encoded string and then decrypt the string on execution.

Four methods are available:

#### Inject with a template (PE32 is the only template as of this writing) and malicious file
<code>
  InjectMalwareFromTemplate(BuiltInTemplates template, string malwareFileName)
</code>

#### Inject from a template file and malicious file
<code>
InjectMalwareFromTemplate(string templateName, string malwareFileName)
</code>

#### Inject from a source file and malicious file
<code>
InjectMalwareFromFile(string sourceFileName, string malwareFileName)
</code>

#### Inject from a source file and arguments
<code>
InjectMalwareFromFile(string sourceFileName, Dictionary<string, string> arguments)
</code>
