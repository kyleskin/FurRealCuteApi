using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
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
        ValueTask<Pet?> retrievePetTask = _petService.RetrievePetByIdAsync(inputId);
        
        // Assert
        await Assert.ThrowsAsync<PetDependencyException>(() =>
            retrievePetTask.AsTask());

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
    public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
    {
        // Arrange
        Guid randomId = Guid.NewGuid();
        Guid inputId = randomId;
        DbUpdateException databaseUpdateException = new();

        PetDependencyException expectedPetDependencyException = new(databaseUpdateException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(inputId))
            .ThrowsAsync(databaseUpdateException);
        
        // Act
        ValueTask<Pet?> retrievePetTask = _petService.RetrievePetByIdAsync(inputId);
        
        // Assert
        await Assert.ThrowsAsync<PetDependencyException>(() => retrievePetTask.AsTask());
        
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
    public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
    {
        // Arrange
        Guid randomId = Guid.NewGuid();
        Guid inputId = randomId;
        Exception serviceException = new();

        FailedPetServiceException failedPetServiceException = new(serviceException);
        PetServiceException expectedPetServiceException = new(failedPetServiceException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(inputId))
            .ThrowsAsync(failedPetServiceException);
        
        // Act
        ValueTask<Pet?> retrievePetTask = _petService.RetrievePetByIdAsync(inputId);
        
        // Arrange
        await Assert.ThrowsAsync<PetServiceException>(() =>
            retrievePetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetServiceException))),
            Times.Once);

        _storageBrokerMock.Verify(broker =>
                broker.SelectPetByIdAsync(inputId),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}