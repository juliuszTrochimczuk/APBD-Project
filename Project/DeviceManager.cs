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
                if (values[0].Contains("SW") && (values[2] == "true" || values[2] == "false"))
                {
                    values[3] = values[3].Remove(values[3].Length - 1);
                    int batteryLevel = int.Parse(values[3]);
                    bool isSwitchOn = values[2] == "true";
                    Smartwatch smartwatch = new Smartwatch(values[0], values[1], isSwitchOn, batteryLevel);
                    allDevices.Add(smartwatch);
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
                    PersonalComputer pc = new PersonalComputer(values[0], values[1], isSwitchOn, operatingSystem);
                    allDevices.Add(pc);
                }
                else if (values[0].Contains("ED"))
                {
                    try
                    {
                        EmbeddedDevice embedded = new EmbeddedDevice(values[0], values[1], values[2], values[3]);
                        allDevices.Add(embedded);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                    continue;
            }
        }

        public void AddDevice(Device newDevice)
        {
            if (allDevices.Count == 15)
                return;
            allDevices.Add(newDevice);
        }

        public void RemoveDevice(Device newDevice) => allDevices.Remove(newDevice);

        public void RemoveDevice(int deviceIndex) => allDevices.RemoveAt(deviceIndex);

        //WHAT THE HELL IT SUPPOSED TO DO
        public void EditDeviceData()
        {

        }

        public void TurnOnDevice(int deviceIndex) => allDevices[deviceIndex].TurnOn();

        public void TurnOffDevice(int deviceIndex) => allDevices[deviceIndex].TurnOff();

        public void ShowAllDevices()
        {
            foreach (Device device in allDevices)
                Console.WriteLine(device.ToString());
        }
    }
}
