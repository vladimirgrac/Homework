using Microsoft.Extensions.Options;
using Homework.Converters;
using Homework.Models;

namespace Homework.Services
{
    /// <summary>
    /// Document service interface
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Returns <see cref="Document"/> from given form file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Document DocumentFromFile(IFormFile file);

        /// <summary>
        /// Returns <see cref="Document"/> from given URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Document DocumentFromUrl(string url);

        /// <summary>
        /// Converts <see cref="Document"/> to target format
        /// </summary>
        /// <param name="document"></param>
        /// <param name="targetFormat"></param>
        /// <returns></returns>
        Document ConvertDocument(Document document, string targetFormat);
    }

    public class DocumentService : IDocumentService
    {
        private readonly StorageSettings _storageSettings;
        private readonly IFileService _fileService;

        public DocumentService(IOptions<StorageSettings> storageSettings, IFileService fileService)
        {
            _storageSettings = storageSettings.Value;
            _fileService = fileService;
        }

        /// <summary>
        /// Returns file format converter
        /// </summary>
        /// <param name="targetFormat"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private IFormatBase GetConverter(string targetFormat)
        {
            return targetFormat switch
            {
                "json" => new JsonFormat(),
                "xml" => new XmlFormat(),
                _ => throw new NotSupportedException()
            };
        }

        /// <summary>
        /// <inheritdoc cref="IDocumentService"/>
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public Document DocumentFromFile(IFormFile file)
        {
            // create directory if not exist
            var baseDir = _fileService.GetDirectoryFullPath(_storageSettings.SourceDir);

            var filePath = Path.Combine(baseDir, file.FileName);

            // upload to server
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            // read content
            var data = _fileService.LoadFromDisk(filePath);

            return new Document
            {
                Data = data,
                FilePath = filePath,
                FileName = file.FileName,
                Format = GetConverter(file.FileName.Substring(file.FileName.IndexOf('.') + 1)),
            };
        }

        /// <summary>
        /// <inheritdoc cref="IDocumentService"/>
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Document DocumentFromUrl(string url)
        {
            // request client
            using var client = new HttpClient();
            var result = client.GetAsync(url).GetAwaiter().GetResult();
            var fileMediaType = result.Content.Headers.ContentType?.MediaType ?? "";

            // get file content
            using var stream = result.Content.ReadAsStream();
            using var streamReader = new StreamReader(stream);
            var content = streamReader.ReadToEnd();

            return new Document
            {
                Data = content,
                Format = GetConverter(fileMediaType),
            };
        }

        /// <summary>
        /// <inheritdoc cref="IDocumentService"/>
        /// </summary>
        /// <param name="document"></param>
        /// <param name="targetFormat"></param>
        /// <returns></returns>
        public Document ConvertDocument(Document document, string targetFormat)
        {
            var content = document.Format.Deserialize(document.Data);

            // create directory if not exist
            var baseDir = _fileService.GetDirectoryFullPath(_storageSettings.DestinationDir);
            var outputFileName = $"{document.FileName}.{targetFormat}";
            var outputFilePath = Path.Combine(baseDir, outputFileName);

            var converter = GetConverter(targetFormat);
            var outputData = converter.Serialize(content);

            _fileService.SaveToDisk(outputFilePath, outputData);

            return new Document
            {
                Data = outputData,
                FilePath = outputFilePath,
                FileName = outputFileName,
            };
        }
    }
}
