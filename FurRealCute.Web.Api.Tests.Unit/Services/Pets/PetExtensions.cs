using FurRealCute.Web.Api.Models.Pets;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public static class PetExtensions
{
    public static Pet DeepClone(this Pet pet)
    {
        return new Pet()
        {
            Id = pet.Id,
            Name = pet.Name,
            Birthdate = pet.Birthdate,
            PetType = pet.PetType,
            Size = pet.Size,
            CreatedDate = pet.CreatedDate,
            UpdatedDate = pet.UpdatedDate,
            CreatedBy = pet.CreatedBy,
            UpdatedBy = pet.UpdatedBy
        };
    }   
}