using Controllers.FileControllers;
using Controllers.Parsers;
using Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    /// <summary>
    /// Global factory class
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// This method is responsable of creating the Device Manager
        /// </summary>
        /// <param name="fileController">Instance of file Controller that Device Manager will be using</param>
        /// <returns></returns>
        public static DeviceManager CreateDeviceManager(FileController fileController, IParser parser)
        {
            DeviceManager deviceManager = new(fileController, parser);
            for (int i = 0; i < fileController.FileLinesCount(); i++)
            {
                try
                {
                    if (!fileController.GetFileLine(i, out string deviceSpecification))
                        continue;
                    if (!parser.TryParsing(deviceSpecification, out Device device))
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
}
