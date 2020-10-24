using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace METL.Tests
{
    [TestClass]
    public class AppendTests
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileNotFound()
        {
            var parser = new METLParser();

            METLParser.AppendBytesFromFile(null, Path.GetRandomFileName());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullSourceBytes()
        {
            var parser = new METLParser();

            METLParser.AppendBytesFromBytes(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullEmbedFileName()
        {
            var parser = new METLParser();

            METLParser.AppendBytesFromFile(null, null);
        }
    }
}