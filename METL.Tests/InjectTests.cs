﻿using System;
using System.Globalization;
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
            METLInjector.InjectMalwareFromFile(sourceFileName: null, arguments: null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CodeNotFound()
        {
            METLInjector.InjectMalwareFromFile(Path.GetRandomFileName(), Path.GetRandomFileName());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TemplateNotFound()
        {
            METLInjector.InjectMalwareFromTemplate("WICK", Path.GetRandomFileName());
        }

        [TestMethod]
        public void TemplatePE32()
        {
            var malFile = Path.Combine(AppContext.BaseDirectory, "Samples/sourcePE");

            METLInjector.InjectMalwareFromTemplate(Enums.BuiltInTemplates.PE32, malFile);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void PE32BadCode()
        {
            var malFile = Path.Combine(AppContext.BaseDirectory, "Samples/sourcePE");
            var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");

            METLInjector.InjectMalwareFromFile(sourceFile, malFile);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void PE32BadMalwareFile()
        {
            var malFile = Path.Combine(AppContext.BaseDirectory, DateTime.Now.ToString(CultureInfo.InvariantCulture));
            var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");

            METLInjector.InjectMalwareFromFile(sourceFile, malFile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PE32NullMalwareFile()
        {
            var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32Bad.cs");

            METLInjector.InjectMalwareFromFile(sourceFile, "");
        }

        [TestMethod]
        public void PEInjector()
        {
            var sourceFile = Path.Combine(AppContext.BaseDirectory, "Samples/PE32.cs");

            var malFile = Path.Combine(AppContext.BaseDirectory, "Samples/sourcePE");

            var injectedBytes = METLInjector.InjectMalwareFromFile(sourceFile, malFile);

            Assert.IsTrue(injectedBytes != null && injectedBytes.Length > 0);

            File.WriteAllBytes("injected.exe", injectedBytes);
        }
    }
}