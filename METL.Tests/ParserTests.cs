using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace METL.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileNotFound()
        {
            var parser = new METLParser();

            METLParser.EmbedFromFile(null, Path.GetRandomFileName(), Enums.GenerationOption.APPEND);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullSourceBytes()
        {
            var parser = new METLParser();

            METLParser.EmbedFromBytes(null, null, Enums.GenerationOption.APPEND);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullEmbedFileName()
        {
            var parser = new METLParser();

            METLParser.EmbedFromFile(null, null, Enums.GenerationOption.APPEND);
        }
    }
}