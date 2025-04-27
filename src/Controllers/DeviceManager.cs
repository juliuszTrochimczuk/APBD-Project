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
        private readonly FileController fileController;
        private readonly IParser parser;
        private readonly IDatabaseHandler? databaseHandler;
        private readonly List<Device> allDevices = new();

        public DeviceManager(FileController fileController, IParser parser, IDatabaseHandler? databaseHandler)
        {
            this.fileController = fileController;
            this.databaseHandler = databaseHandler;
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

        public string? GetDeviceData(string id)
        {
            Device? foundDevice = allDevices.Find((device) => device.Id == id);
            if (foundDevice == null)
                return null;
            return foundDevice.ToString();
        }

        /// <summary>
        /// Adding new device to DeviceManager
        /// </summary>
        public void AddDevice(Device newDevice) => TryAddingDevice(newDevice);

        /// <summary>
        /// Adding new device to DeviceManager
        /// </summary>
        /// <param name="saveToDB">Check to specifi when you need explicity make insert statement</param>
        public void AddDevice(Device newDevice, bool saveToDB)
        {
            TryAddingDevice(newDevice);
            if (saveToDB && databaseHandler != null)
                databaseHandler.AddDevice(newDevice);
        }

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
        /// <param name="deviceId">Index of device that it's stored in list</param>
        public bool TryRemoveDevice(string deviceId)
        {
            Device? foundDevice = allDevices.Find((device) => device.Id ==  deviceId);
            if (foundDevice == null) 
                return false;
            allDevices.Remove(foundDevice);
            if (databaseHandler != null)
                databaseHandler.DeleteDevice(foundDevice);
            return true;
        }

        /// <summary>
        /// Method used to edit device that is stored in DeviceManager
        /// </summary>
        /// <param name="deviceId">Index of device that it's stored in list</param>
        /// <param name="template">New Device that contains all of new informations to edit</param>
        public bool EditDeviceData(string deviceId, Device template)
        {
            Device? foundDevice = allDevices.Find((device) => device.Id == deviceId);
            if (foundDevice == null)
                return false;

            if (template is Smartwatch sw)
            {
                Smartwatch targetSW = (Smartwatch)foundDevice;
                targetSW.BatteryLevel = sw.BatteryLevel;
            }
            else if (template is PersonalComputer pc)
            {
                PersonalComputer targetPC = (PersonalComputer)foundDevice;
                targetPC.OperatingSystem = pc.OperatingSystem;
            }
            else if (template is EmbeddedDevice ed)
            {
                EmbeddedDevice targetED = (EmbeddedDevice)foundDevice;
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
            foundDevice.Name = template.Name;
            foundDevice.IsTurnedOn = template.IsTurnedOn;

            if (databaseHandler != null)
                databaseHandler.UpdateDevice(foundDevice);
            return true;
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