using Homework.Converters;

namespace Homework.Models
{
    /// <summary>
    /// Internal (converted) document representation
    /// </summary>
    public class Document
    {
        public string Data { get; set; }
        //public DocumentFormat Format { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public IFormatBase Format { get; set; }
    }
}
