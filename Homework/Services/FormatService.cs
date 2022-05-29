using Homework.Converters;

namespace Homework.Services
{
    public interface IFormatService
    {
        /// <summary>
        /// Returns file format converter
        /// </summary>
        /// <param name="fileFormat"></param>
        /// <returns></returns>
        IFormatBase GetFormat(string fileFormat);
    }
    public class FormatService : IFormatService
    {
        /// <summary>
        /// Returns file format converter
        /// </summary>
        /// <param name="fileFormat"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public IFormatBase GetFormat(string fileFormat)
        {
            return fileFormat switch
            {
                "json" => new JsonFormat(),
                "xml" => new XmlFormat(),
                _ => throw new NotSupportedException()
            };
        }
    }
}
