using FurRealCute.Web.Api.Models.Pets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FurRealCute.Web.Api.Brokers.Storages;

public partial class StorageBroker
{
    public DbSet<Pet> Pets { get; set; }
    
    public async ValueTask<Pet> InsertPetAsync(Pet pet)
    {
        StorageBroker broker = new(_configuration);
        EntityEntry<Pet> petEntityEntry = await broker.Pets.AddAsync(entity: pet);
        await broker.SaveChangesAsync();

        return petEntityEntry.Entity;
    }

    public IQueryable<Pet> SelectAllPets() => Pets;

    public async ValueTask<Pet?> SelectPetByIdAsync(Guid id)
    {
        StorageBroker broker = new(_configuration);
        broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        return await broker.Pets.FindAsync(id);
    }

    public async ValueTask<Pet> UpdatePetAsync(Pet pet)
    {
        StorageBroker broker = new(_configuration);
        EntityEntry<Pet> petEntityEntry = broker.Pets.Update(entity: pet);
        await broker.SaveChangesAsync();

        return petEntityEntry.Entity;
    }
}