using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

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

        public static byte[] InjectMalwareFromFile([NotNull]string sourceFileName, [NotNull]string malwareFileName)
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



            return Encoding.ASCII.GetBytes(sourceCodeFileText);
        }
    }
}