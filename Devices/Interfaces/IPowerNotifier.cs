using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices
{
    /// <summary>
    /// Interface used to display power notifier 
    /// </summary>
    public interface IPowerNotifier
    {
        /// <summary>
        /// Notification about lack of power
        /// </summary>
        public void Notify();
    }
}
