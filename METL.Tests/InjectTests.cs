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
            METLInjector.InjectMalwareFromFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CodeNotFound()
        {
            METLInjector.InjectMalwareFromFile(Path.GetRandomFileName(), Path.GetRandomFileName());
        }
    }
}