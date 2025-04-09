using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controllers;
using Controllers.FileControllers;
using Controllers.Parsers;

namespace UnitTests
{
    public class DeviceManagerTests
    {
        [Fact]
        public void TestCreationOfDeviceTest()
        {
            string filePath = "..\\..\\..\\test_input.txt";
            DeviceManager deviceManager = Factory.CreateDeviceManager(new TxtFileController(filePath), new StringParser());
            Assert.True(deviceManager.AllDevices.Count() == 5);
        }
    }
}
