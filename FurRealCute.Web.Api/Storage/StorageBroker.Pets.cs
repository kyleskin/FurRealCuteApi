using FurRealCute.Web.Api.Models.Pets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FurRealCute.Web.Api.Storage;

public partial class StorageBroker
{
    public DbSet<Pet> Pets { get; set; }
    
    public async ValueTask<Pet> InsertPetAsync(Pet pet)
    {
        var broker = new StorageBroker(_configuration);
        EntityEntry<Pet> petEntityEntry = await broker.Pets.AddAsync(entity: pet);
        await broker.SaveChangesAsync();

        return petEntityEntry.Entity;
    }
}