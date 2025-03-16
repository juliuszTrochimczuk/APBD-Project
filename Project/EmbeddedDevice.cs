using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class EmbeddedDevice : Device
    {
        public EmbeddedDevice(string id, string name, bool isTurnedOn) : base(id, name, isTurnedOn)
        {
        }
    }
}
