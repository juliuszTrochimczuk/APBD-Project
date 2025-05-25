using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Models;

public class EmbeddedDevice(string id, string name, bool isTurnedOn, string ipAdress, string networkName) : Device(id, name, isTurnedOn)
{
    //This is regex for IPv4
    private Regex regex = new("[0-9]{0,3}[.][0-9]{0,3}[.][0-9]{0,3}");
    
    private string  _ipAdress = ipAdress;

    public string IpAddress
    {
        get => _ipAdress;
        set
        {
            if (regex.IsMatch(value))
                _ipAdress = value;
            else
                throw new WrongIPExcpection();
        }
    }
    
    public string NetworkName { get; set; } = networkName;

    public override void OnTurnOn()
    {
        if (!NetworkName.Contains("MD Ltd."))
            throw new ConnectionException();
    }

    public override void OnTurnOff()
    {
        
    }

    public override string ToString() => base.ToString() + "; Ip Address: " + IpAddress + "; Network Name: " + NetworkName;
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