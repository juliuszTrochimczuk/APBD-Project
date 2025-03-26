using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project
{
    public class DeviceManager
    {
        public class Factory
        {
            public static DeviceManager CreateDeviceManager(string filePath)
            {
                FileController fileController = new FileController(filePath);
                DeviceManager deviceManager = new(fileController);
                for (int i = 0; i < fileController.FileLinesCount(); i++)
                {
                    try
                    {
                        if (deviceManager.TryCreatingDeviceBasedOnText(fileController.GetFileLine(i), out Device device))
                            deviceManager.AddDevice(device);
                    }
                    catch (WrongIPExcpection ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return deviceManager;
            }
        }

        private FileController fileController;
        private List<Device> allDevices = new();
        public List<Device> AllDevices => allDevices;

        private DeviceManager(FileController fileController) => this.fileController = fileController;

        public void AddDevice(Device newDevice) => TryAddingDevice(newDevice);

        public void AddDevice(string specification)
        {
            if (TryCreatingDeviceBasedOnText(specification, out Device device))
                TryAddingDevice(device);
        }

        public void RemoveDevice(Device newDevice) => allDevices.Remove(newDevice);

        public void RemoveDevice(int deviceIndex) => allDevices.RemoveAt(deviceIndex);

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
                try
                {
                    targetED.IpAdress = ed.IpAdress;
                }
                catch (WrongIPExcpection ex)
                {
                    Console.WriteLine(ex.Message);
                }
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
            string messageToWrite = "";
            foreach (Device device in allDevices)
                messageToWrite += device.ToString() + "\n";
            fileController.SaveToFile(messageToWrite);
        }

        private bool TryCreatingDeviceBasedOnText(string text, out Device createdDevice)
        {
            createdDevice = null;
            string[] values = text.Split(',');

            if (bool.TryParse(values[2], out bool isTurnedOn) is false)
                return false;

            if (values[0].StartsWith("SW-"))
            {
                if (values.Length > 4)
                    return false;

                values[3] = values[3].Remove(values[3].Length - 1);
                createdDevice = new Smartwatch(values[0], values[1], isTurnedOn, int.Parse(values[3]));
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
                createdDevice = new PersonalComputer(values[0], values[1], isTurnedOn, operatingSystem);
                return true;
            }
            else if (values[0].StartsWith("ED-"))
            {
                if (values.Length > 5)
                    return false;

                try
                {
                    createdDevice = new EmbeddedDevice(values[0], values[1], isTurnedOn, values[3], values[4]);
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

        private bool TryAddingDevice(Device deviceToAdd)
        {
            if (allDevices.Count == 15)
                return false;
            allDevices.Add(deviceToAdd);
            return true;
        }
    }
}