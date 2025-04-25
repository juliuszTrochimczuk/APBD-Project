using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices
{
    /// <summary>
    /// Representation of smartwatch as a device
    /// </summary>
    public class Smartwatch : Device, IPowerNotifier
    {
        private int _batteryLevel;

        /// <summary>
        /// Battery level property. Clamps potential value [0, 100]
        /// </summary>
        public int BatteryLevel
        {
            get => _batteryLevel;
            set
            {
                if (value < 0)
                    value = 0;
                if (value < 20)
                    Notify();
                else if (value > 100)
                    value = 100;
                _batteryLevel = value;
            }
        }

        public Smartwatch(string id, string name, bool isTurnedOn, int batteryLevel) : base(id, name, isTurnedOn)
        {
            if (batteryLevel < 20) 
                Notify();
            BatteryLevel = batteryLevel;
        }

        /// <inheritdoc/>
        public void Notify() => Console.WriteLine("Battery is too low");

        /// <inheritdoc/>
        public override void TurnOn()
        {
            if (_batteryLevel == 0)
                throw new EmptyBatteryException();
            base.TurnOn();
            BatteryLevel -= 10;
        }

        public override string ToString() => Id + "," + Name + "," + IsTurnedOn + "," + BatteryLevel + "%";
    }


    /// <summary>
    /// Exception shows low battery status
    /// </summary>
    public class EmptyBatteryException : Exception
    {
        public EmptyBatteryException() : base("Can't turned on Smartwatch with 0% battery") { }
    }
}
