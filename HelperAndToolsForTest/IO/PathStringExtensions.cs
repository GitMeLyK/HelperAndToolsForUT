using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HelperAndToolsForTest.Helper.Extensions.IOExtensions
{
    /// <summary>
    ///     Simplify for FileInfo to get FileName withoust extension.
    /// </summary>
    public static class StringPathExtensions
    {

        /// <summary>
        ///     Remove from string as use for path in context from chars not conform
        ///     a windows system.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="removeSpecialLettersHavingASign"></param>
        /// <returns></returns>
        public static string CleanForUsePathWindows(this string expression, bool removeSpecialLettersHavingASign = true)
        {
            var newCharacterWithSpace = " ";
            var newCharacter = "";

            // Return carriage handling
            // ASCII LINE-FEED character (LF),
            expression = expression.Replace("\n", newCharacterWithSpace);
            // ASCII CARRIAGE-RETURN character (CR) 
            expression = expression.Replace("\r", newCharacterWithSpace);

            // less than : used to redirect input, allowed in Unix filenames, see Note 1
            expression = expression.Replace(@"<", newCharacter);
            // greater than : used to redirect output, allowed in Unix filenames, see Note 1
            expression = expression.Replace(@">", newCharacter);
            // colon: used to determine the mount point / drive on Windows; 
            // used to determine the virtual device or physical device such as a drive on AmigaOS, RT-11 and VMS; 
            // used as a pathname separator in classic Mac OS. Doubled after a name on VMS, 
            // indicates the DECnet nodename (equivalent to a NetBIOS (Windows networking) hostname preceded by "\\".). 
            // Colon is also used in Windows to separate an alternative data stream from the main file.
            expression = expression.Replace(@":", newCharacter);
            // quote : used to mark beginning and end of filenames containing spaces in Windows, see Note 1
            expression = expression.Replace(@"""", newCharacter);
            // slash : used as a path name component separator in Unix-like, Windows, and Amiga systems. 
            // (The MS-DOS command.com shell would consume it as a switch character, but Windows itself always accepts it as a separator.[16][vague])
            expression = expression.Replace(@"/", newCharacter);
            // backslash : Also used as a path name component separator in MS-DOS, OS/2 and Windows (where there are few differences between slash and backslash); allowed in Unix filenames, see Note 1
            expression = expression.Replace(@"\", newCharacter);
            // vertical bar or pipe : designates software pipelining in Unix and Windows; allowed in Unix filenames, see Note 1
            expression = expression.Replace(@"|", newCharacter);
            // question mark : used as a wildcard in Unix, Windows and AmigaOS; marks a single character. Allowed in Unix filenames, see Note 1
            expression = expression.Replace(@"?", newCharacter);
            expression = expression.Replace(@"!", newCharacter);
            // asterisk or star : used as a wildcard in Unix, MS-DOS, RT-11, VMS and Windows. Marks any sequence of characters 
            // (Unix, Windows, later versions of MS-DOS) or any sequence of characters in either the basename or extension 
            // (thus "*.*" in early versions of MS-DOS means "all files". Allowed in Unix filenames, see note 1
            expression = expression.Replace(@"*", newCharacter);
            // percent : used as a wildcard in RT-11; marks a single character.
            expression = expression.Replace(@"%", newCharacter);
            // period or dot : allowed but the last occurrence will be interpreted to be the extension separator in VMS, MS-DOS and Windows. 
            // In other OSes, usually considered as part of the filename, and more than one period (full stop) may be allowed. 
            // In Unix, a leading period means the file or folder is normally hidden.
            expression = expression.Replace(@".", newCharacter);
            // space : allowed (apart MS-DOS) but the space is also used as a parameter separator in command line applications. 
            // This can be solved by quoting, but typing quotes around the name every time is inconvenient.
            //expression = expression.Replace(@"%", " ");
            expression = expression.Replace(@"  ", newCharacter);

            if (removeSpecialLettersHavingASign)
            {
                // Because then issues to zip
                // More at : http://www.thesauruslex.com/typo/eng/enghtml.htm
                expression = expression.Replace(@"ê", "e");
                expression = expression.Replace(@"ë", "e");
                expression = expression.Replace(@"ï", "i");
                expression = expression.Replace(@"œ", "oe");
            }

            return expression;
        }

    }



}
