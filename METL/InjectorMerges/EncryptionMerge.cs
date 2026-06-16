using System.Security.Cryptography;
using System.Text;
using METL.InjectorMerges.Base;

namespace METL.InjectorMerges;

/// <summary>
/// Merge strategy for encrypting embedded data using AES encryption.
/// Generates a random encryption key and includes both encrypted data and key in output.
/// </summary>
public class EncryptionMerge : BaseInjectorMerge
{
    /// <inheritdoc/>
    public override string FIELD_NAME => "ENCRYPT_DATA";

    /// <inheritdoc/>
    public override string? Merge(string? argument = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            return "// No encryption data provided";
        }

        // Generate random key and IV
        using var aes = Aes.Create();
        aes.GenerateKey();
        aes.GenerateIV();

        // Encrypt the argument
        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var argumentBytes = Encoding.UTF8.GetBytes(argument);
        var encrypted = encryptor.TransformFinalBlock(argumentBytes, 0, argumentBytes.Length);

        // Return code with encrypted data, key, and IV
        var sb = new StringBuilder();
        sb.AppendLine($"byte[] encryptedData = Convert.FromBase64String(\"{Convert.ToBase64String(encrypted)}\");");
        sb.AppendLine($"byte[] encryptionKey = Convert.FromBase64String(\"{Convert.ToBase64String(aes.Key)}\");");
        sb.Append($"byte[] encryptionIV = Convert.FromBase64String(\"{Convert.ToBase64String(aes.IV)}\");");

        return sb.ToString();
    }
}
