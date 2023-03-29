namespace FurRealCute.Web.Api.Models.Pets.Exceptions;

public class NullPetException : Exception
{
    public NullPetException() : base(message: "The pet is null.") { }
}