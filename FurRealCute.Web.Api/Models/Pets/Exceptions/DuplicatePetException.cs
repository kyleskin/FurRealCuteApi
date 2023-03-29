namespace FurRealCute.Web.Api.Models.Pets.Exceptions;

public class DuplicatePetException : Exception
{
    public DuplicatePetException(Exception innerException) 
        : base(message: "Pet already exists in the database.", innerException: innerException) { }
}