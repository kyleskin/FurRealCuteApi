namespace FurRealCute.Web.Api.Models.Pets.Exceptions;

public class FailedPetServiceException : Exception
{
    public FailedPetServiceException(Exception innerException)
        : base(message: "Failed pet service error occurred, contact support.", innerException) { }
}