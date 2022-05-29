namespace Homework.Converters
{
    /// <summary>
    /// JSON format converter
    /// </summary>
    public class JsonFormat : IFormatBase
    {
        /// <summary>
        /// JSON to JSON
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Deserialize(string content)
        {
            return content;
        }

        /// <summary>
        /// JSON to JSON
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Serialize(string content)
        {
            return content;
        }
    }
}
