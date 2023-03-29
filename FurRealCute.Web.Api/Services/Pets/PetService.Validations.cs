using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;

namespace FurRealCute.Web.Api.Services.Pets;

public partial class PetService
{
    private void ValidatePetOnCreate(Pet? pet)
    {
        ValidatePetIsNotNull(pet);
    }

    private static void ValidatePetIsNotNull(Pet? pet)
    {
        if (pet is null)
            throw new NullPetException();
    }
}