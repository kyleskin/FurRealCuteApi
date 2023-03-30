namespace FurRealCute.Web.Api.Models.Pets.Exceptions;

public class PetDependencyException : Exception
{
    public PetDependencyException(Exception innerException)
        : base(message: "Service dependency error occurred, contact support.", innerException) { }
}