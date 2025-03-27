using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project
{
    /// <summary>
    /// Representation of embdedded device as a device
    /// </summary>
    public class EmbeddedDevice : Device
    {
        /// <summary>
        /// Regex used to check if IP is in IPv4
        /// </summary>
        private Regex regex = new("[0-9]{0,3}[.][0-9]{0,3}[.][0-9]{0,3}");

        private string ipAdress;

        public string IpAdress
        {
            get => ipAdress;
            set
            {
                if (regex.IsMatch(value))
                    ipAdress = value;
                else
                    throw new WrongIPExcpection();
            }
        }

        public string NetworkName { get; set; }

        public EmbeddedDevice(string id, string name, bool isTurnedOn, string ipAdress, string networkName) : base(id, name, isTurnedOn)
        {
            IpAdress = ipAdress;
            NetworkName = networkName;
        }
        
        /// <inheritdoc/>
        public override void TurnOn()
        {
            Connect();
            base.TurnOn();
        }

        public override string ToString() => Id + "," + Name + "," + IsTurnedOn + ',' + IpAdress + "," + NetworkName;

        private void Connect()
        {
            if (!NetworkName.Contains("MD Ltd."))
                throw new ConnectionException();
        }
    }

    /// <summary>
    /// Exceptions shows that Network name is set in wrong way
    /// </summary>
    public class ConnectionException : Exception 
    {
        public ConnectionException() : base("Network name is wrong") { }
    }

    /// <summary>
    /// Exception shows that IP is not in IPv4 standard
    /// </summary>
    public class WrongIPExcpection : Exception
    {
        public WrongIPExcpection() : base("The given IP is not IPv4 Standard") { }
    }
}
