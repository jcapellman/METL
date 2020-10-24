using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using METL.Enums;

namespace METL
{
    public class METLParser
    {
        public static byte[] EmbedFromFile([NotNull]byte[] source, string embedFileName, GenerationOption generationOption = GenerationOption.APPEND)
        {
            if (string.IsNullOrEmpty(embedFileName))
            {
                throw new ArgumentNullException(nameof(embedFileName));
            }

            var embedBytes = File.ReadAllBytes(embedFileName);

            return EmbedFromBytes(source, embedBytes, generationOption);
        }

        public static byte[] EmbedFromBytes([NotNull]byte[] source, [NotNull]byte[] embedBytes, GenerationOption generationOption = GenerationOption.APPEND)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            switch (generationOption)
            {
                case GenerationOption.APPEND:
                    System.Buffer.BlockCopy(embedBytes, 0, source, 0, source.Length);
                    break;
            }

            return embedBytes;
        }
    }
}