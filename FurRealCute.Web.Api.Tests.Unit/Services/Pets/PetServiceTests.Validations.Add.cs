using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
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
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(invalidPet);
        
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
}