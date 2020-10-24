using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace METL.Tests
{
    [TestClass]
    public class InjectTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCode()
        {
            METLParser.InjectMalwareFromFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CodeNotFound()
        {
            METLParser.InjectMalwareFromFile(Path.GetRandomFileName(), Path.GetRandomFileName());
        }
    }
}