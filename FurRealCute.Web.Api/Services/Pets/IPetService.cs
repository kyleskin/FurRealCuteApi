using FurRealCute.Web.Api.Models.Pets;

namespace FurRealCute.Web.Api.Services.Pets;

public interface IPetService
{
    ValueTask<Pet> CreatePetAsync(Pet? pet);
    IQueryable<Pet> RetrieveAllPets();
    ValueTask<Pet?> RetrievePetByIdAsync(Guid id);
}