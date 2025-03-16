using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class PersonalComputer : Device
    {
        private string OperatingSystem {  get; set; }

        public PersonalComputer(string id, string name, bool isTurnedOn, string operatingSystem) : base(id, name, isTurnedOn)
        {
            OperatingSystem = operatingSystem;
        }
    }
}
