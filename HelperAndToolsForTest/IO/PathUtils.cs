using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace HelperAndToolsForTest.IO
{

    /// <summary>
    ///     On check this, the method check if root is Valid without chars and reserveded name on os winodws or in generally
    ///     if a path is valid for Full Qualified question name convention.
    ///     For example.:
    ///     on windows .:   "C:Documents" is False for FullQualified and True For Rooted on linux is True 
    ///     on windows .:   "/Documents" is False for FullQualified and True For Rooted on linux is True
    ///     on windows .:   "C:\Documents" is True for FullQualified and True For Rooted on linux is True
    ///     
    ///     If use a chars or name intranal a windows used for convention we special name, or use space finals or period . at end
    ///     the method return false.
    ///     
    ///     The questions for validate a Path in use check if path is a Relative or Absolute and if present 
    ///     on string used for path a filename convention with or not with extension.
    ///     
    ///     Example.:
    ///     
    ///         Question IsRelativeQualifiedPathForUseToFileNameWithoutExtension: case 1    // Relative
    ///         Question IsRootQualifiedPathForUseToFileNameWithoutExtension: case 2        // Absolute
    ///         Question IsFullQualifiedPathForUseToFileNameWithoutExtension: case 3        // UNC DRIVE Absolute
    ///         
    ///     1: C:Documents\filename        is Rooted not full with result to is relative a destination and valid for use to apply with filename without extension.
    ///         --> Result called from c:\project\myBin result is c:\project\myBin\Documents\filename
    ///     2: /Documents/filename         is Rooted not full with result to is absolute for destination and valid for use to apply with filename without extension.
    ///         --> Result called from c:\project\myBin result is c:\Documents\filename.ext 
    ///     3: C:/Documents/filename.ext   is Rooted And Full with result to is absolute for destination and valid for use to apply with filename without extension.
    ///         --> Result called from c:\project\myBin result is c:\Documents\filename.ext 
    ///         
    ///         Question IsRelativeQualifiedPathForUseToFileNameAndExtension: case 1    // Relative
    ///         Question IsRootQualifiedPathForUseToFileNameAndExtension: case 2        // Absolute
    ///         Question IsFullQualifiedPathForUseToFileNameAndExtension: case 3        // UNC DRIVE Absolute
    ///         
    ///     1: C:Documents\filename.ext    is Rooted not full with result to is relative a destination and valid for use to apply with filename comprensive of extension.
    ///         --> Result called from c:\project\myBin result is c:\project\myBin\Documents\filename.ext 
    ///     2: /Documents/filename.ext     is Rooted not full with result to is absolute for destination and valid for use to apply with filename comprensive of extension.
    ///         --> Result called from c:\project\myBin result is c:\Documents\filename.ext 
    ///     3: C:/Documents/filename.ext   is Rooted And Full with result to is absolute for destination and valid for use to apply with filename comprensive of extension.
    ///         --> Result called from c:\project\myBin result is c:\Documents\filename.ext 
    ///         
    ///         Question IsRelativeQualifiedPathForUseToDirectoryName: case 1   // Relative
    ///         Question IsRootQualifiedPathForUseToDirectoryName: case 2       // Absolute
    ///         Question IsFullQualifiedPathForUseToDirectoryName: case 3       // UNC DRIVE Absolute
    ///         
    ///     1: C:Documents\directoryname\  is Rooted not full with result to is relative a destination and valid for use to apply for sub Directory with Name.
    ///         --> Result called from c:\project\myBin result is c:\project\myBin\Documents\directoryname\ 
    ///     2: /Documents/directoryname/   is Rooted not full with result to is absolute for destination and valid for use to apply for sub Directory with Name.
    ///         --> Result called from c:\project\myBin result is c:\Documents\directoryname\ 
    ///     3: C:/Documents/directoryname/ is Rooted And Full with result to is absolute for destination and valid for use to apply for sub Directory with Name.
    ///         --> Result called from c:\project\myBin result is c:\Documents\directoryname\
    /// 
    /// </summary>
    public enum PathQualified
    {
        // FULL QUALIFIED
        /// <summary> IS_FULL_PATH_QUALIFIED OR NO_FULL_PATH_QUALIFIED </summary>
        [Description("Returns a value that indicates full whether the specified file path is fixed to a specific drive or UNC path, and is valid for question to use FULL.")]
        IsFullQualified,

        /// <summary> IS_FULL_PATH_WITH_DIRECTORYNAME OR NO_FULL_PATH_WITH_DIRECTORYNAME </summary>
        [Description("Returns a value that indicates full whether the specified file path is fixed to a specific drive or UNC path, and is valid for question to use with file name without extension.")]
        IsFullWithFileNameWithoutExtension,

        /// <summary>IS_FULL_PATH_WITH_FILENAME_AND_EXT || NO_FULL_PATH_WITH_FILENAME_AND_EXT</summary>
        [Description("Returns a value that indicates full whether the specified file path is fixed to a specific drive or UNC path, and is valid for question to use with file name and extension.")]
        IsFullWithFileNameAndExtension,

        /// <summary>IS_FULL_PATH_WITH_FILENAME_WITHOUT_EXT || NO_FULL_PATH_WITH_FILENAME_WITHOUT_EXT</summary>
        [Description("Returns a value that indicates full whether the specified directory path is fixed to a specific drive or UNC path, and is valid for question to use for Directory Name.")]
        IsFullWithOnlyDirectoryName,

        // ABSOLUTE QUALIFIED
        /// <summary> IS_FULL_PATH_ROOT OR NO_FULL_PATH_ROOT </summary>
        [Description("Returns a value that indicates absolute whether the specified file path is fixed to a specific drive or UNC path, and is valid for question to use ABOSLUTE.")]
        IsRootQualified,

        /// <summary>IS_ABSOLUTE_PATH_WITH_DIRECTORYNAME || NO_FULL_PATH_WITH_DIRECTORYNAME</summary>
        [Description("Returns a value that indicates absolute whether the specified file path is root absolute to a specific path, and is valid for question to use with file name without extension.")]
        IsRootWithFileNameWithoutExtension,

        /// <summary>IS_ABSOLUTE_PATH_WITH_FILENAME_AND_EXT || NO_ABSOLUTE_PATH_WITH_FILENAME_AND_EXT</summary>
        [Description("Returns a value that indicates absolute whether the specified file path is root absolute to a specific path, and is valid for question to use with file name and extension.")]
        IsRootWithFileNameAndExtension,

        /// <summary>IS_ABSOLUTE_PATH_WITH_FILENAME_WITHOUT_EXT || NO_ABSOLUTE_PATH_WITH_FILENAME_WITHOUT_EXT</summary>
        [Description("Returns a value that indicates absolute whether the specified directory path is root absolute to a specific path, and is valid for question to use for Directory Name.")]
        IsRootWithOnlyDirectoryName,

        // RELATIVE QUALIFIED
        /// <summary> IS_RELATIVE_PATH_ROOT OR NO_RELATIVE_PATH_ROOT </summary>
        [Description("Returns a value that indicates relative whether the specified file path is fixed to a relative current path relativly, and is valid for question to use RELATIVE.")]
        IsRelativeQualified,

        /// <summary>IS_RELATIVE_PATH_WITH_DIRECTORYNAME || NO_RELATIVE_PATH_WITH_DIRECTORYNAME</summary>
        [Description("Returns a value that indicates relative whether the specified file path is base relative to a specific path, and is valid for question to use with file name without extension.")]
        IsRelativeWithFileNameWithoutExtension,

        /// <summary>IS_RELATIVE_PATH_WITH_FILENAME_AND_EXT || NO_RELATIVE_PATH_WITH_FILENAME_AND_EXT</summary>
        [Description("Returns a value that indicates relative whether the specified file path is base relative to a specific path, and is valid for question to use with file name and extension.")]
        IsRelativeWithFileNameAndExtension,

        /// <summary>IS_RELATIVE_PATH_WITH_FILENAME_WITHOUT_EXT || NO_RELATIVE_PATH_WITH_FILENAME_WITHOUT_EXT</summary>
        [Description("Returns a value that indicates relative whether the specified directory is base relative to a specific path, and is valid for question to use for Directory Name.")]
        IsRelativeWithOnlyDirectoryName

    }

    /// <summary>
    ///     On Full Path or Absolute or Relative Paths
    ///     check as if file info iss relative to scope
    ///     in use to for Directory Target, or File Destination 
    ///     with or not with extension final.
    /// </summary>
    public enum PathType
    {
        /// <summary>Result of analyze string is probabilly Root without Path Directories and not contain a filename</summary>
        OnlyRoot,
        /// <summary>Result of analyze string is probabilly Path Directories and not contain a filename</summary>
        OnlyDirectories,
        /// <summary>Result of analyze string is only filename without extension, no contains Path parent</summary>
        OnlyFileWithExtension,
        /// <summary>Result of analyze string is only filename with extension, no contains Path parent</summary>
        OnlyFileWithoutExtension,
        /// <summary>Result of analyze string is only filename with extension, with probabilly Path Directories and contain a filename without extension</summary>
        DirectoriesWithFilenameWithoutExtension,
        /// <summary>Result of analyze string is only filename with extension, with probabilly Path Directories and contain a filename with extension</summary>
        DirectoriesWithFilenameAndExtension,
        /// <summary>Result of analyze string is not valid or null for check</summary>
        NotValid
    }

    /// <summary>
    ///     Utils statics method concern String contained a Path
    /// </summary>
    public static class PathUtils
    {

        /// <summary>
        ///     Context of directory BIN
        /// </summary>
        public static string contextBinDir;

        #region ###         Context Assembly BIN path        ###

        /// <summary>
        ///     Reference base
        /// </summary>
        static PathUtils() {
            System.Reflection.Assembly entryAssembly = System.Reflection.Assembly.GetEntryAssembly() ?? typeof(PathUtils).Assembly;
            contextBinDir = Path.GetFullPath(Path.GetDirectoryName(entryAssembly.Location));
        }

        /// <summary>
        ///     Returns the directory bin of context.
        /// </summary>
        public static string FullPathOfCurrentContextBin
        {
            get
            {
                if (string.IsNullOrEmpty(contextBinDir))
                {
                    System.Reflection.Assembly entryAssembly = System.Reflection.Assembly.GetEntryAssembly() ?? typeof(PathUtils).Assembly;
                    contextBinDir = Path.GetFullPath(Path.GetDirectoryName(entryAssembly.Location));
                }
                return contextBinDir;
            }
        }

        #endregion

        #region ###        Path File From String Analyze And Validations        ###

        /// <summary>
        ///     Check as string if is it a valid string to use for
        ///     full path comprensive of filename and directory.
        /// </summary>
        /// <param name="stringPathCompleteOrPartialOrOnlyFile">String we to contain a Full qualified path and or not filename and or not extension or not or also partial path and filename and or not extension or also nul path and only name of file with or not extension.</param>
        /// <param name="validate">Check validate Chars and string not null or space also not validate for error or warnings for this.</param>
        /// <param name="isPathFullyQualified">If result a Full Qualified Path Valid</param>
        /// <param name="isPathRootQualified">If has a Root Qualified Path Valid</param>
        /// <param name="rootOfPath">Return a Root identificative of current path if exist</param>
        /// <param name="fileNameFound">If contain a valid filename</param>
        /// <param name="extensionFound">If filename is with a valid extension</param>
        /// <param name="errororwarnings">Error or Wanrings on string analyzer</param>
        /// <param name="convention"> For default analyze path on string with convention used a runtime of host, also use this object to customize behavior to validate</param>
        /// <returns>True for analyzed path and file, and not have null or space not valid or other conventions exsclusive of host.</returns>
        /// <remarks>
        ///     On Analyze string to view if is qualified as path valid.
        ///     
        ///         Check If Path is a Path Full example.: c:\dir\ x:\dir\sub\ c:\dir\subdir\filenoextension c:\dir\subdir\file.ext
        ///         or Path is a Absolute.: \dir\ \dir\sub\ \dir\subdir\filenoextension \dir\subdir\file.ext
        ///         or Path is a Relative.: dir\ dir\sub dirorfile subdir\file.ext .\subdir\ .\dir\file.ext
        ///         
        ///         Check return if a Path contain only file example.: file file.ext namefile.abc
        ///         
        ///         Check return if filenae only or filename in a context of path of sequesnce dirs is or not with extension
        ///         
        ///         If not resolve return error or warnings in presence of Chars not valid on path or on filename, check if
        ///         string path is not empty and not is sull and for default valid a convention if a end of path not conatin path or period
        ///         and this and other is modeled to valuate presumible path from conventions dictate in object
        ///         of conventison <see cref="HostTypeConvention"/>.
        ///         
        ///         This conventions assume for default context of string analayzed resolve with conventions 
        ///         for a Host on execution in runtime, also is it possible to use specific host typre from
        ///         custom conventions from default, or custom conventions from arguments.
        ///         
        ///         ** For Full Qualified 
        ///         Possible patterns for the string returned by this method are on MSDN as follows:
        ///             An empty string (path specified a relative path on the current drive or volume).
        ///             ".\dir\" a relative path from this execution
        ///             "/" (path specified an absolute path on the current drive).
        ///             "X:" (path specified a relative path on a drive, where X represents a drive or volume letter).
        ///             "X:/" (path specified an absolute path on a given drive).
        ///             "\\ComputerName\SharedFolder" (a UNC path).
        /// </remarks>
        internal static bool AnalyzeStringPathOrFileQualifier(
                string stringPathCompleteOrPartialOrOnlyFile,
                bool validate,
                out bool isPathFullyQualified,                  
                out bool isPathRootQualified,                   
                out string rootOfPath,
                out string fileNameFound,                       
                out string extensionFound,                      
                out string errororwarnings,                     
                HostTypeConvention convention = null            
            )
        {
            // :: Start with default conventions to analyze path also use a custom from user ::
            if (convention == null) { convention = new HostTypeConvention(); } // RUNTIME DEFAULTS

            // :: Initial state for results ::
            isPathFullyQualified = false; isPathRootQualified = false; fileNameFound = null; extensionFound = null; errororwarnings = null;
            // :: results ::
            string currentPath; string fileNameCompleteFound = null; rootOfPath = null;

            // Check if a Full Qualified and remove to Check Valid Chars and sequence correct for Path.
            // Only control is valid for windows system compatibiles (Win DOS NTF on linux etc.)
            if ( PathUtils.IsPathFullyQualified(stringPathCompleteOrPartialOrOnlyFile)) {
                currentPath = stringPathCompleteOrPartialOrOnlyFile.Replace(Path.GetPathRoot(stringPathCompleteOrPartialOrOnlyFile), "");
                isPathFullyQualified = true;
            }
            else
                currentPath = stringPathCompleteOrPartialOrOnlyFile;

            // :: Check Root of Path ::
            if (isPathFullyQualified)
                rootOfPath = Path.GetPathRoot(stringPathCompleteOrPartialOrOnlyFile);
            else
                rootOfPath = "";        // Assume this relative at context of execution current and not in string analayzed
            //

            // All other system check if compose string with reference to root path (ABSOLUTE)
            if (Path.IsPathRooted(currentPath)) {
                currentPath = Path.GetPathRoot(currentPath);
                isPathRootQualified = true;
            }

            // :: Check if root path start with special names reserved ::
            bool errInvalidRootStart = false;
            if ((isPathRootQualified || isPathFullyQualified) && validate) {
                errInvalidRootStart = convention.ReservedNamesForRoot.Any(x => currentPath.StartsWith(x));
            }
            if (errInvalidRootStart) {
                errororwarnings = $"error: the string for path cannot contain name reserved for this file system os {convention.HostValidation}.";
                return false;
            }

            // :: Check if compose string with filename ::
            if (!String.IsNullOrWhiteSpace(Path.GetFileName(currentPath)))
            {
                fileNameCompleteFound = Path.GetFileName(currentPath);
                if (!String.IsNullOrWhiteSpace(Path.GetExtension(fileNameCompleteFound)))
                {
                    fileNameFound = Path.GetFileNameWithoutExtension(currentPath);
                    extensionFound = Path.GetExtension(fileNameFound);
                    currentPath = currentPath.Substring(0, currentPath.Length - fileNameCompleteFound.Length);
                }
            }

            // :: Check Invalid Chars on only Path (ABSOLUTE OR RELATIVE) ::
            bool errInvalidPathChars = false;
            if (isPathRootQualified && validate){
                errInvalidPathChars = convention.ListOfInvalidCharsForPath.Aggregate(
                                    currentPath, (current, c) => current.Replace(c.ToString(), string.Empty)
                                  ) != currentPath;
            }
            if (errInvalidPathChars)
            {
                errororwarnings = "error: the string for path cannot contain any of the characters that are not accepted for nomenclature.";
                return false;
            }

            // ::Check If name of File and Extension if present is a Filename Valid::
            if (fileNameCompleteFound != null && validate) {
                if (FileNameIsValid(fileNameCompleteFound, out errororwarnings, true,true, convention))
                    return false;
            }

            // All as oK with info to promote path valid!
            return true;

        }

        #endregion

        #region ###         VOIDS FOR CHECKS ON FILENAME            ###

        /// <summary>
        ///     Check that the file name does not contain a reference path but only and exclusively a file name.
        /// </summary>
        /// <param name="FileName">The string which presumably should be just a name to give to a file.</param>
        /// <returns>True if filename is a string valid as clean name without other references to a path.</returns>
        public static bool FileNameContainsReferenceToPath(string FileName)
        {
            return Path.GetDirectoryName(FileName) != null;
        }

        /// <summary>
        ///     Return if a string contain probabilly filename is valid to use in current OS
        /// </summary>
        /// <param name="fileName">String for filename to adopt for check if is valid</param>
        /// <param name="errororwarning">In out return specific error cause</param>
        /// <param name="checkIfFilenameisACleanName">Invalidate a filename if name is part of path relative and not exclusive name of file</param>
        /// <param name="checkIfFilenameIsReservedOnHost">Invalidate a filename if name file end with space or period .</param>
        /// <param name="convention"> For default analyze path on string with convention used a runtime of host, also use this object to customize behavior to validate</param>
        /// <remarks>
        /// 
        ///     with args Restrictive this chars for windows is negated
        /// 
        ///     The following reserved characters:
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
        ///     ** Also avoid these names followed immediately by an extension; 
        ///        for example, NUL.txt is not recommended.For more information
        /// 
        ///     Do not end a file or directory name with a space or a period.Although the 
        ///     underlying file system may support such names, the Windows shell and user 
        ///     interface does not.However, it is acceptable to specify a period as the 
        ///     first character of a name.For example, ".temp".        
        /// </remarks>
        /// <returns>True Valid FileName</returns>
        public static bool FileNameIsValid(string fileName, out string errororwarning, 
                        bool checkIfFilenameisACleanName = true,        // Validate if string is used for identificate esclusive filename with or not extension and not contains referement to subpath 
                        bool checkIfFilenameIsReservedOnHost = true,    // Validate and exit if Filname is reserved on Host
                        HostTypeConvention convention = null
                    )
        {

            // Start with default conventions to analyze path also use a custom from user ::
            if (convention == null) { convention = new HostTypeConvention(); }

            // Results
            errororwarning = "";

            // Not properly corretc on name of file to end with space
            if (fileName.EndsWith(" ") || fileName.EndsWith(".")) {
                errororwarning += "warning: the filename cannot end with spaces or periods\n";
                if (convention.Options.CheckEndNameOnPath) return false;
            }

            // name conventions in windows to not use for name file
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (convention.ReservedNamesForFileName.Contains(fileName)) {
                    errororwarning += "warning: the file name cannot in a windows system have one of the names reserved by the system.\n";
                    if (checkIfFilenameIsReservedOnHost) return false;
                }
            }

            // name on this check if name is clean name and not a part of relative path in subfolders.
            if (Path.GetDirectoryName(fileName) != null && Path.GetDirectoryName(fileName) != "") {
                errororwarning += "warning: the filename is not a string that contains only the filename, it also has references to a path.\n";
                if (checkIfFilenameisACleanName) return false;
            }

            // Check Chars not valid for Filename conventions host
            bool result = Path.GetInvalidFileNameChars().Aggregate(
                    fileName, (current, c) => current.Replace(c.ToString(), string.Empty)) == fileName;

            if (!result)
                errororwarning += "error: the file name cannot contain any of the characters that are not accepted for nomenclature.";

            errororwarning = errororwarning == "" ? null : errororwarning;
            return result;
        }


        #endregion

        #region ###             VOIDS FOR CHECKS ON PATH            ###

        /// <summary>
        ///     Return if a string contain probabilly path is valid to use in current OS
        /// </summary>
        /// <param name="path">String for path to adopt for check if is valid</param>
        /// <param name="question">Check validation for scope to promote a string path valid for <see cref="PathQualified"/></param>
        /// <param name="conventionHost">If not specified use a defaults from Runtime OS in execution policy for standard conventions to check validity also use a object <see cref="HostTypeConvention"/> to validate policy and options for this path.</param>
        /// <param name="validate">Check non conformity on Name File or Directory or name do no use in os destination</param>
        /// <param name="errororwarning">Return a error on confomity of question or warning if ambiguos for path to correlate question</param>
        /// <returns>True if question is conform or false if not conform</returns>
        public static bool PathTypeIsValidForPromoteAt(string path, PathQualified question, out string errororwarning, HostTypeConvention conventionHost = null, bool validate = true)
        {
            bool result = false;
            string outOnNotValid = "FULL";

            bool errInvalidPathNull = String.IsNullOrWhiteSpace(path);

            AnalyzeStringPathOrFileQualifier(path, validate,  out bool isPathFullyQualified, out bool isPathRootQualified, out string rootOfPath, out string fileNameFound, out string extensionFound, out string errororwarnings, conventionHost);

            switch (question)
            {
                // Question FULL is FullQualified
                case PathQualified.IsFullQualified: outOnNotValid = "FULL"; if (isPathFullyQualified) result = true; break;
                // Question FULL for Dir
                case PathQualified.IsFullWithOnlyDirectoryName: outOnNotValid = "FULL"; if (isPathFullyQualified && fileNameFound == null) result = true; break;
                // Question FULL for Filename and extension
                case PathQualified.IsFullWithFileNameAndExtension: outOnNotValid = "FULL"; if (isPathFullyQualified && fileNameFound != null && extensionFound != null) result = true; break;
                // Question FULL for Filename without extension
                case PathQualified.IsFullWithFileNameWithoutExtension: outOnNotValid = "FULL"; if (isPathFullyQualified && fileNameFound != null && extensionFound == null) result = true; break;
                //
                // Question ABSOLUTE is Root
                case PathQualified.IsRootQualified: outOnNotValid = "FULL"; if (isPathRootQualified) result = true; break;
                // Question ABSOLUTE for Dir
                case PathQualified.IsRootWithOnlyDirectoryName: outOnNotValid = "ABSOLUTE"; if (isPathRootQualified && fileNameFound == null) result = true; break;
                // Question ABSOLUTE for Filename and extension
                case PathQualified.IsRootWithFileNameAndExtension: outOnNotValid = "ABSOLUTE"; if (isPathRootQualified && fileNameFound != null && extensionFound != null) result = true; break;
                // Question ABSOLUTE for Filename without extension
                case PathQualified.IsRootWithFileNameWithoutExtension: outOnNotValid = "ABSOLUTE"; if (isPathRootQualified && fileNameFound != null && extensionFound == null) result = true; break;
                //
                // Question RELATIVE is Not Root and Not Full
                case PathQualified.IsRelativeQualified: outOnNotValid = "FULL"; if (!isPathRootQualified) result = true; break;
                // Question RELATIVE for Dir
                case PathQualified.IsRelativeWithOnlyDirectoryName: outOnNotValid = "RELATIVE"; if (!isPathRootQualified && fileNameFound == null) result = true; break;
                // Question RELATIVE for Filename and extension
                case PathQualified.IsRelativeWithFileNameAndExtension: outOnNotValid = "RELATIVE"; if (!isPathFullyQualified && fileNameFound != null && extensionFound != null) result = true; break;
                // Question RELATIVE for Filename without extension
                case PathQualified.IsRelativeWithFileNameWithoutExtension: outOnNotValid = "RELATIVE"; if (!isPathFullyQualified && fileNameFound != null && extensionFound == null) result = true; break;
            }

            if (!errInvalidPathNull && errororwarnings != null && !result)
                errororwarning = $"error: Path Not Qualificable for {outOnNotValid}. Not Valid!";
            else if (errInvalidPathNull)
                errororwarning = "warning: Invalid Path to Question is Null";
            else if (errororwarnings != null)
                errororwarning = $"::error or warnings on string path:: \n {errororwarnings}";
            else
                errororwarning = null;
            //
            return result;
        }

        /// <summary>
        ///     Return if a string contain probabilly path is valid to use in current OS
        /// </summary>
        /// <param name="path">String for path to adopt for check if is valid</param>
        /// <param name="qualified">Return INVALID FULL ABOSOLUTE OR RELATIVE</param>
        /// <param name="validate">Check non conformity on Name File or Directory or name do no use in os destination</param>
        /// <param name="errororwarning">Return a error on confomity of question or warning if ambiguos for path to correlate question</param>
        /// <returns>True if question is conform or false if not conform</returns>
        public static PathType GetPathType(string path, out string qualified, out string errororwarning, bool validate = true)
        {

            errororwarning = null;
            qualified = "INVALID";

            if (String.IsNullOrWhiteSpace(path))
            {
                if (validate) errororwarning = "error: string path is empty or null.";
                return PathType.NotValid;
            }

            bool result = AnalyzeStringPathOrFileQualifier(path, validate, out bool isPathFullyQualified, out bool isPathRootQualified, out string rootOfPath, out string fileNameFound, out string extensionFound, out string errororwarnings);

            // Qualifier
            if (!isPathFullyQualified && !isPathRootQualified)
                qualified = "RELATIVE";
            else if (isPathFullyQualified && !isPathRootQualified)
                qualified = "FULL";
            else if (!isPathFullyQualified && isPathRootQualified)
                qualified = "ABSOLUTE";
            //

            // Path Type
            if ((isPathFullyQualified || isPathRootQualified) && fileNameFound != null && extensionFound != null)
                return PathType.DirectoriesWithFilenameAndExtension;
            else if ((isPathFullyQualified || isPathRootQualified) && fileNameFound != null && extensionFound == null)
                return PathType.DirectoriesWithFilenameWithoutExtension;
            else if ((isPathFullyQualified || isPathRootQualified) && fileNameFound == null && extensionFound == null)
            {
                if (Path.GetPathRoot(path) == path)
                    return PathType.OnlyRoot;
                else
                    return PathType.OnlyDirectories;
            }
            else if ((!isPathFullyQualified && !isPathRootQualified) && fileNameFound != null && extensionFound != null)
                return PathType.OnlyFileWithExtension;
            else if ((!isPathFullyQualified && !isPathRootQualified) && fileNameFound != null && extensionFound == null)
                return PathType.OnlyFileWithoutExtension;
            else
                return PathType.NotValid;

        }

        /// <summary>
        ///     Universal use with or without netstandard
        /// </summary>
        /// <param name="path">path to check if is Full</param>
        /// <returns>True if path is verify on UNC path to promote at Full Wualified name</returns>
        public static bool IsPathFullyQualified(string path)
        {
            #if !NETSTANDARD
                return Path.IsPathFullyQualified(path);
            #else
            if (path == null) return false; // throw new ArgumentNullException(nameof(path));
            if (path.Length < 2) return false; //There is no way to specify a fixed path with one character (or less).
            if (path.Length == 2 && IsValidDriveChar(path[0]) && path[1] == System.IO.Path.VolumeSeparatorChar) return true; //Drive Root C:
            if (path.Length >= 3 && IsValidDriveChar(path[0]) && path[1] == System.IO.Path.VolumeSeparatorChar && IsDirectorySeperator(path[2])) return true; //Check for standard paths. C:\
            if (path.Length >= 3 && IsDirectorySeperator(path[0]) && IsDirectorySeperator(path[1])) return true; //This is start of a UNC path
            return false; //Default
            #endif
        }

        private static bool IsDirectorySeperator(char c) => c == System.IO.Path.DirectorySeparatorChar | c == System.IO.Path.AltDirectorySeparatorChar;
        private static bool IsValidDriveChar(char c) => c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z';

        #endregion

        #region ###       Utils to Get a Path Free Name      ###

        /// <summary>
        ///     Returns a valid path for destination when file already exist.
        /// </summary>
        /// <param name="file">Object InfoFile on conain a ref to file relative to get next valid item on directory existent.</param>
        /// <returns>A path that doesn't relate to any existing file or directory.</returns>
        /// <example>
        ///     Assuming the directory <c>C:\MyDir\</c> contains the file <c>FileTest.txt</c>,
        ///     the code <c>PathUtils.GetNextNameCountedForFile(new FileInfo(@"C:\MyDir\FileTest.txt"));</c>
        ///     will return a next valid file name counted to use<c>C:\MyDir\FileTest(2).txt</c>.
        /// </example>
        public static string GetNextNameCountedForFile(FileInfo file)
        {
            int nameNum = 1;
            string path = file.FullName;
            while (Directory.Exists(path) || File.Exists(path))
            {
                nameNum++;
                path = $"{Path.GetFileNameWithoutExtension(file.FullName)}({nameNum}){file.Extension}";
            }
            return path;
        }

        #endregion

        #region ###     Utils to Get a Path Relative         ###

        /// <summary>
        ///     Returns the relative path for file from alleged path to another.
        /// </summary>
        /// <param name="originPathFile">The path of file to make a relative from other.</param>
        /// <param name="relativePathDestination">Path to make if is it relative to.</param>
        /// <returns>A path relative for destination, if <see cref="System.IO.Path.Combine(string,string)">combined</see> with <c>relativeTo</c>, equals the original path.</returns>
        /// <example>
        ///     <c>PathUtils.GetRelativeFilePath(@"C:\MyDir\SubDir\FileTest.txt", @"C:\MyDir")</c> will return <c>SubDir\File.txt</c>.
        /// </example>
        public static string GetRelativeFilePath(string originPathFile, string relativePathDestination = ".")
        {
            string origin = Path.GetFullPath(originPathFile);
            string relative = Path.GetFullPath(relativePathDestination);

            // Destination string relative is not valid if path contain full path with other file corrispondence.
            if (Path.GetFileName(relative) != null)
                return null;

            string fileOrigin = Path.GetFileName(origin);
            string foldOrigin = Path.GetDirectoryName(fileOrigin);

            // If Origin and Destination is not in some Root context this not valid to generate relative path.
            if (Directory.GetDirectoryRoot(foldOrigin) != Directory.GetDirectoryRoot(relative))
                return null;

            // Tokenize with os env separator for directory!
            string resultDir = "";
            string[] dirOriginToken = foldOrigin.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string[] dirRelateToken = relative.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // Iteration to Parent directory from context relative to origin and use a other 
            // recursive iteration for give up parent directory until we've reached the smallest
            // mutual directory context of relative for file path.
            int numBackDir = dirOriginToken.Length - dirRelateToken.Length;
            int sameDirIndex = int.MaxValue;
            for (int i = 0; i < Math.Min(dirOriginToken.Length, dirRelateToken.Length); i++)
            {
                if (dirOriginToken[i] != dirRelateToken[i])
                {
                    numBackDir = dirRelateToken.Length - i; break;
                }
                else
                {
                    sameDirIndex = i;
                }
            }
            if (numBackDir > 0)
            {
                resultDir = (".." + Path.DirectorySeparatorChar).Multiply(numBackDir) + resultDir;
            }
            for (int i = sameDirIndex + 1; i < dirOriginToken.Length; i++)
            {
                resultDir = Path.Combine(resultDir, dirOriginToken[i]);
            }

            return Path.Combine(resultDir, fileOrigin);
        }

        /// <summary>
        ///     Returns the relative path for directory from alleged path to another.
        /// </summary>
        /// <param name="originPathDirectory">The path of directory to make a relative from other.</param>
        /// <param name="relativePathDestination">Path to make if is it relative to.</param>
        /// <returns>A path relative for destination, if <see cref="System.IO.Path.Combine(string,string)">combined</see> with <c>relativeTo</c>, equals the original path.</returns>
        /// <example>
        ///     <c>PathUtils.GetRelativeFilePath(@"C:\MyDir\SubDir\", @"C:\MyDir")</c> will return <c>SubDir\</c>.
        /// </example>
        public static string GetRelativeDirectoryPath(string originPathDirectory, string relativePathDestination = ".")
        {
            // Destination string relative is not valid if path contain full path with file corrispondence.
            if (Path.GetFileName(Path.GetFullPath(relativePathDestination)) != null) return null;
            // Reuse a GetRelativeFilePath for get relative.
            string fakeFilePathRelative = GetRelativeFilePath(Path.Combine(originPathDirectory, "_._"), relativePathDestination);
            if (fakeFilePathRelative == null) return null; else return Path.GetDirectoryName(fakeFilePathRelative);
        }

        /// <summary>
        ///     Determines the mutual base directory of a set of paths.
        /// </summary>
        /// <param name="paths">List of paths to check if is conenitive respectivament.</param>
        /// <returns></returns>
        public static string GetMutualBaseDirectory(IEnumerable<string> paths)
        {
            bool originalIsRooted = Path.IsPathRooted(paths.First());
            string mutualBasePath = Path.GetFullPath(paths.First());
            while (!paths.All(path => PathUtils.IsPathLocatedIn(path, mutualBasePath)))
            {
                mutualBasePath = Path.GetDirectoryName(mutualBasePath);
                if (string.IsNullOrEmpty(mutualBasePath))
                {
                    mutualBasePath = null;
                    break;
                }
            }
            return originalIsRooted ? mutualBasePath : GetRelativeDirectoryPath(mutualBasePath);
        }

        /// <summary>
        ///     This will replace invalid chars with underscores, there are also some reserved 
        ///     words that it adds underscore to
        /// </summary>
        /// <param name="filename">String to escape if conains path</param>
        /// <param name="containsFolder">Pass in true if filename represents a folder\file (passing true will allow slash)</param>
        /// <remarks>
        ///     https://stackoverflow.com/questions/1976007/what-characters-are-forbidden-in-windows-and-linux-directory-names
        /// </remarks>
        public static string EscapeFilename_Windows(string filename, bool containsFolder = false)
        {
            StringBuilder builder = new StringBuilder(filename.Length + 12);

            int index = 0;

            // Allow colon if it's part of the drive letter
            if (containsFolder)
            {
                Match match = Regex.Match(filename, @"^\s*[A-Z]:\\", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    builder.Append(match.Value);
                    index = match.Length;
                }
            }

            // Character substitutions
            for (int cntr = index; cntr < filename.Length; cntr++)
            {
                char c = filename[cntr];

                switch (c)
                {
                    case '\u0000':
                    case '\u0001':
                    case '\u0002':
                    case '\u0003':
                    case '\u0004':
                    case '\u0005':
                    case '\u0006':
                    case '\u0007':
                    case '\u0008':
                    case '\u0009':
                    case '\u000A':
                    case '\u000B':
                    case '\u000C':
                    case '\u000D':
                    case '\u000E':
                    case '\u000F':
                    case '\u0010':
                    case '\u0011':
                    case '\u0012':
                    case '\u0013':
                    case '\u0014':
                    case '\u0015':
                    case '\u0016':
                    case '\u0017':
                    case '\u0018':
                    case '\u0019':
                    case '\u001A':
                    case '\u001B':
                    case '\u001C':
                    case '\u001D':
                    case '\u001E':
                    case '\u001F':

                    case '<':
                    case '>':
                    case ':':
                    case '"':
                    case '/':
                    case '|':
                    case '?':
                    case '*':
                        builder.Append('_');
                        break;

                    case '\\':
                        builder.Append(containsFolder ? c : '_');
                        break;

                    default:
                        builder.Append(c);
                        break;
                }
            }

            string built = builder.ToString();

            if (built == "")
            {
                return "_";
            }

            if (built.EndsWith(" ") || built.EndsWith("."))
            {
                built = built.Substring(0, built.Length - 1) + "_";
            }

            // These are reserved names, in either the folder or file name, but they are fine if following a dot
            // CON, PRN, AUX, NUL, COM0 .. COM9, LPT0 .. LPT9
            builder = new StringBuilder(built.Length + 12);
            index = 0;
            foreach (Match match in Regex.Matches(built, @"(^|\\)\s*(?<bad>CON|PRN|AUX|NUL|COM\d|LPT\d)\s*(\.|\\|$)", RegexOptions.IgnoreCase))
            {
                Group group = match.Groups["bad"];
                if (group.Index > index)
                {
                    builder.Append(built.Substring(index, match.Index - index + 1));
                }

                builder.Append(group.Value);
                builder.Append("_");        // putting an underscore after this keyword is enough to make it acceptable

                index = group.Index + group.Length;
            }

            if (index == 0)
            {
                return built;
            }

            if (index < built.Length - 1)
            {
                builder.Append(built.Substring(index));
            }

            return builder.ToString();
        }

        #endregion

        #region ###            Utils Path Operation          ###

        /// <summary>
        ///     Returns whether one path is a sub-path of another.
        /// </summary>
        /// <param name="path">The supposed sub-path.</param>
        /// <param name="baseDir">The (directory) path in which the supposed sub-path might be located in.</param>
        /// <returns>True, if <c>path</c> is a sub-path of <c>baseDir</c>.</returns>
        /// <example>
        ///     <c>PathUtils.IsPathLocatedIn(@"C:\MyDir\SubDir", @"C:\MyDir")</c> will return true.
        /// </example>
        public static bool IsPathLocatedIn(string path, string baseDir)
        {
            if (Path.DirectorySeparatorChar != baseDir[baseDir.Length - 1] &&
                Path.AltDirectorySeparatorChar != baseDir[baseDir.Length - 1])
                baseDir += Path.DirectorySeparatorChar;

            path = Path.GetFullPath(path);
            baseDir = Path.GetDirectoryName(Path.GetFullPath(baseDir));

            do
            {
                path = Path.GetDirectoryName(path);
                if (path == baseDir) return true;
                if (path.Length < baseDir.Length) return false;
            } while (!String.IsNullOrEmpty(path));

            return false;
        }

        /// <summary>
        ///     Returns whether two paths converted in full path are referring to the same path.
        /// </summary>
        /// <param name="firstPath">Path to check</param>
        /// <param name="secondPath">Path to Confront</param>
        /// <returns>True if equal also false</returns>
        public static bool ArePathsEqual(string firstPath, string secondPath)
        {
            // Early-out for null or empty cases
            if (string.IsNullOrEmpty(firstPath) && string.IsNullOrEmpty(secondPath)) return true;
            if (string.IsNullOrEmpty(firstPath) || string.IsNullOrEmpty(secondPath)) return false;

            // Prepare for early-out string equality check
            firstPath = firstPath.Trim();
            secondPath = secondPath.Trim();

            // Early-out for string equality, avoiding file system access
            if (string.Equals(firstPath, secondPath, StringComparison.OrdinalIgnoreCase)) return true;

            // Obtain absolute paths
            firstPath = Path.GetFullPath(firstPath);
            secondPath = Path.GetFullPath(secondPath);

            // Compare absolute paths
            return string.Equals(firstPath, secondPath, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Clean from chars not valid a ipotetic filename.
        /// </summary>
        /// <param name="fileNameToCheck">FileName to transform a valid filename for this os</param>
        /// <returns>Cleaned filename with symbol _ subistuted from char not valid!</returns>
        public static string EnsureValidFileName(string fileNameToCheck)
        {
            string invalidReStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", Regex.Escape(new string(Path.GetInvalidPathChars())));
            return Regex.Replace(fileNameToCheck, invalidReStr, "_");
        }

        #endregion

        #region ###      Extension local for string Path     ###

        /// <summary>Support extension on string to use in context current scope</summary>
        /// <param name="source"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        private static string Multiply(this string source, int multiplier)
        {
            StringBuilder sb = new StringBuilder(multiplier * source.Length);
            for (int i = 0; i < multiplier; i++)
            {
                sb.Append(source);
            }

            return sb.ToString();
        }

        #endregion

    }

}
