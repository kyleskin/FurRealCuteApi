using FurRealCute.Web.Api.Models.Pets;

namespace FurRealCute.Web.Api.Storage;

public partial interface IStorageBroker
{
    ValueTask<Pet> InsertPetAsync(Pet pet);
    IQueryable<Pet> SelectAllPets();
}