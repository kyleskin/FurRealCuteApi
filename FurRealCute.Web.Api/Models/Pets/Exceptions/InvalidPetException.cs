namespace FurRealCute.Web.Api.Models.Pets.Exceptions;

public class InvalidPetException : Exception
{
    public InvalidPetException(string parameterName, object parameterValue)
        : base(message: $"Invalid pet, " +
                        $"parameter name: {parameterName}, " +
                        $"parameter value: {parameterValue}.") 
    { }
}