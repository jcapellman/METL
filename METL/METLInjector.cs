﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using METL.Enums;
using METL.InjectorMerges.Base;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace METL
{
    public class METLInjector
    {
        private static byte[] CompileCodeToBytes(string sourceCode)
        {
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp9);

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(sourceCode, options);

            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(File).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "System.Runtime.dll"))
            };

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

            var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

            foreach (var diagnostic in failures)
            {
                Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
            }

            return null;
        }

        private static string ParseAndMergeSource(string sourceFile, Dictionary<string, string> arguments)
        {
            var merges = Assembly.GetAssembly(typeof(BaseInjectorMerge)).GetTypes().Where(a => a.BaseType == typeof(BaseInjectorMerge) && !a.IsAbstract).Select(b => (BaseInjectorMerge)Activator.CreateInstance(b)).ToList();

            foreach (var merge in merges)
            {
                sourceFile = sourceFile.Replace($"[{merge.FIELD_NAME}]", merge.Merge(arguments.ContainsKey(merge.FIELD_NAME) ? arguments[merge.FIELD_NAME] : null));
            }

            return sourceFile;
        }

        public static byte[] InjectMalwareFromTemplate(BuiltInTemplates template, [NotNull] string malwareFileName) => InjectMalwareFromTemplate(template.ToString(), malwareFileName);

        public static byte[] InjectMalwareFromTemplate([NotNull]string templateName, [NotNull] string malwareFileName)
        {
            var assembly = typeof(METLInjector).GetTypeInfo().Assembly;

            Stream resource = assembly.GetManifestResourceStream($"METL.IncludedScripts.{templateName}.cs");

            if (resource == null)
            {
                throw new ArgumentOutOfRangeException(nameof(templateName), $"{templateName} is an invalid template name");
            }

            var reader = new StreamReader(resource);
            var text = reader.ReadToEnd();

            var arguments = new Dictionary<string, string>
            {
                { "MALWARE_EMBED", malwareFileName }
            };

            return CompileCodeToBytes(ParseAndMergeSource(text, arguments));
        }

        public static byte[] InjectMalwareFromFile([NotNull] string sourceFileName, [NotNull] string malwareFileName)
        {
            var arguments = new Dictionary<string, string>
            {
                { "MALWARE_EMBED", malwareFileName }
            };

            return InjectMalwareFromFile(sourceFileName, arguments);
        }

        public static byte[] InjectMalwareFromFile([NotNull] string sourceFileName, Dictionary<string, string> arguments)
        {
            if (string.IsNullOrEmpty(sourceFileName))
            {
                throw new ArgumentNullException(sourceFileName);
            }

            if (!File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName);
            }

            var sourceCodeFileText = File.ReadAllText(sourceFileName);

            return CompileCodeToBytes(ParseAndMergeSource(sourceCodeFileText, arguments));
        }
    }
}