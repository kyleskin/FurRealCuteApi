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
}