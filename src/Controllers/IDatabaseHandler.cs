using Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public interface IDatabaseHandler
    {
        public void UpdateDevice(Device deviceToUpdate);

        public void DeleteDevice(Device deviceToDelete);

        public void AddDevice(Device deviceToAdd);
    }
}
