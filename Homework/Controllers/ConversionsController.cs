using Microsoft.AspNetCore.Mvc;
using Homework.Models;
using Homework.Services;
using System.Net.Mime;

namespace Homework.Controllers
{
    /// <summary>
    /// Controller that provides document format conversions
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ConversionsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IFileService _fileService;

        public ConversionsController(
            IDocumentService documentService,
            IFileService fileService)
        {
            _documentService = documentService;
            _fileService = fileService;
        }

        /// <summary>
        /// Convert given file to requested format
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet(Name = "ConvertFromFile")]
        public IActionResult ConvertFile([FromForm] FormRequest form)
        {
            var sourceDoc = _documentService.DocumentFromFile(form.File);
            var targetDoc = _documentService.ConvertDocument(sourceDoc, form.OutputFormat);

            return DoResponse(targetDoc);
        }

        /// <summary>
        /// Gets file from given URL and convert it to requested format
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet(Name ="ConvertFromUrl")]
        public IActionResult GetFromUrl([FromForm] FormRequest form)
        {
            var sourceDoc = _documentService.DocumentFromUrl(form.Url);
            var targetDoc = _documentService.ConvertDocument(sourceDoc, form.OutputFormat);

            return DoResponse(targetDoc);
        }

        /// <summary>
        /// Final step - send email and give file as response
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="sendEmail"></param>
        /// <returns></returns>
        private IActionResult DoResponse(Document doc)
        {
            var disposition = new ContentDisposition
            {
                FileName = doc.FileName,
                Inline = true
            };
            Response.Headers.Add("Content-disposition", disposition.ToString());

            // return file
            var fileStream = new FileStream(doc.FilePath, FileMode.Open);
            return File(fileStream, _fileService.GetContentType(doc.FilePath));
        }
    }
}
