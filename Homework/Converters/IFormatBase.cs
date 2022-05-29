namespace Homework.Converters
{
    /// <summary>
    /// Defines common methods for format custom converters
    /// </summary>
    public interface IFormatBase
    {
        /// <summary>
        /// Serialize object to requested format
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        string Serialize(string content);

        /// <summary>
        /// Deserialize object to 'base' format
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        string Deserialize(string content);
    }
}
