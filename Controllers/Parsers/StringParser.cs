using Devices;

namespace Controllers.Parsers
{
    public class StringParser : IParser
    {
        public bool TryParsing(object input, out Device parsedDevice)
        {
            parsedDevice = null;
            string text = "";
            try
            {
                text = input as string;
            } 
            catch
            {
                Console.WriteLine("Given input is not of type string");
                return false;
            }
            string[] values = text.Split(',');

            if (bool.TryParse(values[2], out bool isTurnedOn) is false)
                return false;

            if (values[0].StartsWith("SW-"))
            {
                if (values.Length > 4)
                    return false;

                values[3] = values[3].Remove(values[3].Length - 1);
                parsedDevice = new Smartwatch(values[0], values[1], isTurnedOn, int.Parse(values[3]));
                return true;
            }
            else if (values[0].StartsWith("P-"))
            {
                if (values.Length > 4)
                    return false;

                string operatingSystem;
                try
                {
                    operatingSystem = values[3];
                }
                catch (IndexOutOfRangeException)
                {
                    return false;
                }
                parsedDevice = new PersonalComputer(values[0], values[1], isTurnedOn, operatingSystem);
                return true;
            }
            else if (values[0].StartsWith("ED-"))
            {
                if (values.Length > 5)
                    return false;

                try
                {
                    parsedDevice = new EmbeddedDevice(values[0], values[1], isTurnedOn, values[3], values[4]);
                    return true;
                }
                catch (WrongIPExcpection ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return false;
        }
    }
}
