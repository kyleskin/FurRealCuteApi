namespace FurRealCute.Web.Api.Models.Pets.Exceptions;

public class PetValidationException : Exception
{
    public PetValidationException(Exception innerException)
        : base(message: "Invalid input, contact support.", innerException) { }
}