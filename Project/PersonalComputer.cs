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

        public override void TurnOn()
        {
            if (OperatingSystem == null || OperatingSystem == "")
                throw new EmptySystemException();
            base.TurnOn();
        }

        public override string ToString()
        {
            return Id + "," + Name + "," + IsTurnedOn + "," + OperatingSystem;
        }
    }

    public class EmptySystemException : Exception
    {
        public EmptySystemException() : base("Can't launch PC that dosen't have operating system") { }
    }
}
