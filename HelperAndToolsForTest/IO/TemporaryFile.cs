using HelperAndToolsForUT.Helper.Extensions.IOExtensions;
using System;
using System.IO;
using System.Text;

using HelperAndToolsForUT.Helper.Extensions.IOExtensions;

namespace HelperAndToolsForTest.IO
{

    /// <summary>
    ///     Helper to support creation of file temporary and dispose automaic.
    /// </summary>
    /// <example>
    ///     using(var file = new TemporaryFile())
    ///     {
    ///         Assume.That(file.FileInfo.Length, Is.EqualTo(0))
    ///     
    ///         File.WriteAllText(file, "write text after");
    ///     
    ///         file.FileInfo.Refresh();
    ///         Assert.That(file.FileInfo.Length, Is.GreaterThan(0));
    ///     }    
    /// </example>
    public sealed class TemporaryFile : IDisposable
    {
        private readonly FileInfo file;
        private bool _NotDelete = false;

        /// <summary>
        ///     Return context of file temprary
        /// </summary>
        public FileInfo FileInfo { get { return file; } } 

        #region ###     .ctors      ###

        /// <summary>Empty random file in folder temporary</summary>
        /// <param name="useSomePath">Use a some Path of this process in execution well to be call this util</param>
        /// <param name="overwriteIfExist">If exist detination file temporary, overwrite if exist</param>
        public TemporaryFile(bool useSomePath = false, bool overwriteIfExist = false) 
            : this(new FileInfo( System.IO.Path.GetRandomFileName()), useSomePath, overwriteIfExist) { }

        /// <summary>Empty file (named) in folder temporary</summary>
        /// <param name="useSomePath">Use a Path of relative fileName used in argumentation if exist directory well to contai also used only for name destination temp</param>
        /// <param name="overwriteIfExist">If exist detination file temporary, overwrite if exist</param>
        /// <param name="fileName">Name of File temporary to create</param>
        public TemporaryFile(string fileName, bool useSomePath = false, bool overwriteIfExist = false) : this(useSomePath, overwriteIfExist, fileName) { }

        /// <summary>Empty file (object file <see cref="FileInfo"/>) in folder temporary</summary>
        /// <param name="useSomePath">Use a Path of relative fileName used in argumentation if exist directory well to contain also used only for name destination temp</param>
        /// <param name="overwriteIfExist">If exist detination file temporary, overwrite if exist</param>
        /// <param name="temporaryFile">Object File to create</param>
        public TemporaryFile(FileInfo temporaryFile, bool useSomePath = false, bool overwriteIfExist = false) : this(useSomePath, overwriteIfExist, temporaryFile.FullName) { }

        /// <summary>File (with contents writed)  in folder temporary</summary>
        /// <param name="useSomePath">Use a some Path of this process in execution well to be call this util</param>
        /// <param name="overwriteIfExist">If exist detination file temporary, overwrite if exist</param>
        /// <param name="initialFileContents">Content stream to write destination temp file</param>
        public TemporaryFile(Stream initialFileContents, bool useSomePath = false, bool overwriteIfExist = false) : this(useSomePath,overwriteIfExist)
        {
            using (var file = new FileStream(this, FileMode.Open))
                initialFileContents.CopyTo(file);
        }

        /// <summary>File named (with contents writed)  in folder temporary</summary>
        /// <param name="useSomePath">Use a some Path of this process in execution well to be call this util</param>
        /// <param name="overwriteIfExist">If exist detination file temporary, overwrite if exist</param>
        /// <param name="fileName">Name of File temporary to create</param>
        /// <param name="initialFileContents">Content stream to write destination temp file</param>
        public TemporaryFile(string fileName, Stream initialFileContents, bool useSomePath = false, bool overwriteIfExist = false) : this(fileName,useSomePath,overwriteIfExist)
        {
            using (var file = new FileStream(this, FileMode.Open))
                initialFileContents.CopyTo(file);
        }

        /// <summary>File object (with contents writed)  in folder temporary</summary>
        /// <param name="useSomePath">Use a some Path of this process in execution well to be call this util</param>
        /// <param name="overwriteIfExist">If exist detination file temporary, overwrite if exist</param>
        /// <param name="temporaryFile">Object File to create</param>
        /// <param name="initialFileContents">Content stream to write destination temp file</param>
        public TemporaryFile(FileInfo temporaryFile, Stream initialFileContents, bool useSomePath = false, bool overwriteIfExist = false) : this(temporaryFile,useSomePath,overwriteIfExist)
        {
            using (var file = new FileStream(this, FileMode.Open))
                initialFileContents.CopyTo(file);
        }

        /// <summary>File (with contents writed)  in folder temporary</summary>
        /// <param name="fileContents">Content textual to write destination temp file</param>
        /// <param name="encoding">Encode of Text to use for write - Default UTF8</param>
        /// <param name="useSomePath">Use a some Path of this process in execution well to be call this util</param>
        /// <param name="overwriteIfExist">If exist detination file temporary, overwrite if exist</param>
        public TemporaryFile(string fileContents, Encoding encoding , bool useSomePath = false, bool overwriteIfExist = false) : this(useSomePath,overwriteIfExist)
        {
            CreateTextContetFile(fileContents, encoding);
        }

        /// <summary>File named (with contents writed)  in folder temporary</summary>
        /// <param name="fileName">Name of File temporary to create</param>
        /// <param name="fileContents">Content textual to write destination temp file</param>
        /// <param name="encoding">Encode of Text to use for write - Default UTF8</param>
        /// <param name="useSomePath">Use a some Path of this process in execution well to be call this util</param>
        /// <param name="overwriteIfExist">If exist detination file temporary, overwrite if exist</param>
        public TemporaryFile(string fileName, string fileContents, Encoding encoding , bool useSomePath = false, bool overwriteIfExist = false) : this(fileName,useSomePath,overwriteIfExist)
        {
            CreateTextContetFile(fileContents, encoding);
        }

        /// <summary>File Object (with contents writed)  in folder temporary</summary>
        /// <param name="temporaryFile">Object File to create</param>
        /// <param name="fileContents">Content textual to write destination temp file</param>
        /// <param name="encoding">Encode of Text to use for write - Default UTF8</param>
        /// <param name="useSomePath">Use a some Path of this process in execution well to be call this util</param>
        /// <param name="overwriteIfExist">If exist detination file temporary, overwrite if exist</param>
        public TemporaryFile(FileInfo temporaryFile, string fileContents, Encoding encoding , bool useSomePath = false, bool overwriteIfExist = false) : this(temporaryFile,useSomePath,overwriteIfExist)
        {
            CreateTextContetFile(fileContents, encoding);
        }

        /// <summary>
        ///     Check if a Path is a Directory on file system or file or not valid
        /// </summary>
        /// <param name="path">Path to check this</param>
        /// <returns>True = Is Directory, False Is File, Null Is Not Valid!</returns>
        public static bool? IsDirectory(string path)
        {
            if (Directory.Exists(path)) return true; // is a directory
            else if (File.Exists(path)) return false; // is a file
            else return null; // is a nothing
        }

        // ...Main...
        private TemporaryFile(bool useSomePath, bool overwriteIfExist, string pathFileForPartialName = null)
        {
            string tmpPath = null;
            FileInfo fileInfo = null;
            FileInfo tmpFileInfo = null;
            bool PathDirExist = false;
            bool PathFileExist = false;

            if (pathFileForPartialName != null)
            {

                try
                {
                    fileInfo = new FileInfo(pathFileForPartialName);

                    if (IsDirectory(fileInfo.FullName) == true)
                        PathDirExist = fileInfo.Directory.Exists;
                    else if (IsDirectory(fileInfo.FullName) == false)
                    {
                        PathDirExist = fileInfo.Directory.Exists;
                        PathFileExist = fileInfo.Exists;
                    }
                    else {
                        //
                        if (IsDirectory(fileInfo.Directory.FullName) == true)
                            PathDirExist = fileInfo.Directory.Exists;
                        //
                        PathFileExist = false;
                    }

                }
                catch (Exception) { };

                try
                {
                    tmpFileInfo = new FileInfo(Path.GetTempFileName());
                    if (fileInfo != null && useSomePath)
                    {

                        if (PathDirExist && PathFileExist)
                        {
                            tmpPath = Path.Combine(fileInfo.Directory.FullName, fileInfo.Name + "_" + tmpFileInfo.Name, tmpFileInfo.Extension);
                        }
                        else if (PathDirExist && !PathFileExist)
                        {
                            tmpPath = Path.Combine(fileInfo.Directory.FullName, fileInfo.GetFileNameWithoutExtension() + "_" + tmpFileInfo.GetFileNameWithoutExtension()+ fileInfo.Extension);
                        }
                    }
                    else if (fileInfo != null && !useSomePath) {
                        if(PathDirExist)
                            tmpPath = Path.Combine(tmpFileInfo.Directory.FullName, fileInfo.GetFileNameWithoutExtension() + "_" + tmpFileInfo.GetFileNameWithoutExtension() + fileInfo.Extension);
                        else
                            tmpPath = Path.Combine(tmpFileInfo.Directory.FullName, fileInfo.GetFileNameWithoutExtension() + "_" + tmpFileInfo.GetFileNameWithoutExtension() + fileInfo.Extension);
                    }
                    else {
                        tmpPath = tmpFileInfo.FullName;
                    }
                    //
                    File.Delete(tmpFileInfo.FullName);
                    File.Create(tmpPath).Close();
                    tmpFileInfo = null;
                    file = new FileInfo(tmpPath);
                }
                catch (Exception) {
                    file = null;
                }

            }
        }

        private void CreateTextContetFile(string fileContents, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            File.WriteAllText(this.FileInfo.FullName, fileContents, encoding);
        }

        /// <summary></summary>
        public void NotDelete()
        {
            _NotDelete = true;
        }

        #endregion

        #region ###     Operators   ###
        /// <summary></summary>
        /// <param name="temporaryFile"></param>
        public static implicit operator FileInfo(TemporaryFile temporaryFile)
        {
            return temporaryFile.file;
        }
        /// <summary></summary>
        /// <param name="temporaryFile"></param>
        public static implicit operator string(TemporaryFile temporaryFile)
        {
            return temporaryFile.file.FullName;
        }
        /// <summary></summary>
        /// <param name="temporaryFile"></param>
        public static explicit operator TemporaryFile(FileInfo temporaryFile)
        {
            return new TemporaryFile(temporaryFile);
        }

        #endregion

        #region ### ~ ###

        private volatile bool disposed;

        /// <summary></summary>
        public void Dispose()
        {
            try
            {
                if (!_NotDelete)
                    file.Delete();
                disposed = true;
            }
            catch (Exception) { } // Ignore
        }

        /// <summary></summary>
        ~TemporaryFile()
        {
            if (!disposed) Dispose();
        }

        #endregion
    }

}
