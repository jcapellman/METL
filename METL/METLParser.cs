using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace METL
{
    public class METLParser
    {
        public static byte[] AppendBytesFromFile([NotNull]byte[] source, string embedFileName)
        {
            if (string.IsNullOrEmpty(embedFileName))
            {
                throw new ArgumentNullException(nameof(embedFileName));
            }

            if (!File.Exists(embedFileName))
            {
                throw new FileNotFoundException(embedFileName);
            }

            var embedBytes = File.ReadAllBytes(embedFileName);

            return AppendBytesFromBytes(source, embedBytes);
        }

        public static byte[] AppendBytesFromBytes([NotNull]byte[] source, [NotNull]byte[] embedBytes)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            System.Buffer.BlockCopy(embedBytes, 0, source, 0, source.Length);
  
            return embedBytes;
        }
    }
}