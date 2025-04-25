using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Controllers.FileControllers;
using Controllers.Parsers;
using Devices;

namespace Controllers
{
    /// <summary>
    /// Class dedicated to manage all of the devices
    /// </summary>
    public class DeviceManager
    {
        private FileController fileController;
        private IParser parser;
        private List<Device> allDevices = new();
        public List<Device> AllDevices => allDevices;

        public DeviceManager(FileController fileController, IParser parser)
        {
            this.fileController = fileController;
        }

        public string GetDeviceData()
        {
            string response = "";
            foreach (Device device in allDevices)
            {
                response += device.ToString() + "\n";
            }
            return response;
        }

        public string GetDeviceData(int index)
        {
            if (index >= 0 && index <= allDevices.Count)
                return allDevices[index].ToString();
            return "Object not found";
        }

        /// <summary>
        /// Adding new device to DeviceManager
        /// </summary>
        public void AddDevice(Device newDevice) => TryAddingDevice(newDevice);

        /// <summary>
        /// Using parser to create new device from string
        /// </summary>
        /// <param name="specification">Raw specification of device (like with input data)</param>
        public bool TryGetDeviceFromText(string specification, out Device parsedDevice)
        {
            parser.TryParsing(specification, out parsedDevice);
            return parsedDevice != null;
        }


        /// <summary>
        /// Remove device from DeviceManager
        /// </summary>
        public bool TryRemoveDevice(Device newDevice)
        {
            if (allDevices.Contains(newDevice))
            {
                allDevices.Remove(newDevice);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove device from DeviceManager
        /// </summary>
        /// <param name="deviceIndex">Index of device that it's stored in list</param>
        public bool TryRemoveDevice(int deviceIndex)
        {
            try
            {
                allDevices.RemoveAt(deviceIndex);
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

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