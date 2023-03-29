using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Type = FurRealCute.Web.Api.Models.Pets.Type;

namespace FurRealCute.Web.Api.Services.Pets;

public partial class PetService
{
    private void ValidatePetOnCreate(Pet? pet)
    {
        ValidatePetIsNotNull(pet);
        ValidatePetId(pet!.Id);
        ValidatePetRequiredFields(pet);
        ValidatePetAuditFieldsOnCreate(pet);
    }
    
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

    private static void ValidatePetRequiredFields(Pet pet)
    {
        switch (pet)
        {
            case { } when IsInvalid(pet.Name):
                throw new InvalidPetException(
                    parameterName: nameof(Pet.Name),
                    parameterValue: pet.Name);
            case { } when IsInvalid(pet.Birthdate):
                throw new InvalidPetException(
                    parameterName: nameof(Pet.Birthdate),
                    parameterValue: pet.Birthdate);
            case { } when IsInvalid(pet.Type):
                throw new InvalidPetException(
                    parameterName: nameof(Pet.Type),
                    parameterValue: pet.Type);
            case { } when IsInvalid(pet.Size):
                throw new InvalidPetException(
                    parameterName: nameof(Pet.Size),
                    parameterValue: pet.Size);
        }
    }

    private static void ValidatePetAuditFieldsOnCreate(Pet pet)
    {
        switch (pet)
        {
            case { } when IsInvalid(pet.CreatedDate):
                throw new InvalidPetException(
                    parameterName: nameof(Pet.CreatedDate),
                    parameterValue: pet.CreatedDate);
            case { } when IsInvalid(pet.UpdatedDate):
                throw new InvalidPetException(
                    parameterName: nameof(Pet.UpdatedDate),
                    parameterValue: pet.UpdatedDate);
        }
    }
    
    private static bool IsInvalid(Guid petId) => petId == Guid.Empty;
    private static bool IsInvalid(string input) => string.IsNullOrWhiteSpace(input);

    private static bool IsInvalid(DateTimeOffset dateTime)
    {
        return dateTime > DateTimeOffset.UtcNow
            || dateTime == default;
    }

    private static bool IsInvalid(Type type) => type == default;
    private static bool IsInvalid(Size size) => size == default;
}