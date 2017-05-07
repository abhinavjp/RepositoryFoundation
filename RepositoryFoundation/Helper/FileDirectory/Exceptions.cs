using HelperFoundation.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperFoundation.Exceptions
{
    public enum ValidationType
    {
        FileAlreadyExists,
        FileDoesNotExist,
        FileDoesNotExistAtSourcePath,
        DirectoryDoesNotExist,
        DestinationPathDoesNotExist,
        DirectoryAlreadyExists
    }

    public class DirectoryException: ValidationException
    {

        public DirectoryException(string sourcePath, string targetPath)
            : this(sourcePath, targetPath, ValidationType.FileAlreadyExists)
        {

        }
        public DirectoryException(string sourcePath, string targetPath, 
            ValidationType validationType)
            :base($"Cannot move file from {sourcePath} to {targetPath}. {FileDirectoryExceptionHelper.GetErrorMessage(validationType)}")
        {

        }

        public DirectoryException(string directoryPath) : this(directoryPath, ValidationType.DirectoryDoesNotExist)
        {

        }

        public DirectoryException(string directoryPath, ValidationType validationType): 
            base($"{directoryPath}: {FileDirectoryExceptionHelper.GetErrorMessage(validationType)}")
        {

        }
    }

    public class FileException : ValidationException
    {

        public FileException(string fileName, string sourcePath): base($"{fileName} in {sourcePath}. {FileDirectoryExceptionHelper.GetErrorMessage(ValidationType.FileDoesNotExistAtSourcePath)}")
        {

        }

        public FileException(string fileName, string sourcePath, ValidationType validationType) : base($"{fileName} in {sourcePath}. {FileDirectoryExceptionHelper.GetErrorMessage(validationType)}")
        {

        }

    }

    internal static class FileDirectoryExceptionHelper
    {
        internal static string GetErrorMessage(ValidationType validationType)
        {
            switch (validationType)
            {
                case ValidationType.FileAlreadyExists:
                    return "A file with same name already exists.";
                case ValidationType.DirectoryDoesNotExist:
                    return "The directory path does not exist.";
                case ValidationType.DestinationPathDoesNotExist:
                    return "The destination path does not exist. Please use 'forced move' to move irrelvant to the existence of destination path.";
                case ValidationType.FileDoesNotExist:
                    return "The file at given path does not exist.";
                case ValidationType.FileDoesNotExistAtSourcePath:
                    return "File does not exist at the source path.";
                case ValidationType.DirectoryAlreadyExists:
                    return "Directory already exists.";
                default:
                    return "File Directory Operation Failed.";
            }
        }
    }
}
