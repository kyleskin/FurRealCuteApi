using System.Data;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
    {
        // Arrange
        int randomNumber = GetNegativeRandomNumber();
        int randomMinutes = randomNumber;
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        Guid petId = inputPet.Id;
        inputPet.CreatedDate = inputPet.CreatedDate.AddMinutes(randomMinutes);

        PostgresException postgresException = GetPostgresException();
        PetDependencyException expectedDependencyException = new(postgresException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(petId))
            .ThrowsAsync(postgresException);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetDependencyException>(() => modifyPetTask.AsTask());
        
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectPetByIdAsync(inputPet.Id),
            Times.Once);
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(expectedDependencyException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
    {
        // Arrange
        int randomNumber = GetNegativeRandomNumber();
        int randomMinutes = randomNumber;
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        Guid petId = inputPet.Id;
        inputPet.CreatedDate = inputPet.CreatedDate.AddMinutes(randomMinutes);

        DbUpdateException dbUpdateException = new();
        PetDependencyException expectedDependencyException = new(dbUpdateException);
        
        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(petId))
            .ThrowsAsync(dbUpdateException);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetDependencyException>(() => modifyPetTask.AsTask());
        
        _dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
                broker.SelectPetByIdAsync(inputPet.Id),
            Times.Once);
        
        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedDependencyException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnModifyIfDbConcurrencyExceptionOccursAndLogItAsync()
    {
        // Arrange
        int randomNumber = GetNegativeRandomNumber();
        int randomMinutes = randomNumber;
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        Guid petId = inputPet.Id;
        inputPet.CreatedDate = inputPet.CreatedDate.AddMinutes(randomMinutes);

        DBConcurrencyException dbConcurrencyException = new();
        PetDependencyException expectedDependencyException = new(dbConcurrencyException);
        
        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(petId))
            .ThrowsAsync(dbConcurrencyException);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetDependencyException>(() => modifyPetTask.AsTask());
        
        _dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
                broker.SelectPetByIdAsync(inputPet.Id),
            Times.Once);
        
        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedDependencyException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
    {
        // Arrange
        int randomNumber = GetNegativeRandomNumber();
        int randomMinutes = randomNumber;
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        Guid petId = inputPet.Id;
        inputPet.CreatedDate = inputPet.CreatedDate.AddMinutes(randomMinutes);

        Exception serviceException = new();
        FailedPetServiceException failedPetServiceException = new(serviceException);
        PetServiceException expectedPetServiceException = new(failedPetServiceException);
        
        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(petId))
            .ThrowsAsync(serviceException);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetServiceException>(() => modifyPetTask.AsTask());
        
        _dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
                broker.SelectPetByIdAsync(inputPet.Id),
            Times.Once);
        
        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPetServiceException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}