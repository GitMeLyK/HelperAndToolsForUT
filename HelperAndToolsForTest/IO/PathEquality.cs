using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HelperAndToolsForTest.IO
{
    /// <summary>
    ///     Determines equality of path strings.
    /// </summary>
    public class PathEqualityComparer : IEqualityComparer<string>
    {
        /// <summary>Equality comparer implementation for two string rappresentative for path</summary>
        /// <param name="x">Path A</param>
        /// <param name="y">Path B</param>
        /// <returns>True if equal</returns>
        public bool Equals(string x, string y)
        {
            return PathUtils.ArePathsEqual(x, y);
        }

        /// <summary>Hashcode For equalit implementation comparer</summary>
        /// <param name="obj"></param>
        /// <returns>Hash of equailty object value</returns>
        public int GetHashCode(string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
                return 0;
            else
                return Path.GetFullPath(obj).GetHashCode();
        }
    }

}
