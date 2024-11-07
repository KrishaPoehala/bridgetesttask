namespace Bridge.Application.Common.Exceptions;

public class NonExistentSortAttribute : ApplicationException
{
    public NonExistentSortAttribute(string value): base($"Sorting attribute with value {value} does not exist")
    {
    }
}
