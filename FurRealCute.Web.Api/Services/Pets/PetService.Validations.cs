using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;

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

    private void ValidatePetOnModify(Pet? pet)
    {
        ValidatePetIsNotNull(pet);
        ValidatePetId(pet!.Id);
        ValidatePetRequiredFields(pet);
        ValidatePetAuditFieldsOnModify(pet);
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
            
            case { } when IsInvalid(pet.PetType):
                throw new InvalidPetException(
                    parameterName: nameof(Pet.PetType),
                    parameterValue: pet.PetType);
            
            case { } when IsInvalid(pet.Size):
                throw new InvalidPetException(
                    parameterName: nameof(Pet.Size),
                    parameterValue: pet.Size);
        }
    }

    private void ValidatePetAuditFieldsOnCreate(Pet pet)
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
            
            case { } when IsInvalid(pet.CreatedBy):
                throw new InvalidPetException(
                    parameterName: nameof(Pet.CreatedBy),
                    parameterValue: pet.CreatedBy);
            
            case { } when IsInvalid(pet.UpdatedBy):
                throw new InvalidPetException(
                    parameterName: nameof(Pet.UpdatedBy),
                    parameterValue: pet.UpdatedBy);
            
            case { } when pet.UpdatedDate != pet.CreatedDate:
                throw new InvalidPetException(
                    parameterName: nameof(Pet.UpdatedDate),
                    parameterValue: pet.UpdatedDate);
            
            case { } when pet.UpdatedBy != pet.CreatedBy:
                throw new InvalidPetException(
                    parameterName: nameof(Pet.UpdatedBy),
                    parameterValue: pet.UpdatedBy);
            
            case { } when IsNotRecentDate(pet.CreatedDate):
                throw new InvalidPetException(
                    parameterName: nameof(Pet.CreatedDate),
                    parameterValue: pet.CreatedDate);
        }
    }

    private static void ValidatePetAuditFieldsOnModify(Pet pet)
    {
        switch (pet)
        {
            case { } when IsInvalid(pet.CreatedBy):
                throw new InvalidPetException(
                    parameterName: nameof(pet.CreatedBy),
                    parameterValue: pet.CreatedBy);
        }
    }

    private static void ValidatePetStorage(Pet? storagePet, Guid petId)
    {
        if (storagePet is null)
            throw new NotFoundPetException(petId);
    }
    
    private static bool IsInvalid(Guid petId) => petId == Guid.Empty;
    private static bool IsInvalid(string input) => string.IsNullOrWhiteSpace(input);
    private static bool IsInvalid(DateTimeOffset dateTime)
    {
        return dateTime > DateTimeOffset.UtcNow
            || dateTime == default;
    }
    private static bool IsInvalid(PetType type) => type == default;
    private static bool IsInvalid(Size size) => size == default;

    private bool IsNotRecentDate(DateTimeOffset dateTime)
    {
        DateTimeOffset currentDateTime = _dateTimeBroker.GetCurrentDateTime();
        TimeSpan timeDifference = currentDateTime.Subtract(dateTime);
        TimeSpan oneMinute = TimeSpan.FromMinutes(1);

        return timeDifference.Duration() > oneMinute;
    }
}