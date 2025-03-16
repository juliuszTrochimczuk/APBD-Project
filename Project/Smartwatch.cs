using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Smartwatch : Device
    {
        private int BatteryLevel { get; set; }

        public Smartwatch(string id, string name, bool isTurnedOn, int batteryLevel) : base(id, name, isTurnedOn)
        {
            BatteryLevel = batteryLevel;
        }
    }
}
