using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;

namespace FurRealCute.Web.Api.Services.Pets;

public partial class PetService
{
    private void ValidatePetOnCreate(Pet? pet)
    {
        ValidatePetIsNotNull(pet);
        ValidatePetId(pet!.Id);
    }

    private static bool IsInvalid(Guid petId) => petId == Guid.Empty;

    private static void ValidatePetIsNotNull(Pet? pet)
    {
        if (pet is null)
            throw new NullPetException();
    }

    private static void ValidatePetId(Guid petId)
    {
        if (IsInvalid(petId))
        {
            throw new InvalidPetException(
                parameterName: nameof(Pet.Id),
                parameterValue: petId);
        }
    }
}