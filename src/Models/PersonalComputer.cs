using Microsoft.IdentityModel.Tokens;

namespace Models;

public class PersonalComputer(string id, string name, bool isTurnedOn, string? operationSystem) : Device(id, name, isTurnedOn)
{
    public string? OperationSystem { get; set; } = operationSystem;
    
    public override void OnTurnOn()
    {
        if (OperationSystem.IsNullOrEmpty())
            throw new EmptySystemException();
    }

    public override void OnTurnOff()
    {
        
    }

    public override string ToString()
    {
        return base.ToString() + "; Operation System: " + OperationSystem;
    }
}

/// <summary>
/// Exception shows the lack of the operating system in PC
/// </summary>
public class EmptySystemException : Exception
{
    public EmptySystemException() : base("Can't launch PC that dosen't have operating system") { }
}