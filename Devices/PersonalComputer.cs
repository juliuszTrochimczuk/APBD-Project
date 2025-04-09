using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices
{
    /// <summary>
    /// Represnation of PC as a device
    /// </summary>
    public class PersonalComputer : Device
    {
        /// <summary>
        /// Name of the operating system
        /// </summary>
        public string OperatingSystem {  get; set; }

        public PersonalComputer(string id, string name, bool isTurnedOn, string operatingSystem) : base(id, name, isTurnedOn)
        {
            OperatingSystem = operatingSystem;
        }

        /// <inheritdoc/>
        public override void TurnOn()
        {
            if (OperatingSystem == null || OperatingSystem == "")
                throw new EmptySystemException();
            base.TurnOn();
        }

        public override string ToString() => Id + "," + Name + "," + IsTurnedOn + "," + OperatingSystem;
    }

    /// <summary>
    /// Exception shows the lack of the operating system in PC
    /// </summary>
    public class EmptySystemException : Exception
    {
        public EmptySystemException() : base("Can't launch PC that dosen't have operating system") { }
    }
}
