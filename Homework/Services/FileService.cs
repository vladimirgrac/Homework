using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Homework.Models;

namespace Homework.Services
{
    public interface IFileService
    {
        /// <summary>
        /// Load file from disk
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string LoadFromDisk(string filePath);

        /// <summary>
        /// Save file to disk
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="doc"></param>
        void SaveToDisk(string filePath, string? doc);

        /// <summary>
        /// Get absolute directory path
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        string GetDirectoryFullPath(string dirPath);

        /// <summary>
        /// Try to get file content type
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string GetContentType(string filePath);
    }


    public class FileService : IFileService
    {
        private readonly StorageSettings _storageSettings;

        public FileService(IOptions<StorageSettings> storageSettings)
        {
            _storageSettings = storageSettings.Value;
        }

        /// <summary>
        /// <inheritdoc cref="IFileService"/>
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetContentType(string filePath)
        {
            return new FileExtensionContentTypeProvider().TryGetContentType(filePath, out string? contentType) ? contentType : "";
        }

        /// <summary>
        /// <inheritdoc cref="IFileService"/>
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        public string GetDirectoryFullPath(string directoryName)
        {
            var rootPath = Path.Combine(Environment.CurrentDirectory, _storageSettings.BaseDir);
            var absoluteDirPath = Path.Combine(rootPath, directoryName);
            if (!Directory.Exists(absoluteDirPath))
            {
                // create new directory
                Directory.CreateDirectory(absoluteDirPath);
            }

            return absoluteDirPath;
        }

        /// <summary>
        /// <inheritdoc cref="IFileService"/>
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string LoadFromDisk(string filePath)
        {
            using var file = File.Open(filePath, FileMode.Open);
            using var reader = new StreamReader(file);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// <inheritdoc cref="IFileService"/>
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="doc"></param>
        public void SaveToDisk(string filePath, string? doc)
        {
            var file = File.Open(filePath, FileMode.Create, FileAccess.Write);
            using var sw = new StreamWriter(file);
            sw.Write(doc);
        }
    }
}
