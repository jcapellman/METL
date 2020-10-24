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
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullEmbedFileName()
        {
            METLAppender.AppendBytesFromFile(null, null);
        }
    }
}