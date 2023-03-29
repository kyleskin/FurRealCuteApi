using FurRealCute.Web.Api.Brokers.DateTimeBroker;
using FurRealCute.Web.Api.Brokers.Logging;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Storage;

namespace FurRealCute.Web.Api.Services.Pets;

public class PetService : IPetService
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

    public async ValueTask<Pet> CreatePetAsync(Pet pet)
    {
        return await _storageBroker.InsertPetAsync(pet);
    }
}