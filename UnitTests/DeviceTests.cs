using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Devices;

namespace UnitTests
{
    public class DeviceTests
    {
        [Fact]
        public void TestTurningOnSmartwatchWithBattery()
        {
            Smartwatch smartwatch = new("P-1", "IPhone", false, 80);
            smartwatch.TurnOn();
            Assert.True(smartwatch.IsTurnedOn);
        }

        [Fact]
        public void TestTurningOnSmartwatchWithoutBattery()
        {
            Smartwatch smartwatch = new("P-1", "IPhone", false, 15);
            try
            {
                smartwatch.TurnOn();
            }
            catch (EmptyBatteryException)
            {
                Assert.False(smartwatch.IsTurnedOn);
            }
        }

        [Fact]
        public void TestTurningOnComputerWithOS()
        {
            PersonalComputer pc = new("P-1", "Gaming", false, "Windows");
            pc.TurnOn();
            Assert.True(pc.IsTurnedOn);
        }

        [Fact]
        public void TestTurningOnComputerWithoutOS()
        {
            PersonalComputer pc = new("P-1", "Gaming", false, "");
            try
            {
                pc.TurnOn();
            }
            catch (EmptySystemException)
            {
                Assert.False(pc.IsTurnedOn);
            }
        }

        [Fact]
        public void TestEmbeddedDeviceCreationWithWrongParameters()
        {
            EmbeddedDevice device = null;
            try
            {
                device = new("ED-3","Pi4", false,"whatisIP","MyWifiName");
            }
            catch (ArgumentException)
            {
                Assert.Null(device);
            }
        }

        [Fact]
        public void TestEmbeddedDeviceCreationWitGoodParameters()
        {
            EmbeddedDevice device = null;
            try
            {
                device = new("ED-1", "Pi3", true,"192.168.1.44", "MD Ltd.Wifi - 1");
            }
            catch (ArgumentException)
            {
                Assert.NotNull(device);
            }
        }

        [Fact]
        public void TestTurningOnEmbeddedDeviceWithCorrectNetworkName()
        {
            EmbeddedDevice device = new("ED-1", "Pi3", false, "192.168.1.44", "MD Ltd.Wifi - 1");
            device.TurnOn();
            Assert.True(device.IsTurnedOn);
        }

        [Fact]
        public void TestTurningOnEmbeddedDeviceWithWrongNetworkName()
        {
            EmbeddedDevice device = new("ED-1", "Pi3", false, "192.168.1.44", "Wifi");
            try
            {
                device.TurnOn();
            }
            catch (ConnectionException)
            {
                Assert.False(device.IsTurnedOn);
            }
        }
    }
}
