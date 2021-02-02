using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using METL.Enums;
using METL.Helpers;
using METL.InjectorMerges.Base;

namespace METL
{
    public class METLInjector
    {
        // TODO: Clean up to use a series of helper methods to wrap .NET CLI Calls
        private static byte[] CompileCodeToBytes(string sourceCode)
        {
            var projectName = Guid.NewGuid().ToString().Replace("-","");

            return new NETCLI().CompileAndReturnBytes(sourceCode, projectName);
        }

        private static string ParseAndMergeSource(string sourceFile, Dictionary<string, string> arguments)
        {
            var merges = Assembly.GetAssembly(typeof(BaseInjectorMerge))?.GetTypes().Where(a => a.BaseType == typeof(BaseInjectorMerge) && !a.IsAbstract).Select(b => (BaseInjectorMerge)Activator.CreateInstance(b)).ToList();

            if (merges == null)
            {
                throw new NullReferenceException($"Failed to obtain mail merges on {sourceFile}");
            }

            return merges.Aggregate(sourceFile, (current, merge) => 
                current.Replace($"[{merge.FIELD_NAME}]", 
                    merge.Merge(arguments.ContainsKey(merge.FIELD_NAME) ? arguments[merge.FIELD_NAME] : null)));
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