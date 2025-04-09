using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Devices;

namespace Controllers
{
    /// <summary>
    /// Class dedicated to manage all of the devices
    /// </summary>
    public class DeviceManager
    {

        /// <summary>
        /// Class dedicated to create new instance of DeviceManager
        /// </summary>
        public class Factory
        {
            /// <summary>
            /// This method creates the instance of DeviceManager
            /// </summary>
            /// <param name="filePath">Path to the file where devices are stored</param>
            /// <returns>Instance of DeviceManager with loaded devices</returns>
            public static DeviceManager CreateDeviceManager(string filePath)
            {
                TxtFileController fileController = new TxtFileController(filePath);
                DeviceManager deviceManager = new(fileController);
                for (int i = 0; i < fileController.FileLinesCount(); i++)
                {
                    try
                    {
                        if (!fileController.GetFileLine(i, out string deviceSpecification))
                            continue;
                        if (!deviceManager.TryCreatingDeviceBasedOnText(deviceSpecification, out Device device))
                            continue;
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

        private TxtFileController fileController;
        private List<Device> allDevices = new();
        public List<Device> AllDevices => allDevices;

        private DeviceManager(TxtFileController fileController) => this.fileController = fileController;

        /// <summary>
        /// Adding new device to DeviceManager
        /// </summary>
        public void AddDevice(Device newDevice) => TryAddingDevice(newDevice);


        /// <summary>
        /// Adding new device to DeviceManager
        /// </summary>
        /// <param name="specification">Raw specification of device (like with input data)</param>
        public void AddDevice(string specification)
        {
            if (TryCreatingDeviceBasedOnText(specification, out Device device))
                TryAddingDevice(device);
        }


        /// <summary>
        /// Remove device from DeviceManager
        /// </summary>
        public void RemoveDevice(Device newDevice) => allDevices.Remove(newDevice);

        /// <summary>
        /// Remove device from DeviceManager
        /// </summary>
        /// <param name="deviceIndex">Index of device that it's stored in list</param>
        public void RemoveDevice(int deviceIndex) => allDevices.RemoveAt(deviceIndex);

        /// <summary>
        /// Method used to edit device that is stored in DeviceManager
        /// </summary>
        /// <param name="deviceIndex">Index of device that it's stored in list</param>
        /// <param name="template">New Device that contains all of new informations to edit</param>
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

        /// <summary>
        /// Turn on the device. Also catch all of the errors that might appear.
        /// </summary>
        /// <param name="deviceIndex">Index of the device that it's stored in the list</param>
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

        /// <summary>
        /// Turn of the device
        /// </summary>
        /// <param name="deviceIndex">Index of the device that it's stored in the list</param>
        public void TurnOffDevice(int deviceIndex) => allDevices[deviceIndex].TurnOff();

        /// <summary>
        /// Show all of the devices that DeviceManager contains
        /// </summary>
        public void ShowAllDevices()
        {
            foreach (Device device in allDevices)
                Console.WriteLine(device.ToString());
        }

        /// <summary>
        /// Save all of the connected devices to the file (the same one as the date was read from)
        /// </summary>
        public void SaveDevicesToFile()
        {
            string messageToWrite = "";
            foreach (Device device in allDevices)
                messageToWrite += device.ToString() + "\n";
            fileController.SaveToFile(messageToWrite);
        }

        /// <summary>
        /// Create a Device based on text specification
        /// </summary>
        /// <param name="text">Text specification of the device</param>
        /// <param name="createdDevice">Created device</param>
        /// <returns>Returns bool that shows if device was created successfuly</returns>
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

        /// <summary>
        /// Add new device to the list
        /// </summary>
        /// <returns>Returns bool that shows if you successfuly added the device</returns>
        private bool TryAddingDevice(Device deviceToAdd)
        {
            if (allDevices.Count == 15)
                return false;
            allDevices.Add(deviceToAdd);
            return true;
        }
    }
}