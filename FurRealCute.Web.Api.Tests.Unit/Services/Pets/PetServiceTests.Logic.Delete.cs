using FluentAssertions;
using FurRealCute.Web.Api.Models.Pets;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldDeletePetAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet storagePet = randomPet;
        Guid petId = storagePet.Id;
        Pet expectedPet = storagePet;

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(petId))
            .ReturnsAsync(storagePet);

        _storageBrokerMock.Setup(broker =>
                broker.DeletePetAsync(storagePet))
            .ReturnsAsync(expectedPet);
        
        // Act
        Pet actualPet = await _petService.RemovePetByIdAsync(petId);
        
        // Assert
        actualPet.Should().BeEquivalentTo(expectedPet);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectPetByIdAsync(petId),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.DeletePetAsync(storagePet),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}