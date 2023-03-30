namespace FurRealCute.Web.Api.Models.Pets.Exceptions;

public class PetServiceException : Exception
{
    public PetServiceException(Exception innerException)
        : base(message: "Service error occurred, contact support.", innerException) { }
}