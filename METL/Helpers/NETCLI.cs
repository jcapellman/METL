using System.Diagnostics;

namespace METL.Helpers
{
    public static class NETCLI
    {
        public static bool GenerateProject(string projectType, string projectName)
        {
            Process.Start($"dotnet new {projectType} -n {projectName}");

            return true;
        }
    }
}