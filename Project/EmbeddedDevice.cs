using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project
{
    public class EmbeddedDevice : Device
    {
        private Regex regex;

        private string ipAdress;
        public string IpAdress
        {
            get => ipAdress;
            set
            {
                if (regex.IsMatch(value))
                    ipAdress = value;
                else
                    throw new ArgumentException();
            }
        }
        public string NetworkName { get; set; }

        public EmbeddedDevice(string id, string name, string ipAdress, string networkName) : base(id, name, false)
        {
            //Regex expression for IPv4
            regex = new("[0-9]{0,3}[.][0-9]{0,3}[.][0-9]{0,3}");

            IpAdress = ipAdress;
            NetworkName = networkName;
        }

        public override void TurnOn()
        {
            Connect();
            base.TurnOn();
        }

        public override string ToString()
        {
            return Id + "," + Name + "," + IpAdress + "," + NetworkName;
        }

        private void Connect()
        {
            if (!NetworkName.Contains("MD Ltd."))
                throw new ConnectionException();
        }
    }

    public class ConnectionException : Exception 
    {
        public ConnectionException() : base("Network name is wrong") { }
    }
}
