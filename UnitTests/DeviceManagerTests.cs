using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controllers;

namespace UnitTests
{
    public class DeviceManagerTests
    {
        const string filePath = "..\\..\\..\\test_input.txt";

        [Fact]
        public void TestCreationOfDeviceTest()
        {
            string filePath = "..\\..\\..\\test_input.txt";
            DeviceManager deviceManager = DeviceManager.Factory.CreateDeviceManager(filePath);
            Assert.True(deviceManager.AllDevices.Count() == 5);
        }
    }
}
