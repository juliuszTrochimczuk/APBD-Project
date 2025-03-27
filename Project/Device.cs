using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    /// <summary>
    /// Representation of device
    /// </summary>
    public abstract class Device
    {
        /// <summary>
        /// Id of the device
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the device
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// State of the device
        /// </summary>
        public bool IsTurnedOn { get; set; }

        protected Device(string id, string name, bool isTurnedOn)
        {
            this.Id = id;
            this.Name = name;
            this.IsTurnedOn = isTurnedOn;
        }

        /// <summary>
        /// Turns on the device.
        /// </summary>
        public virtual void TurnOn() => IsTurnedOn = true;

        /// <summary>
        /// Turns off the device
        /// </summary>
        public virtual void TurnOff() => IsTurnedOn = false;
    }
}
