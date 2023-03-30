using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenPetIsNullAndLogItAsync()
    {
        // Arrange
        Pet? invalidPet = null;
        NullPetException nullPetException = new();
        PetValidationException expectedPetValidationException = new(nullPetException);
        
        // Act
        ValueTask<Pet> modifyPetTask = _petService.ModifyPetAsync(invalidPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => modifyPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenPetIdIsInvalidAndLogItAsync()
    {
        // Arrange
        Guid invalidId = Guid.Empty;
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet invalidPet = randomPet;
        invalidPet.Id = invalidId;

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.Id),
            parameterValue: invalidPet.Id);

        PetValidationException expectedPetValidationException = new(invalidPetException);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(invalidPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => modifyPetTask.AsTask());

        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}