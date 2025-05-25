namespace Models;

public abstract class Device(string id, string name, bool isTurnedOn)
{
    public string Id { get; set; } = id;

    public string Name { get; set; } = name;

    public bool IsTurnedOn { get; set; } = isTurnedOn;

    public void TurnOn()
    {
        OnTurnOn();
        IsTurnedOn = true;
    }

    public void TurnOff()
    {
        OnTurnOff();
        IsTurnedOn = false;
    }
    
    public abstract void OnTurnOn();
    
    public abstract void OnTurnOff();

    public override string ToString()
    {
        return "Id : " + Id + "; Name : " + Name;
    }
}
