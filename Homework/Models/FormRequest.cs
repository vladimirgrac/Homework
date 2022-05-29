namespace Homework.Models
{
    /// <summary>
    /// Request form model
    /// </summary>
    public class FormRequest
    {
        public IFormFile File { get; set; }
        public string Url { get; set; }
        public string OutputFormat { get; set; }
    }
}
