using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async void ShouldThrowValidationExceptionOnAddWhenPetIsNullAndLogItAsync()
    {
        // Arrange
        Pet? invalidPet = null;

        NullPetException nullPetException = new();

        PetValidationException expectedPetValidationException = new(nullPetException);
        
        // Act
        ValueTask<Pet> createPetTask = _petService.CreatePetAsync(invalidPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() =>
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(
                expectedPetValidationException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker => 
            broker.InsertPetAsync(It.IsAny<Pet>()),
            Times.Never);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async void ShouldThrowValidationExceptionOnAddWhenIdIsInvalidAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.Id = Guid.Empty;

        InvalidPetInputException invalidPetInputException = new(
            parameterName: nameof(Pet.Id),
            parameterValue: inputPet.Id);

        PetValidationException expectedPetValidationException = new(invalidPetInputException);
        
        // Act
        ValueTask<Pet> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() =>
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.InsertPetAsync(It.IsAny<Pet>()),
            Times.Never);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}