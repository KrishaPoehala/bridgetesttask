namespace Bridge.Application.Common.Exceptions;

public class DogsNameAlreadyExists : ApplicationException
{
    public DogsNameAlreadyExists(string name) : base($"A dog with a name {name} already exists")
    {
    }
}
