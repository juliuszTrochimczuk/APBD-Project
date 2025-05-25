using System;
using System.Collections.Generic;

namespace Models;

public class Smartwatch(string id, string name, bool isTurnedOn, int batteryPercentage) : Device(id, name, isTurnedOn), IPowerNotifier
{
    private int _batteryPercentage = batteryPercentage;
    
    public int BatteryPercentage
    {
        get => _batteryPercentage;
        set
        {
            if (value < 0)
                value = 0;
            if (value < 20)
                Notify();
            else if (value > 100)
                value = 100;
            _batteryPercentage = value;
        }
    }

    public override void OnTurnOn()
    {
        if (BatteryPercentage == 0)
            throw new EmptyBatteryException();
        BatteryPercentage -= 10;
    }

    public override void OnTurnOff()
    {
        
    }
    
    public void Notify() => Console.WriteLine("Battery is too low");

    public override string ToString() => base.ToString() + "; Battery Percentage: " + BatteryPercentage.ToString() + "%";
}

/// <summary>
/// Exception shows low battery status
/// </summary>
public class EmptyBatteryException : Exception
{
    public EmptyBatteryException() : base("Can't turned on Smartwatch with 0% battery") { }
}