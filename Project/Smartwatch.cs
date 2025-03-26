using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Smartwatch : Device, IPowerNotifier
    {
        private int _batteryLevel;
        public int BatteryLevel
        {
            get => _batteryLevel;
            set
            {
                if (value < 0)
                    value = 0;
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

        public void Notify() => Console.WriteLine("Battery is too low");

        public override void TurnOn()
        {
            if (_batteryLevel == 0)
                throw new EmptyBatteryException();
            base.TurnOn();
            BatteryLevel -= 10;
        }

        public override string ToString() => Id + "," + Name + "," + IsTurnedOn + "," + BatteryLevel + "%";
    }

    public class EmptyBatteryException : Exception
    {
        public EmptyBatteryException() : base("Can't turned on Smartwatch with 0% battery") { }
    }
}
