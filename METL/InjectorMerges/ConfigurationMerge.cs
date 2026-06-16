using METL.InjectorMerges.Base;

namespace METL.InjectorMerges;

/// <summary>
/// Merge strategy for embedding configuration values.
/// Supports comma-separated key=value pairs.
/// </summary>
public class ConfigurationMerge : BaseInjectorMerge
{
    /// <inheritdoc/>
    public override string FIELD_NAME => "CONFIGURATION";

    /// <inheritdoc/>
    public override string? Merge(string? argument = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            return "var configuration = new Dictionary<string, string>();";
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("var configuration = new Dictionary<string, string>");
        sb.AppendLine("{");

        // Parse key=value pairs separated by commas
        var pairs = argument.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        for (int i = 0; i < pairs.Length; i++)
        {
            var parts = pairs[i].Split('=', 2);
            if (parts.Length == 2)
            {
                var comma = i < pairs.Length - 1 ? "," : "";
                sb.AppendLine($"    {{ \"{parts[0].Trim()}\", \"{parts[1].Trim()}\" }}{comma}");
            }
        }

        sb.Append("};");
        return sb.ToString();
    }
}
