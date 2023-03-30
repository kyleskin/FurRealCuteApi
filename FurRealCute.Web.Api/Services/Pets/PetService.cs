using FurRealCute.Web.Api.Brokers.DateTimeBroker;
using FurRealCute.Web.Api.Brokers.Logging;
using FurRealCute.Web.Api.Models.Pets;
using IStorageBroker = FurRealCute.Web.Api.Brokers.Storages.IStorageBroker;

namespace FurRealCute.Web.Api.Services.Pets;

public partial class PetService : IPetService
{
    private readonly IStorageBroker _storageBroker;
    private readonly IDateTimeBroker _dateTimeBroker;
    private readonly ILoggingBroker _loggingBroker;

    public PetService(IStorageBroker storageBroker, IDateTimeBroker dateTimeBroker, ILoggingBroker loggingBroker)
    {
        _storageBroker = storageBroker;
        _dateTimeBroker = dateTimeBroker;
        _loggingBroker = loggingBroker;
    }

    public ValueTask<Pet> CreatePetAsync(Pet? pet) =>
    TryCatch(async () =>
    {
        ValidatePetOnCreate(pet);
        
        return await _storageBroker.InsertPetAsync(pet!);
    });

    public IQueryable<Pet> RetrieveAllPets() =>
    TryCatch(() => _storageBroker.SelectAllPets());
}