namespace FurRealCute.Web.Api.Models.Pets.Exceptions;

public class NotFoundPetException : Exception
{
    public NotFoundPetException(Guid petId)
        : base(message: $"Couldn't find pet with id: { petId }.") { }
}