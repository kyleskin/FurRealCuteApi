using System.Data.Common;
using System.Data.SqlTypes;
using EntityFramework.Exceptions.Common;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FurRealCute.Web.Api.Services.Pets;

public partial class PetService
{
    private delegate ValueTask<Pet> ReturningPetFunction();

    private delegate IQueryable<Pet> ReturningQueryablePetFunction();

    private async ValueTask<Pet> TryCatch(ReturningPetFunction returningPetFunction)
    {
        try
        {
            return await returningPetFunction();
        }
        catch (NullPetException nullPetException)
        {
            throw CreateAndLogPetValidationException(nullPetException);
        }
        catch (InvalidPetException invalidPetException)
        {
            throw CreateAndLogPetValidationException(invalidPetException);
        }
        catch (DbException dbException)
        {
            throw CreateAndLogCriticalDependencyException(dbException);
        }
        catch (UniqueConstraintException uniqueConstraintException)
        {
            DuplicatePetException duplicatePetException = new(uniqueConstraintException);
            throw CreateAndLogPetValidationException(duplicatePetException);
        }
    }

    private IQueryable<Pet> TryCatch(ReturningQueryablePetFunction returningQueryablePetFunction)
    {
        return returningQueryablePetFunction();
    }

    private PetValidationException CreateAndLogPetValidationException(Exception exception)
    {
        PetValidationException petValidationException = new(exception);
        _loggingBroker.LogError(petValidationException);

        return petValidationException;
    }

    private PetDependencyException CreateAndLogCriticalDependencyException(Exception exception)
    {
        PetDependencyException petDependencyException = new(exception);
        _loggingBroker.LogCritical(petDependencyException);

        return petDependencyException;
    }
}