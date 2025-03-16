using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public abstract class Device
    {
        protected string Id { get; set; }
        protected string Name { get; set; }
        protected bool IsTurnedOn { get; set; }

        protected Device(string id, string name, bool isTurnedOn)
        {
            this.Id = id;
            this.Name = name;
            this.IsTurnedOn = isTurnedOn;
        }

        public virtual void TurnOn() => IsTurnedOn = true;

        public virtual void TurnOff() => IsTurnedOn = false;
    }
}
