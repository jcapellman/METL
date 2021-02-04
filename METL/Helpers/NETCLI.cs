using System;
using System.Diagnostics;
using System.IO;

namespace METL.Helpers
{
    public class NETCLI
    {
        private void DotnetProcess(string arguments, string workingPath = null)
        {
            var procInfo = new ProcessStartInfo("dotnet")
            {
                Arguments = arguments,
                WorkingDirectory = workingPath ?? AppContext.BaseDirectory,
                RedirectStandardOutput = true
            };

            var process = new Process
            {
                StartInfo = procInfo
            };

            process.Start();

            process.WaitForExit();
        }

        public byte[] CompileAndReturnBytes(string sourceCodeContent, string projectName)
        {
            DotnetProcess($"new console -n {projectName}");

            var fullPath = Path.Combine(AppContext.BaseDirectory, projectName);

            File.WriteAllText(Path.Combine(fullPath, "Program.cs"), sourceCodeContent);

            DotnetProcess("publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false", fullPath);

            return File.ReadAllBytes(Path.Combine(fullPath, $"bin\\Release\\net5.0\\win-x64\\publish\\{projectName}.exe"));
        }
    }
}