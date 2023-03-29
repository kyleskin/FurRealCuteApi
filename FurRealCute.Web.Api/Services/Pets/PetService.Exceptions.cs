using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;

namespace FurRealCute.Web.Api.Services.Pets;

public partial class PetService
{
    private delegate ValueTask<Pet?> ReturningPetFunction();

    private async ValueTask<Pet?> TryCatch(ReturningPetFunction returningPetFunction)
    {
        try
        {
            return await returningPetFunction();
        }
        catch (NullPetException nullPetException)
        {
            throw CreateAndLogPetValidationException(nullPetException);
        }
    }

    private PetValidationException CreateAndLogPetValidationException(Exception exception)
    {
        PetValidationException petValidationException = new(exception);
        _loggingBroker.LogError(petValidationException);

        return petValidationException;
    }
}