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
            METLAppender.AppendBytesFromFile(null, Path.GetRandomFileName());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullSourceBytes()
        {
            METLAppender.AppendBytesFromBytes(null, null);
        }

        [TestMethod]
        public void ValidSourceBytes()
        {
            var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");
            var destFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");

            METLAppender.AppendBytesFromBytes(File.ReadAllBytes(sourceFile), File.ReadAllBytes(destFile));
        }

        [TestMethod]
        public void ValidSourcFiles()
        {
            var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");
            var destFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");

            METLAppender.AppendBytesFromFile(File.ReadAllBytes(sourceFile), destFile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullEmbedFileName()
        {
            METLAppender.AppendBytesFromFile(null, null);
        }
    }
}