using System.Data;
using System.Data.Common;
using EntityFramework.Exceptions.Common;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FurRealCute.Web.Api.Services.Pets;

public partial class PetService
{
    private delegate ValueTask<Pet?> ReturningPetFunction();

    private delegate IQueryable<Pet> ReturningQueryablePetFunction();

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
        catch (InvalidPetException invalidPetException)
        {
            throw CreateAndLogPetValidationException(invalidPetException);
        }
        catch (NotFoundPetException notFoundPetException)
        {
            throw CreateAndLogPetValidationException(notFoundPetException);
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
        catch (DbUpdateException dbUpdateException)
        {
            throw CreateAndLogDependencyException(dbUpdateException);
        }
        catch (DBConcurrencyException dbConcurrencyException)
        {
            throw CreateAndLogDependencyException(dbConcurrencyException);
        }
        catch (Exception exception)
        {
            FailedPetServiceException failedPetServiceException = new(exception);

            throw CreateAndLogServiceException(failedPetServiceException);
        }
    }

    private IQueryable<Pet> TryCatch(ReturningQueryablePetFunction returningQueryablePetFunction)
    {
        try
        {
            return returningQueryablePetFunction();

        }
        catch (PostgresException postgresException)
        {
            throw CreateAndLogCriticalDependencyException(postgresException);
        }
        catch (Exception exception)
        {
            FailedPetServiceException failedPetServiceException = new(exception);

            throw CreateAndLogServiceException(failedPetServiceException);
        }
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

    private PetDependencyException CreateAndLogDependencyException(Exception exception)
    {
        PetDependencyException petDependencyException = new(exception);
        _loggingBroker.LogError(petDependencyException);

        return petDependencyException;
    }

    private PetServiceException CreateAndLogServiceException(Exception exception)
    {
        PetServiceException petServiceException = new(exception);
        _loggingBroker.LogError(petServiceException);

        return petServiceException;
    }
}