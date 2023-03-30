using FluentAssertions;
using FurRealCute.Web.Api.Models.Pets;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldRetrievePetByIdAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Guid inputPetId = randomPet.Id;
        Pet inputPet = randomPet;
        Pet storagePet = inputPet;
        Pet expectedPet = storagePet;

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(inputPetId).Result)
            .Returns(storagePet);
        
        // Act
        Pet? actualPet = await _petService.RetrievePetByIdAsync(inputPetId);
        
        // Assert
        actualPet.Should().BeEquivalentTo(expectedPet);
        
       _storageBrokerMock.Verify(broker =>
           broker.SelectPetByIdAsync(inputPetId),
           Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}