using HelperAndToolsForTest.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#if !NETSTANDARD
    using System.Security.AccessControl;
#endif

namespace HelperAndToolsForUT.Helper.Extensions.IOExtensions
{

    /// <summary>Argument for suppositon in question if path string contain.</summary>
    public enum TypePathSupposed
    {
        /// <summary>On check string path assume with string contain a path relative to only sequence folders without referement at file!</summary>
        ForFolders,
        /// <summary>On check string path assume with string contain a path relative to only sequence folders with final referement at file complete of extension!</summary>
        ForFoldersWithFileComplete,
        /// <summary>On check string path assume with string contain a path relative to only sequence folders with final referement at file without extension!</summary>
        ForFoldersWithFileWithoutExtension
    }

    /// <summary>
    ///     Simplify for FileInfo to get FileName withoust extension.
    /// </summary>
    public static class FileInfoExtensions
    {

        #region ###     Extensions for Path Qualifier and Check Promote     ###

        /// <summary>
        ///     Gets a value indicating whether the specified @this string contains a full qualified path
        ///     on system target path and validate a path for use and scope for Directory or File to do Destination.
        /// </summary>
        /// <param name="this">The @this to test.</param>
        /// <param name="supposed">Question if string path supposed is referement to a only sequence of Directories or a sequence of Directory with final referment to File with or not extension<see cref="TypePathSupposed"/></param>
        /// <param name="errororwarning">Return if valid in false the motiv</param>
        /// <returns>
        ///     true if <paramref name="this" /> contains a full path qualified for system win and scope is for as scoe Directory or File target; otherwise, false.
        /// </returns>
        /// <remarks>
        ///     For Directory :
        ///     
        ///         @"c:\foo"     .IsPathQualificableFull(TypeScope.OnlyDirectory);     // true
        ///         @"\foo"       .IsPathQualificableFull(TypeScope.OnlyDirectory);     // false
        ///         "foo"         .IsPathQualificableFull(TypeScope.OnlyDirectory);     // false
        ///         @"c:1\foo"    .IsPathQualificableFull(TypeScope.OnlyDirectory);     // false
        ///         @"c:1\fo|o"   .IsPathQualificableFull(TypeScope.OnlyDirectory);     // false
        ///         
        ///     For File (Without extension)
        ///     
        ///         @"c:\foo\filename"    .IsPathQualificableFull(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // true
        ///         @"\foo\filename"      .IsPathQualificableFull(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         "foo"                 .IsPathQualificableFull(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         @"c:1\foo\filename"   .IsPathQualificableFull(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         @"c:1\fo|o"           .IsPathQualificableFull(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///
        ///     For File (and extension)
        ///     
        ///         @"c:\foo\filename.txt"    .IsPathQualificableFull(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // true
        ///         @"\foo\filename.txt       .IsPathQualificableFull(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         "foo"                     .IsPathQualificableFull(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         @"c:1\foo\filename.txt"   .IsPathQualificableFull(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         @"c:1\fo|o.txt"           .IsPathQualificableFull(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        /// </remarks>
        public static bool IsPathQualificableFull(this FileInfo @this, TypePathSupposed supposed, out string errororwarning)
        {
            bool valid = false;
            if (supposed ==  TypePathSupposed.ForFolders )
                valid = PathUtils.PathTypeIsValidForPromoteAt(@this.ToString(), PathQualified.IsFullWithOnlyDirectoryName, out errororwarning);
            else if (supposed == TypePathSupposed.ForFoldersWithFileWithoutExtension)
                valid = PathUtils.PathTypeIsValidForPromoteAt(@this.ToString(), PathQualified.IsFullWithFileNameWithoutExtension, out errororwarning);
            else if (supposed ==  TypePathSupposed.ForFoldersWithFileComplete)
                valid = PathUtils.PathTypeIsValidForPromoteAt(@this.ToString(), PathQualified.IsFullWithFileNameAndExtension, out errororwarning);
            else
                errororwarning = null;
            //
            return valid;
        }

        /// <summary>
        ///     Gets a value indicating whether the specified @this string contains a root 
        ///     absolute path and validate a path for use and scope for Directory or File to do Destination.
        /// </summary>
        /// <param name="this">The @this to test.</param>
        /// <param name="errororwarning">Return if valid in false the motiv</param>
        /// <param name="supposed">Question if string path supposed is referement to a only sequence of Directories or a sequence of Directory with final referment to File with or not extension<see cref="TypePathSupposed"/></param>
        /// <returns>
        ///     true if <paramref name="this" /> contains a root and scope is for as scoe Directory or File target; otherwise, false.
        /// </returns>
        /// <remarks>
        ///     For Directory :
        ///     
        ///         @"c:\foo"     .IsPathQualificableAbsolute(TypeScope.OnlyDirectory);     // true
        ///         @"\foo"       .IsPathQualificableAbsolute(TypeScope.OnlyDirectory);     // true
        ///         "foo"         .IsPathQualificableAbsolute(TypeScope.OnlyDirectory);     // false
        ///         @"c:1\foo"    .IsPathQualificableAbsolute(TypeScope.OnlyDirectory);     // surprisingly also true
        ///         @"c:1\fo|o"   .IsPathQualificableAbsolute(TypeScope.OnlyDirectory);     // surprisingly also true
        ///         
        ///     For File (Without extension)
        ///     
        ///         @"c:\foo\filename"    .IsPathQualificableAbsolute(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // true
        ///         @"\foo\filename"      .IsPathQualificableAbsolute(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // true
        ///         "foo"                 .IsPathQualificableAbsolute(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         @"c:1\foo\filename"   .IsPathQualificableAbsolute(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // surprisingly also true
        ///         @"c:1\fo|o"           .IsPathQualificableAbsolute(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // surprisingly also true
        ///
        ///     For File (and extension)
        ///     
        ///         @"c:\foo\filename.txt"    .IsPathQualificableAbsolute(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // true
        ///         @"\foo\filename.txt       .IsPathQualificableAbsolute(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // true
        ///         "foo"                     .IsPathQualificableAbsolute(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         @"c:1\foo\filename.txt"   .IsPathQualificableAbsolute(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // surprisingly also true
        ///         @"c:1\fo|o.txt"           .IsPathQualificableAbsolute(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // surprisingly also true
        /// </remarks>
        public static bool IsPathQualificableAbsolute(this FileInfo @this, TypePathSupposed supposed, out string errororwarning)
        {
            bool valid = false;
            if (supposed ==  TypePathSupposed.ForFolders)
                valid = PathUtils.PathTypeIsValidForPromoteAt(@this.ToString(), PathQualified.IsRootWithOnlyDirectoryName, out errororwarning);
            else if (supposed ==  TypePathSupposed.ForFoldersWithFileWithoutExtension)
                valid = PathUtils.PathTypeIsValidForPromoteAt(@this.ToString(), PathQualified.IsRootWithFileNameWithoutExtension, out errororwarning);
            else if (supposed ==  TypePathSupposed.ForFoldersWithFileComplete)
                valid = PathUtils.PathTypeIsValidForPromoteAt(@this.ToString(), PathQualified.IsRootWithFileNameAndExtension, out errororwarning);
            else
                errororwarning = null;
            //
            return valid;
        }

        /// <summary>
        ///     Gets a value indicating whether the specified @this string contains a base
        ///     relative path and validate a path for use and scope for Directory or File to do Destination.
        /// </summary>
        /// <param name="this">The @this to test.</param>
        /// <param name="errororwarning">Return if valid in false the motiv</param>
        /// <param name="supposed">Question if string path supposed is referement to a only sequence of Directories or a sequence of Directory with final referment to File with or not extension<see cref="TypePathSupposed"/></param>
        /// <returns>
        ///     true if <paramref name="this" /> contains a base relative path for target and scope is for as scoe Directory or File target; otherwise, false.
        /// </returns>
        /// <remarks>
        ///     For Directory :
        ///     
        ///         @"c:\foo"     .IsPathQualificableRelative(TypeScope.OnlyDirectory);     // false
        ///         @"\foo"       .IsPathQualificableRelative(TypeScope.OnlyDirectory);     // true
        ///         "foo"         .IsPathQualificableRelative(TypeScope.OnlyDirectory);     // false
        ///         @"c:1\foo"    .IsPathQualificableRelative(TypeScope.OnlyDirectory);     // false
        ///         @"c:1\fo|o"   .IsPathQualificableRelative(TypeScope.OnlyDirectory);     // false
        ///         
        ///     For File (Without extension)
        ///     
        ///         @"c:\foo\filename"    .IsPathQualificableRelative(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         @"\foo\filename"      .IsPathQualificableRelative(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // true
        ///         "foo"                 .IsPathQualificableRelative(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         @"c:1\foo\filename"   .IsPathQualificableRelative(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         @"c:1\fo|o"           .IsPathQualificableRelative(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///
        ///     For File (and extension)
        ///     
        ///         @"c:\foo\filename.txt"    .IsPathQualificableRelative(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         @"\foo\filename.txt       .IsPathQualificableRelative(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // true
        ///         "foo"                     .IsPathQualificableRelative(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         @"c:1\foo\filename.txt"   .IsPathQualificableRelative(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        ///         @"c:1\fo|o.txt"           .IsPathQualificableRelative(TypeScope.DirectoryAndFilenameDestinationWithoutExtension);     // false
        /// </remarks>
        public static bool IsPathQualificableRelative(this FileInfo @this, TypePathSupposed supposed, out string errororwarning)
        {
            bool valid = false;
            if (supposed ==  TypePathSupposed.ForFolders)
                valid = PathUtils.PathTypeIsValidForPromoteAt(@this.ToString(), PathQualified.IsRelativeWithOnlyDirectoryName, out errororwarning);
            else if (supposed ==  TypePathSupposed.ForFoldersWithFileWithoutExtension)
                valid = PathUtils.PathTypeIsValidForPromoteAt(@this.ToString(), PathQualified.IsRelativeWithFileNameWithoutExtension, out errororwarning);
            else if (supposed ==  TypePathSupposed.ForFoldersWithFileComplete)
                valid = PathUtils.PathTypeIsValidForPromoteAt(@this.ToString(), PathQualified.IsRelativeWithFileNameAndExtension, out errororwarning);
            else
                errororwarning = null;
            //
            return valid;
        }

        #endregion

        #region ###     Extensions for Operation on Context File System     ###

        /// <summary>Gets the total number of lines in a file. </summary>
        /// <param name="this">The file to perform the count on.</param>
        /// <returns>The total number of lines in a file. </returns>
        /// <inheritdoc cref="File.ReadAllLines(string)"/>
        public static int CountLines(this FileInfo @this)
        {
            return File.ReadAllLines(@this.FullName).Length;
        }

        /// <summary>Gets the total number of lines in a file that satisfy the condition in the predicate function.</summary>
        /// <param name="this">The file to perform the count on.</param>
        /// <param name="predicate">A function to test each line for a condition.</param>
        /// <returns>The total number of lines in a file that satisfy the condition in the predicate function.</returns>
        /// <exception cref="OverflowException" />
        /// <inheritdoc cref="File.ReadAllLines(string)"/>
        public static int CountLines(this FileInfo @this, Func<string, bool> predicate)
        {
            return File.ReadAllLines(@this.FullName).Count(predicate);
        }

        /// <summary>
        ///     A FileInfo extension method that renames.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="newName">Name of the new.</param>
        /// <returns>Null if OK, else Error or Warning for name or path not valid!</returns>
        /// <inheritdoc cref="FileInfo.MoveTo(string)"/>
        public static string Rename(this FileInfo @this, string newName)
        {
            if (PathUtils.FileNameIsValid(newName, errororwarning: out string errororwarning))
            {
                string filePath = Path.Combine(@this.Directory.FullName, newName);
                @this.MoveTo(filePath);
                return null;
            }
            else
            {
                return errororwarning;
            }
        }

        /// <summary>
        ///     Creates all directories and subdirectories in the specified @this if the directory doesn't already exists.
        ///     This methods is the same as FileInfo.CreateDirectory however it's less ambigues about what happen if the
        ///     directory already exists.
        /// </summary>
        /// <param name="this">The directory @this to create.</param>
        /// <returns>An object that represents the directory for the specified @this.</returns>
        /// <inheritdoc cref="Directory.CreateDirectory(string)"/>
        public static DirectoryInfo EnsureDirectoryExists(this FileInfo @this)
        {
            return Directory.CreateDirectory(@this.Directory.FullName);
        }

#if !NETSTANDARD

                /// <summary>
                ///     Creates all directories and subdirectories in the specified @this if the directory doesn't already exists.
                ///     This methods is the same as FileInfo.CreateDirectory however it's less ambigues about what happen if the
                ///     directory already exists.
                /// </summary>
                /// <param name="this">The directory to create.</param>
                /// <param name="directorySecurity">The access control to apply to the directory.</param>
                /// <returns>An object that represents the directory for the specified @this.</returns>
                /*public static DirectoryInfo EnsureDirectoryExists(this FileInfo @this, DirectorySecurity directorySecurity)
                {
                    return Directory.CreateDirectory(@this.Directory.FullName, directorySecurity);
                }
                */
#endif

        /// <summary>
        ///     Changes the extension of a @this string.
        /// </summary>
        /// <param name="this">The file to perform the count on.</param>
        /// <param name="extension">New extension to change</param>
        /// <inheritdoc cref="Path.ChangeExtension(string, string)"/>
        public static String ChangeExtension(this FileInfo @this, String extension)
        {
            return Path.ChangeExtension(@this.FullName, extension);
        }

        #endregion

        #region ###         Extension for Revisited internal Path.          ###

        /// <summary>
        ///     Returns the file name of the specified @this string without the extension.
        /// </summary>
        /// <param name="this">The @this of the file.</param>
        /// <returns>
        ///     The string returned by <see cref="M:System.IO.Path.GetFileName(System.String)" />, minus the last period (.)
        ///     and all characters following it.
        /// </returns>
        public static String GetFileNameWithoutExtension(this FileInfo @this)
        {
            return Path.GetFileNameWithoutExtension(@this.FullName);
        }

        #endregion

        #region ###       Extensions for Content of File context Info       ###

        /// <summary>
        ///     A FileInfo extension method that appends all lines.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="contents">The contents.</param>
        /// <inheritdoc cref="File.AppendAllLines(string, IEnumerable{string})"/>
        public static void AppendAllLines(this FileInfo @this, IEnumerable<String> contents)
        {
            File.AppendAllLines(@this.FullName, contents);
        }

        /// <summary>
        ///     A FileInfo extension method that appends all lines.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="encoding">The encoding.</param>
        /// <inheritdoc cref="File.AppendAllLines(string, IEnumerable{string})"/>
        public static void AppendAllLines(this FileInfo @this, IEnumerable<String> contents, Encoding encoding)
        {
            File.AppendAllLines(@this.FullName, contents, encoding);
        }

        /// <summary>
        ///     Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist,
        ///     this method creates a file, writes the specified string to the file, then closes the file.
        /// </summary>
        /// <param name="this">The file to append the specified string to.</param>
        /// <param name="contents">The string to append to the file.</param>
        /// <inheritdoc cref="File.AppendAllText(string, string)"/>
        public static void AppendAllText(this FileInfo @this, String contents)
        {
            File.AppendAllText(@this.FullName, contents);
        }

        /// <summary>
        ///     Appends the specified string to the file, creating the file if it does not already exist.
        /// </summary>
        /// <param name="this">The file to append the specified string to.</param>
        /// <param name="contents">The string to append to the file.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <inheritdoc cref="File.AppendAllText(string, string, Encoding)"/>
        public static void AppendAllText(this FileInfo @this, String contents, Encoding encoding)
        {
            File.AppendAllText(@this.FullName, contents, encoding);
        }

        #endregion

        #region ###         Extensions used to Check Valid Root Path        ###

        /// <summary>
        ///     Gets a value indicating whether the specified @this string contains 
        ///     a Absolute Path in os target system with full qualified path.
        /// </summary>
        /// <param name="this">The @this to test.</param>
        /// <returns>
        ///     true if <paramref name="this" /> contains a root; otherwise, false.
        /// </returns>
        /// <remarks>
        ///     .IsPathRooted(@"c:foo");    // false
        ///     .IsPathRooted(@"/foo");     // true
        ///     .IsPathRooted(@"c:/foo");   // true
        /// </remarks>
        /// <inheritdoc cref="Path.IsPathRooted(string)"/>
        public static Boolean IsPathRooted(this FileInfo @this)
        {
            return Path.IsPathRooted(@this.FullName);
        }

        /// <summary>
        ///     Gets a value indicating whether the specified @this string contains 
        ///     a Full Path UNC in win system with full qualified path.
        /// </summary>
        /// <param name="this">The @this to test.</param>
        /// <returns>
        ///     true if <paramref name="this" /> contains a root; otherwise, false.
        /// </returns>
        /// <remarks>
        ///     .IsPathFullyQualified(@"c:foo");    // false
        ///     .IsPathFullyQualified(@"/foo");     // false
        ///     .IsPathFullyQualified(@"c:/foo");   // true
        /// </remarks>
        /// <inheritdoc cref="Path.IsPathRooted(string)"/>
        public static Boolean IsPathFullyQualified(this FileInfo @this)
        {
            return PathUtils.IsPathFullyQualified(@this.FullName);
        }

        #endregion

    }

}
