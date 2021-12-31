using System;
using System.IO;

namespace HelperAndToolsForUT.Helper.Utils
{
    /// <summary>
    ///     IO Utils
    /// </summary>
    public static class ContextIOSystem
    {

        /// <summary>
        ///     Check in file phrase 
        /// </summary>
        public static bool CheckKeywordsExist(string filePath, params string[] keywords)
        {
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    var content = sr.ReadToEnd();
                    foreach (var item in keywords)
                    {
                        if (!content.Contains(item))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }
    }

}
