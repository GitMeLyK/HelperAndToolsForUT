using System;
using System.Linq;
using System.Collections.Generic;
//using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
//
using FluentAssertions;
using NUnit.Framework;
//
using HelperAndToolsForUT.Helper.Test.IocModule.BaseTests;
using HelperAndToolsForUT.Helper.Extensions.IocExtensions;
using HelperAndToolsForTest.IO;
using System.IO;

namespace HelperAndToolsForUT.Helper.Test.IOTools
{

    [TestFixture]
    public class ModuleExtensionsTests
    {
        [Test]
        public void TempFileEmpty_WhenUseTempDirectoryContextAndWriteAfter()
        {
            using(var file = new TemporaryFile())
            {
                Assume.That(file.FileInfo.Length, Is.EqualTo(0));
            
                File.WriteAllText(file, "write text after");
            
                file.FileInfo.Refresh();
                Assert.That(file.FileInfo.Length, Is.GreaterThan(0));
            }    

        }

        [Test]
        public void TempFileNamedPredicate_WhenUseTempDirectoryContextAndWriteAfter()
        {
            using (var file = new TemporaryFile("TestTempFileName.tst"))
            {
                Assume.That(file.FileInfo.Length, Is.EqualTo(0));

                File.WriteAllText(file, "write text after");

                file.FileInfo.Refresh();
                Assert.AreEqual(file.FileInfo.Extension, ".tst");
                Assert.IsTrue(file.FileInfo.FullName.Contains("\\Temp\\"));
            }
        }

        [Test]
        public void TempFileNamedPredicate_WhenUseTempSubDirectoryContextAndWriteAfter()
        {
            using (var file = new TemporaryFile("SubFoldTemp/TestTempFileName.tst"))
            {
                Assume.That(file.FileInfo.Length, Is.EqualTo(0));

                File.WriteAllText(file, "write text after");

                file.FileInfo.Refresh();
                Assert.AreEqual(file.FileInfo.Extension, ".tst");
                Assert.IsTrue(file.FileInfo.FullName.Contains("\\Temp\\"));
                Assert.IsTrue(file.FileInfo.FullName.Contains("\\Temp\\SubFoldTemp\\"));

            }
        }

        [Test]
        public void TempFileNamedPredicate_WhenUseSomeDirectoryContextAndWriteAfter()
        {
            using (var file = new TemporaryFile("TestTempFileName.tst",true))
            {
                Assume.That(file.FileInfo.Length, Is.EqualTo(0));

                File.WriteAllText(file, "write text after");

                file.FileInfo.Refresh();
                Assert.AreEqual(file.FileInfo.Extension, ".tst");
                // :: File temporary present in folder bin compiled ::
                Assert.IsTrue(file.FileInfo.FullName.Contains("\\HelperAndToolsForTest.Test\\"));
            }

        }

    }

}
