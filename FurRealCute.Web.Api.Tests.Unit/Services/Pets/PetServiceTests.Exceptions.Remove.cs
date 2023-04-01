using System.Runtime.CompilerServices;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
    {
        // Arrange
        Guid randomId = Guid.NewGuid();
        Guid inputId = randomId;
        
        PostgresException sqlException = GetPostgresException();
        PetDependencyException expectedPetDependencyException = new(sqlException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(inputId))
            .ThrowsAsync(sqlException);
        
        // Act
        ValueTask<Pet> removePetTask = _petService.RemovePetByIdAsync(inputId);
        
        // Assert
        await Assert.ThrowsAsync<PetDependencyException>(() => removePetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(expectedPetDependencyException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectPetByIdAsync(inputId),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
    {
        // Arrange
        Guid randomId = Guid.NewGuid();
        Guid inputId = randomId;

        DbUpdateException dbUpdateException = new();
        PetDependencyException expectedPetDependencyException = new(dbUpdateException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(inputId))
            .ThrowsAsync(dbUpdateException);
        
        // Act
        ValueTask<Pet> removePetTask = _petService.RemovePetByIdAsync(inputId);
        
        // Assert
        await Assert.ThrowsAsync<PetDependencyException>(() => removePetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetDependencyException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectPetByIdAsync(inputId),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbConcurrencyExceptionOccursAndLogItAsync()
    {
        // Arrange
        Guid randomId = Guid.NewGuid();
        Guid inputId = randomId;

        DbUpdateConcurrencyException dbUpdateConcurrencyException = new();
        PetDependencyException expectedPetDependencyException = new(dbUpdateConcurrencyException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(inputId))
            .ThrowsAsync(dbUpdateConcurrencyException);
        
        // Act
        ValueTask<Pet> removePetTask = _petService.RemovePetByIdAsync(inputId);
        
        // Assert
        await Assert.ThrowsAsync<PetDependencyException>(() => removePetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPetDependencyException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
                broker.SelectPetByIdAsync(inputId),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}