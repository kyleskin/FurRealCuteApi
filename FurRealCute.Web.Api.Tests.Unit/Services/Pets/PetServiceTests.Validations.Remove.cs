using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRemoveWhenIdIsInvalidAndLogItAsync()
    {
        // Arrange
        Guid invalidId = Guid.Empty;

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.Id),
            parameterValue: invalidId);

        PetValidationException expectedPetValidationException = new(invalidPetException);

        // Act
        ValueTask<Pet> removePetTask = _petService.RemovePetByIdAsync(invalidId);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => removePetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnRemoveWhenStoragePetIsNullAndLogItAsync()
    {
        // Arrange
        Guid randomId = Guid.NewGuid();
        Guid inputId = randomId;
        Pet? invalidStoragePet = null;

        NotFoundPetException notFoundPetException = new(inputId);

        PetValidationException expectedPetValidationException = new(notFoundPetException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(inputId))
            .ReturnsAsync(invalidStoragePet);
        
        // Act
        ValueTask<Pet> deletePetTask = _petService.RemovePetByIdAsync(inputId);
        
        // Assert
        await Assert.ThrowsAsync<NotFoundPetException>(() => deletePetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectPetByIdAsync(inputId),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}