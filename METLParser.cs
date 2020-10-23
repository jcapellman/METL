using METL.Enums;

using System.Diagnostics.CodeAnalysis;

namespace METL
{
    public class METLParser
    {
        public byte[] EmbedFromBytes([NotNull]byte[] source, [NotNull]byte[] embedBytes, GenerationOption generationOption = GenerationOption.APPEND)
        {
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