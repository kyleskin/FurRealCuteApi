using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        PostgresException postgresException = GetPostgresException();

        PetDependencyException expectedPetDependencyException = new(postgresException);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);

        _storageBrokerMock.Setup(broker =>
                broker.InsertPetAsync(inputPet))
            .ThrowsAsync(postgresException);
        
        // Act
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetDependencyException>(() =>
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(
                expectedPetDependencyException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.InsertPetAsync(inputPet),
            Times.Once);
        
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
        
    }

    [Fact]
    public async Task ShouldThrowDependencyExceptionOnAddWhenDbUpdateExceptionOccursAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        DbUpdateException databaseUpdateException = new();

        PetDependencyException expectedPetDependencyException = new(databaseUpdateException);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);

        _storageBrokerMock.Setup(broker =>
                broker.InsertPetAsync(inputPet))
            .ThrowsAsync(databaseUpdateException);
        
        // Act
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetDependencyException>(() =>
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetDependencyException))), 
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
                broker.InsertPetAsync(inputPet),
            Times.Once);
        
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        Exception serviceException = new();

        FailedPetServiceException failedPetServiceException = new(serviceException);

        PetServiceException expectedPetServiceException = new(failedPetServiceException);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);

        _storageBrokerMock.Setup(broker =>
                broker.InsertPetAsync(inputPet))
            .ThrowsAsync(serviceException);
        
        // Act
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetServiceException>(() => createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetServiceException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.InsertPetAsync(inputPet),
            Times.Once);
        
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}