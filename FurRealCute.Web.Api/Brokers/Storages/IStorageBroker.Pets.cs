using FurRealCute.Web.Api.Models.Pets;

namespace FurRealCute.Web.Api.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<Pet> InsertPetAsync(Pet pet);
    IQueryable<Pet> SelectAllPets();
    ValueTask<Pet?> SelectPetByIdAsync(Guid id);
    ValueTask<Pet> UpdatePetAsync(Pet pet);
    ValueTask<Pet> DeletePetAsync(Pet pet);
}