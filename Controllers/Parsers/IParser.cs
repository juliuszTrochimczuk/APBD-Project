using Devices;

namespace Controllers.Parsers
{
    public interface IParser
    {
        /// <summary>
        /// Create a Device based on specification
        /// </summary>
        /// <param name="text">Text specification of the device</param>
        /// <param name="createdDevice">Created device</param>
        /// <returns>Returns bool that shows if device was created successfuly</returns>
        public bool TryParsing(object input, out Device parsedDevice);
    }
}
