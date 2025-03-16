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
        private int BatteryLevel
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
            {
                Notify();
                return;
            }
            BatteryLevel = batteryLevel;
        }

        public void Notify() => Console.WriteLine("Battery is too low");

        public override void TurnOn()
        {
            try
            {
                if (_batteryLevel == 0)
                    throw new EmptyBatteryException();
                base.TurnOn();
                BatteryLevel -= 10;
            }
            catch (EmptyBatteryException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public override string ToString()
        {
            return "Id: " + Id + "; Name: " + Name + "; Is turned on: " + IsTurnedOn + "; Battery level: " + BatteryLevel + "%";
        }
    }

    public class EmptyBatteryException : Exception
    {
        public EmptyBatteryException() : base("Can't turned on Smartwatch with 0% battery") { }
    }
}
