using HelperFoundation.Exceptions;
using HelperFoundation.Validator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HelperFoundation.Exceptions.DirectoryException;

namespace HelperFoundation.FileDirectory
{
    public static class Helper
    {
        /// <summary>
        /// Helps in creating a directory if it does not exist or else will not do anything
        /// </summary>
        /// <param name="path">The full path of the directory</param>
        /// <returns>Error Handler object which will tell you if the operation succeeded or not. If not it gives the reason as a message</returns>        
        public static void CreatDirectoryIfNotExists(string path)
        {
            ParameterValidator.CheckParametersAreNull(path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Helps in moving a directory if it exists or else will not do anything
        /// </summary>
        /// <param name="sourcePath">The full path of the directory where it currently exists</param>
        /// <param name="targetPath">The full path of the directory where it has to be moved</param>
        /// <returns>Error Handler object which will tell you if the operation succeeded or not. If not it gives the reason as a message</returns>        
        public static void MoveDirectoryIfExists(string sourcePath, string targetPath)
        {
            ParameterValidator.CheckParametersAreNull(sourcePath, targetPath);
            if (Directory.Exists(sourcePath))
            {
                Directory.Move(sourcePath, targetPath);
            }
        }

        /// <summary>
        /// Helps in moving a file if it exists or else will not do anything
        /// </summary>
        /// <param name="fileName">The name of the file with extension to be moved</param>
        /// <param name="sourcePath">The full path of the directory where the file exists</param>
        /// <param name="targetPath">The full path of the directory where the file has to be moved</param>
        /// <param name="isForcedMove">Used to know if the file has to be moved even if the destination path does not exist.
        /// If destination path does not exist it is created.</param>
        /// <returns>Error Handler object which will tell you if the operation succeeded or not. If not it gives the reason as a message</returns>        
        public static void MoveFileIfExists(string fileName, string sourcePath, string targetPath, bool isForcedMove)
        {
            ParameterValidator.CheckParametersAreNull(fileName, sourcePath, targetPath);
            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryException(sourcePath);
            }
            var oldCombinedPath = Path.Combine(sourcePath, fileName);
            var newCombinedPath = Path.Combine(targetPath, fileName);
            if (!File.Exists(oldCombinedPath))
            {
                throw new FileException(fileName, sourcePath);
            }
            if (Directory.Exists(targetPath))
            {
                if (File.Exists(newCombinedPath))
                {
                    throw new DirectoryException(sourcePath, targetPath);
                }
                File.Move(sourcePath, targetPath);
            }
            else
            {
                if (isForcedMove)
                {
                    File.Move(sourcePath, targetPath);
                }
                else
                {
                    throw new DirectoryException(targetPath, ValidationType.DestinationPathDoesNotExist);
                }
            }
        }

        /// <summary>>
        /// Helps in moving a file if it exists or else will not do anything
        /// </summary>
        /// <param name="fileName">The name of the file with extension to be moved</param>
        /// <param name="sourcePath">The full path of the directory where the file exists</param>
        /// <param name="targetPath">The full path of the directory where the file has to be moved</param>
        public static void MoveFileIfExists(string fileName, string sourcePath, string targetPath)
        {
            MoveFileIfExists(fileName, sourcePath, targetPath, false);
        }

        /// <summary>
        /// Helps in deleting a directory if it exists or else will not do anything.
        /// All the subdirectories and files in the directory will also be deleted.
        /// </summary>
        /// <param name="path">The full path of the directory</param>
        /// <returns>Error Handler object which will tell you if the operation succeeded or not. If not it gives the reason as a message</returns>        
        public static void DeleteDirectoryIfExists(string path)
        {
            ParameterValidator.CheckParametersAreNull(path);
            if (!Directory.Exists(path))
            {
                throw new DirectoryException(path);
            }
            Directory.Delete(path, true);
        }

        /// <summary>
        /// Helps in deleting a directory if it exists or else will not do anything.
        /// If there are any subdirectory or file in the directory, the directory will not be deleted.
        /// </summary>
        /// <param name="path">The full path of the directory</param>
        /// <returns>Error Handler object which will tell you if the operation succeeded or not. If not it gives the reason as a message</returns>        
        public static void DeleteDirectoryIfEmpty(string path)
        {
            ParameterValidator.CheckParametersAreNull(path);
            if (!Directory.Exists(path))
                throw new DirectoryException(path);
            if (!Directory.EnumerateDirectories(path).Any() && !Directory.EnumerateFiles(path).Any())
            {
                Directory.Delete(path, false);
            }
        }

        /// <summary>
        /// Helps in deleting a file if it exists or else will not do anything.
        /// </summary>
        /// <param name="path">The full path of the directory where the file exists without the file name</param>
        /// <param name="fileName">The file name with the extension</param>
        /// <returns>Error Handler object which will tell you if the operation succeeded or not. If not it gives the reason as a message</returns>        
        public static void DeleteFileIfExists(string path, string fileName)
        {
            ParameterValidator.CheckParametersAreNull(path, fileName);
            if (!Directory.Exists(path))
            {
                throw new DirectoryException(path);
            }
            var combinedPath = Path.Combine(path, fileName);
            if (!File.Exists(combinedPath))
            {
                throw new FileException(fileName, path);
            }
            File.Delete(combinedPath);
        }

        /// <summary>
        /// Gets the list of all directories at a specified path
        /// </summary>
        /// <param name="path">The full path of the directory</param>
        /// <returns>Returns the list of all directory names at the specified path if any within a wrapper object.
        /// Access it using Result property of the object returned</returns>        
        public static List<string> GetAllDirectoriesFromPath(string path)
        {
            ParameterValidator.CheckParametersAreNull(path);
            if (!Directory.Exists(path))
            {
                throw new DirectoryException(path);
            }
            return Directory.EnumerateDirectories(path).ToList();
        }

        /// <summary>
        /// Gets the list of all files at a specified path
        /// </summary>
        /// <param name="path">The full path of the directory</param>
        /// <returns>Returns the list of all file names at the specified path if any within a wrapper object.
        /// Access it using Result property of the object returned</returns>        
        public static List<string> GetAllFilesFromPath(string path)
        {
            ParameterValidator.CheckParametersAreNull(path);
            if (!Directory.Exists(path))
            {
                throw new DirectoryException(path);
            }
            return Directory.EnumerateFiles(path).ToList();
        }

        /// <summary>
        /// Helps in searching directories with a particular searchtext at a specified path
        /// </summary>
        /// <param name="path">The full path of the directory to search sub directories</param>
        /// <param name="searchText">The keyword used to search files which has filename including the keyword</param>
        /// <param name="searchAsExact">To check if you want to search files with filenames which contains the search keyword or matches the exact keyword.
        /// If true, directory names that exactly matches the seaech keyword are returned or else the file names that contain the search keyword</param>
        /// <returns>Returns the list of matching directory names at the specified path if any within a wrapper object.
        /// Access it using Result property of the object returned</returns>        
        public static List<string> SearchDirectoriesFromPath(string path, string searchText = "", bool searchAsExact = false)
        {
            ParameterValidator.CheckParametersAreNull(path);
            if (searchText == "")
                return GetAllDirectoriesFromPath(path);
            if (!Directory.Exists(path))
            {
                throw new DirectoryException(path);
            }
            if (!searchAsExact)
                return Directory.EnumerateDirectories(path, "*" + searchText + "*").ToList();
            else
                return Directory.EnumerateDirectories(path, searchText).ToList();
        }

        /// <summary>
        /// Helps in searching files with a particular searchtext at a specified path
        /// </summary>
        /// <param name="path">The full path of the directory to search the files</param>
        /// <param name="searchText">The keyword used to search files which has filename including the keyword</param>
        /// <param name="searchAsExact">To check if you want to search files with filenames which contains the search keyword or matches the exact keyword
        /// If true, file names that exactly matches the seaech keyword are returned or else the file names that contain the search keyword</param>
        /// <returns>Returns the list of matching file names at the specified path if any within a wrapper object.
        /// Access it using Result property of the object returned.
        /// Error Handler object which will also tell you if the operation succeeded or not. If not it gives the reason as a message</returns>        
        public static List<string> SearchFilesFromPath(string path, string searchText = "", bool searchAsExact = false)
        {
            ParameterValidator.CheckParametersAreNull(path);
            if (searchText == "")
                return GetAllFilesFromPath(path);
            if (!Directory.Exists(path))
            {
                throw new DirectoryException(path);
            }
            if (!searchAsExact)
                return Directory.EnumerateFiles(path, "*" + searchText + "*").ToList();
            else
                return Directory.EnumerateFiles(path, searchText).ToList();
        }

        /// <summary>
        /// Helps to rename a directory is it exists
        /// </summary>
        /// <param name="path">The full path to the directory(excluding directory name)</param>
        /// <param name="oldName">Old name of the directory</param>
        /// <param name="newName">New name of the directory</param>
        /// <returns>Error Handler object which will tell you if the operation succeeded or not. If not it gives the reason as a message</returns>        
        public static void RenameDirectoryIfExists(string path, string oldName, string newName)
        {
            ParameterValidator.CheckParametersAreNull(path, oldName, newName);
            if (!Directory.Exists(path))
            {
                throw new DirectoryException(path);
            }
            var oldCombinedPath = Path.Combine(path, oldName);
            var newCombinedPath = Path.Combine(path, newName);
            if (!Directory.Exists(oldCombinedPath))
            {
                throw new DirectoryException(oldCombinedPath);
            }
            if (Directory.Exists(newCombinedPath))
            {
                throw new DirectoryException(newCombinedPath, ValidationType.DirectoryAlreadyExists);
            }
            Directory.Move(oldCombinedPath, newCombinedPath);
        }

        /// <summary>
        /// Helps to rename a file is it exists
        /// </summary>
        /// <param name="path">The full path to the file(excluding file name)</param>
        /// <param name="oldFileName">Old name of the file</param>
        /// <param name="newFileName">New name of the file</param>
        /// <returns>Error Handler object which will tell you if the operation succeeded or not. If not it gives the reason as a message</returns>        
        public static void RenameFileIfExists(string path, string oldFileName, string newFileName)
        {
            ParameterValidator.CheckParametersAreNull(path, oldFileName, newFileName);
            if (!Directory.Exists(path))
            {
                throw new DirectoryException(path);
            }
            var oldCombinedPath = Path.Combine(path, oldFileName);
            var newCombinedPath = Path.Combine(path, newFileName);
            if (!File.Exists(oldCombinedPath))
            {
                throw new FileException(oldFileName, path, ValidationType.FileDoesNotExist);
            }
            if (File.Exists(newCombinedPath))
            {
                throw new FileException(newFileName, path, ValidationType.FileAlreadyExists);
            }
            File.Move(oldCombinedPath, newCombinedPath);
        }
    }
}
