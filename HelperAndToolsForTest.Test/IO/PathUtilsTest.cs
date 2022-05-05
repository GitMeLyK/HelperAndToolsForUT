using System;
using System.Collections.Generic;
using System.IO;
using HelperAndToolsForTest.IO;
using NUnit.Framework;

namespace HelperAndToolsForTest.Test.IO
{
    class PathUtilsTest
    {
        bool isWindows;

        [SetUp]
        public void Setup()
        {
            // .netcore
            isWindows = Environment.OSVersion.Platform.ToString().StartsWith("Win");
            // .NET Framework
            // bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows); // .NET Core
        }

        #region ###     FILENAMEISVALID     ###

        [Test]
        public void PathUtils_MethodsStatic_WhenUseToCheckAvalidFileNameWith_FileNameIsValid()
        {

            /// <summary>
            ///             ///     The following reserved characters:
            ///       (less than) (** not visible for this xml in this remark)
            ///     > (greater than)
            ///     : (colon)
            ///     " (double quote)
            ///     / (forward slash)
            ///     \ (backslash)
            ///     | (vertical bar or pipe)
            ///     ? (question mark)
            ///     * (asterisk)
            ///     
            ///     Integer value zero, sometimes referred to as the ASCII NUL character
            ///     
            ///     Special conventions names
            ///     
            ///     CON, PRN, AUX, NUL, COM1, COM2, COM3, COM4, 
            ///     COM5, COM6, COM7, COM8, COM9, LPT1, LPT2, LPT3, 
            ///     LPT4, LPT5, LPT6, LPT7, LPT8, and LPT9.
            /// </summary>

            List<String> validFileNames;
            List<String> notValidFileNames;

            if (isWindows)
            {
                validFileNames = new List<String>(new String[] {
                    "COM5.txt",         // The Convention Name on win alert for this name we to valid with extension name
                    "pippo.txt",        // Restriction on Name on win prohibited period end with SPACE
                    "pi ppo.txt",
                    "pippo.txt",
                    "$pippo.txt",
                    "pi!ppo", 
                    "plutofile.1",
                    ".tmp"
                });

                notValidFileNames = new List<String>(new String[] {
                    "COM5",             // Convention Name on win prohibited
                    "LPT1",             // Convention Name on win prohibited
                    "pippo.txt ",       // Restriction on Name on win prohibited period end with SPACE
                    "..",               // Restriction on Name on win prohibited period end with .
                    ".",                // Restriction on Name on win prohibited period end with .
                    "pippo.",           // Restriction on Name on win prohibited period end with period without extension
                    "p|ppo",
                    "pip>po.txt",
                    "pip<po.txt",
                    "pip?po.txt",
                    "pip:po.txt",
                    "pip*po.txt",
                    ">file.min",
                    "file.<"
                });

            }
            else
            {
                validFileNames = new List<String>(new String[] {
                    "pippo.txt",
                    "$pippo.txt",
                    "pi!ppo",
                    "plutofile.1"
                });

                notValidFileNames = new List<String>(new String[] {
                    "..",
                    "p|ppo",
                    "pip>po.txt",
                    "pip<po.txt",
                    "pip?po.txt",
                    "pip:po.txt",
                    "pip*po.txt",
                    ">file.min",
                    "file.<"
                });
            }

            foreach (string filename in validFileNames){
                Assert.IsTrue(PathUtils.FileNameIsValid(filename,out string errororwarning));
                Assert.IsNull(errororwarning);
            }

            foreach (string filename in notValidFileNames){
                Assert.IsFalse(PathUtils.FileNameIsValid(filename, out string errororwarning));
                Assert.IsNull(errororwarning);
            }
        }

        #endregion

        #region ###         ISFULLPATH      ###

        [Test]
        public void PathUtils_MethodsStatic_WhenUseToCheckAvalidPathWith_IsAbsoluteValidPath()
        {

            // These are full paths on Windows, but not on Linux
            TryPickedConventionTypeForPath(
                // Expceted True for this string well to be contain a Path Full on Host "Window" without error or warnings 
                HostType.WINDOWS, @"C:\dir\file.ext", PathQualified.IsFullQualified,true);

            /*
            TryIsAbsolutePath(@"C:\dir\", isWindows);
            TryIsAbsolutePath(@"C:\dir", isWindows);
            TryIsAbsolutePath(@"C:\", isWindows);   
            TryIsAbsolutePath(@"\\unc\share\dir\file.ext", isWindows);
            TryIsAbsolutePath(@"\\unc\share", isWindows);

            // These are full paths on Linux, but not on Windows
            TryIsAbsolutePath(@"/some/file", !isWindows);
            TryIsAbsolutePath(@"/dir", !isWindows);
            TryIsAbsolutePath(@"/", !isWindows);

            // Not full paths on either Windows or Linux
            TryIsAbsolutePath(@"file.ext", false);
            TryIsAbsolutePath(@"dir\file.ext", false);
            TryIsAbsolutePath(@"\dir\file.ext", false);
            TryIsAbsolutePath(@"C:", false);
            TryIsAbsolutePath(@"C:dir\file.ext", false);
            TryIsAbsolutePath(@"\dir", false); // An "absolute", but not "full" path

            // Invalid on both Windows and Linux
            TryIsAbsolutePath(null, false, false, false);
            TryIsAbsolutePath("", false, false, false);
            TryIsAbsolutePath("   ", false, false, false);
            TryIsAbsolutePath(@"C:\inval|d", false, false, false);
            TryIsAbsolutePath(@"\\is_this_a_dir_or_a_hostname", false, false);
            TryIsAbsolutePath(@"\\is_this_a_dir_or_a_hostname\", false, !isWindows);
            TryIsAbsolutePath(@"\\is_this_a_dir_or_a_hostname\\", false, !isWindows);
            */
        }

        /// <summary>Try  test with expected for IsFull IsValid IsValidChars</summary>
        private static void TryPickedConventionTypeForPath(
            HostType HOSTTYPE, 
            string path, 
            PathQualified typequestion, 
            bool? expectedIsValid = true,           // For Test expected result is True for question assert
            bool? expectedIsValidChars = true,      // For Test expected valid chars is True for question assert
            bool? expectedIsValidPathEnd = true,    // For Test expected end without space or period is True for question assert
            bool? expectedIsValidNamedRoot = true,  // For Test expcted on presence of Named reserveted for Root not valid
            bool? expectedIsValidFileName = true    // For Test expcted on presence of Named reserveted for FileName not valid
            )
        {
            bool result;

            // :: Select Convention PICKED from list to Test with Convention Host well to be use a path ::
            HostTypeConvention conventionTest = new HostTypeConvention(HOSTTYPE);

            // :: Open question if Type is Valid to promote at valid for this string path ::
            result = PathUtils.PathTypeIsValidForPromoteAt(path, typequestion,out string errororwarning, conventionTest, false);

            // :: Test if type question is equal to result expceted of question string path::
            if(expectedIsValid != null)
                Assert.AreEqual(expectedIsValid, result, $" {Enum.GetName(typequestion)} for path.valid('{path }') not is {expectedIsValid}");

            // :: valuate (in use with this convention) a Result with value of Expected for Assert is True or False ::

            // :: For Convention "Invalid Chars" in Path ::
            if(expectedIsValidChars != null)
                if (expectedIsValidChars.Value) {
                    if (errororwarning == null) errororwarning = "";
                    Assert.That(errororwarning.Contains("Not Valid Chars"), $" {Enum.GetName(typequestion)} for path.IsValidChars('{path}') not is {expectedIsValidChars}");
                } else {
                    Assert.That(!errororwarning.Contains("Not Valid Chars"), $" {Enum.GetName(typequestion)} for path.IsValidChars('{path }') not is {expectedIsValidChars}");
                }

            // :: For Convention "End of Path" with on not space and period in Path ::
            if (expectedIsValidPathEnd != null)
                if (expectedIsValidPathEnd.Value) {
                    Assert.That(errororwarning.Contains("Not End correct"), $" {Enum.GetName(typequestion)} for path.EndOfPath('{path }') not is {expectedIsValidPathEnd}");
                } else {
                    Assert.That(!errororwarning.Contains("Not End correct"), $" {Enum.GetName(typequestion)} for path.EndOfPath('{path }') not is {expectedIsValidPathEnd}");
                }

            // :: For Convention "Name for Root reserved" for names prohibited in Root of Path ::
            if (expectedIsValidNamedRoot != null)
                if (expectedIsValidNamedRoot.Value) {
                    Assert.That(errororwarning.Contains("Root Path with reserved name"), $" {Enum.GetName(typequestion)} for path.EndOfPath('{path }') not is {expectedIsValidNamedRoot}");
                } else {
                    Assert.That(!errororwarning.Contains("Root Path with reserved name"), $" {Enum.GetName(typequestion)} for path.EndOfPath('{path }') not is {expectedIsValidNamedRoot}");
                }

            // :: For Convention "Name for File reserved" for names prohibited in FileName of Path ::
            if(expectedIsValidFileName != null)
                if (expectedIsValidFileName.Value) {
                    Assert.That(errororwarning.Contains("FileName not valid"), $" {Enum.GetName(typequestion)} for path.EndOfPath('{path }') not is {expectedIsValidFileName}");
                } else {
                    Assert.That(!errororwarning.Contains("FileName not valid"), $" {Enum.GetName(typequestion)} for path.EndOfPath('{path }') not is {expectedIsValidFileName}");
                }

        }

        #endregion

        #region ###     PATHISVALID     ###



        #endregion
    }
}
