namespace Homework.Models
{
    /// <summary>
    /// Email attachment model
    /// </summary>
    public class Attachment
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
    }
}
