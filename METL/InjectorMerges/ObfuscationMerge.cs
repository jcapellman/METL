using System.Text;
using METL.InjectorMerges.Base;

namespace METL.InjectorMerges;

/// <summary>
/// Merge strategy for obfuscating strings using simple XOR encoding.
/// </summary>
public class ObfuscationMerge : BaseInjectorMerge
{
    /// <inheritdoc/>
    public override string FIELD_NAME => "OBFUSCATE_STRING";

    /// <inheritdoc/>
    public override string? Merge(string? argument = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            return "string obfuscatedValue = string.Empty;";
        }

        // Use simple XOR obfuscation with a random key
        var random = new Random();
        var key = (byte)random.Next(1, 256);
        var bytes = Encoding.UTF8.GetBytes(argument);
        var obfuscated = new byte[bytes.Length];

        for (int i = 0; i < bytes.Length; i++)
        {
            obfuscated[i] = (byte)(bytes[i] ^ key);
        }

        var sb = new StringBuilder();
        sb.AppendLine($"byte[] obfuscatedBytes = Convert.FromBase64String(\"{Convert.ToBase64String(obfuscated)}\");");
        sb.AppendLine($"byte obfuscationKey = {key};");
        sb.AppendLine("byte[] deobfuscatedBytes = new byte[obfuscatedBytes.Length];");
        sb.AppendLine("for (int i = 0; i < obfuscatedBytes.Length; i++)");
        sb.AppendLine("{");
        sb.AppendLine("    deobfuscatedBytes[i] = (byte)(obfuscatedBytes[i] ^ obfuscationKey);");
        sb.AppendLine("}");
        sb.Append("string obfuscatedValue = System.Text.Encoding.UTF8.GetString(deobfuscatedBytes);");

        return sb.ToString();
    }
}
