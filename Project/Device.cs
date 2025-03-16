using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public abstract class Device
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsTurnedOn { get; set; }

        public Device(string id, string name, bool isTurnedOn)
        {
            this.Id = id;
            this.Name = name;
            this.IsTurnedOn = isTurnedOn;
        }

        public virtual void TurnOn() => IsTurnedOn = true;

        public virtual void TurnOff() => IsTurnedOn = false;
    }
}
