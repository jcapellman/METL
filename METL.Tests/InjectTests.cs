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

        [TestMethod]
        public void PEInjector()
        {
            var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32.cs");

            var malFile = Path.Combine(AppContext.BaseDirectory, "Samples/sourcePE");

            var injectedBytes = METLInjector.InjectMalwareFromFile(sourceFile, malFile);

            File.WriteAllBytes("injected.exe", injectedBytes);
        }
    }
}