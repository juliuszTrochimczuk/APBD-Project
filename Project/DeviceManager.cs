using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class DeviceManager
    {
        private List<Device> allDevices = new();
        public List<Device> AllDevices => allDevices;

        private readonly string filePath;

        public DeviceManager() { }

        public DeviceManager(string filePath) 
        {
            this.filePath = filePath;
            Console.WriteLine(filePath);

            if (!File.Exists(filePath)) 
                throw new FileNotFoundException();

            foreach (string line in File.ReadLines(filePath))
            {
                try 
                {

                    Device device = CreateDeviceBasedOnText(line);
                    if (device != null)
                        allDevices.Add(device);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void AddDevice(Device newDevice)
        {
            if (allDevices.Count == 15)
                return;
            allDevices.Add(newDevice);
        }

        public void AddDevice(string specification)
        {
            if (allDevices.Count == 15)
                return;
            Device device = CreateDeviceBasedOnText(specification);
            if (device != null)
                allDevices.Add(device);
        }

        public void RemoveDevice(Device newDevice) => allDevices.Remove(newDevice);

        public void RemoveDevice(int deviceIndex) => allDevices.RemoveAt(deviceIndex);

        //WHAT THE HELL IT SUPPOSED TO DO
        public void EditDeviceData(int deviceIndex, Device template)
        {
            if (template is Smartwatch sw)
            {
                Smartwatch targetSW = (Smartwatch)allDevices[deviceIndex];
                targetSW.BatteryLevel = sw.BatteryLevel;
            }
            else if (template is PersonalComputer pc)
            {
                PersonalComputer targetPC = (PersonalComputer)allDevices[deviceIndex];
                targetPC.OperatingSystem = pc.OperatingSystem;
            }
            else if (template is EmbeddedDevice ed)
            {
                EmbeddedDevice targetED = (EmbeddedDevice)allDevices[deviceIndex];
                targetED.IpAdress = ed.IpAdress;
                targetED.NetworkName = ed.NetworkName;
            }
            allDevices[deviceIndex].Id = template.Id;
            allDevices[deviceIndex].Name = template.Name;
            allDevices[deviceIndex].IsTurnedOn = template.IsTurnedOn;
        }

        public void TurnOnDevice(int deviceIndex)
        {
            try
            {
                allDevices[deviceIndex].TurnOn();
            } 
            catch (EmptyBatteryException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (EmptySystemException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ConnectionException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void TurnOffDevice(int deviceIndex) => allDevices[deviceIndex].TurnOff();

        public void ShowAllDevices()
        {
            foreach (Device device in allDevices)
                Console.WriteLine(device.ToString());
        }

        public void SaveDevicesToFile()
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            string messageToWrite = "";
            foreach (Device device in allDevices)
                messageToWrite += device.ToString() + "\n";
            File.WriteAllText(filePath, messageToWrite);
        }

        public Device CreateDeviceBasedOnText(string text)
        {
            Device device = null;
            string[] values = text.Split(',');
            if (values[0].Contains("SW"))
            {
                values[3] = values[3].Remove(values[3].Length - 1);
                device = new Smartwatch(values[0], values[1], values[2] == "true", int.Parse(values[3]));
            }
            else if (values[0].Contains('P'))
            {
                string operatingSystem;
                try
                {
                    operatingSystem = values[3];
                }
                catch (IndexOutOfRangeException)
                {
                    operatingSystem = "";
                }
                device = new PersonalComputer(values[0], values[1], values[2] == "true", operatingSystem);
            }
            else if (values[0].Contains("ED"))
                device = new EmbeddedDevice(values[0], values[1], values[2], values[3]);

            return device;
        }
    }
}
