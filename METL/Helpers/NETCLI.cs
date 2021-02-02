using System;
using System.Diagnostics;
using System.IO;

namespace METL.Helpers
{
    public class NETCLI
    {
        public byte[] CompileAndReturnBytes(string sourceCodeContent, string projectName)
        {
            GenerateProject("console", projectName);

            var fullPath = Path.Combine(AppContext.BaseDirectory, projectName);
            
            File.WriteAllText(Path.Combine(fullPath, "Program.cs"), sourceCodeContent);

            Process.Start($"dotnet build -c Release -o Output");

            return File.ReadAllBytes(Path.Combine(fullPath, $"bin\\Release\\Net5.0\\{projectName}.exe"));
        }

        private bool GenerateProject(string projectType, string projectName)
        {
            Process.Start($"dotnet new {projectType} -n {projectName}");

            // TODO: handle errors
            return true;
        }
    }
}