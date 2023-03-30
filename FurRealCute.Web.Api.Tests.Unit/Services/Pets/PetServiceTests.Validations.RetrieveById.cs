using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
  [Fact]
  public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenIdNullAndLogItAsync()
  {
    // Arrange
    Guid invalidId = Guid.Empty;

    InvalidPetException invalidPetException = new(
      parameterName: nameof(Pet.Id),
      parameterValue: invalidId);

    PetValidationException expectedPetValidationException = new(invalidPetException);
    
    // Act
    ValueTask<Pet?> retrievePetTask = _petService.RetrievePetByIdAsync(invalidId);
    
    // Assert
    await Assert.ThrowsAsync<PetValidationException>(() =>
      retrievePetTask.AsTask());
    
    _loggingBrokerMock.Verify(broker =>
      broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
      Times.Once);
    
    _dateTimeBrokerMock.VerifyNoOtherCalls();
    _loggingBrokerMock.VerifyNoOtherCalls();
    _storageBrokerMock.VerifyNoOtherCalls();
  }
}