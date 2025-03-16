using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class DeviceManager
    {
        private List<Device> allDevices = new();

        public DeviceManager(string filePath) 
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException();

            foreach (string line in File.ReadLines(filePath))
            {
                string[] values = line.Split(',');
                Device deviceToCreate;
                if (values[0].Contains("SW") && (values[2] == "true" || values[2] == "false"))
                {
                    values[3] = values[3].Remove(values[3].Length - 1);
                    int batteryLevel = int.Parse(values[3]);
                    bool isSwitchOn = values[2] == "true";
                    deviceToCreate = new Smartwatch(values[0], values[1], isSwitchOn, batteryLevel);
                }
                else if (values[0].Contains('P') && (values[2] == "true" || values[2] == "false"))
                {
                    bool isSwitchOn = values[2] == "true";
                    string operatingSystem;
                    try
                    {
                        operatingSystem = values[3];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        operatingSystem = "";
                    }
                    deviceToCreate = new PersonalComputer(values[0], values[1], isSwitchOn, operatingSystem);
                }
                else if (values[0].Contains("ED"))
                    deviceToCreate = new EmbeddedDevice(values[0], values[1], values[2], values[3]);
                else
                    continue;
                
                allDevices.Add(deviceToCreate);
                Console.WriteLine("[{0}]", string.Join(", ", values));
            }
        }
    }
}
