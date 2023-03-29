using FluentAssertions;
using FurRealCute.Web.Api.Models.Pets;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldAddPetAsync()
    {
        // Arrange
        DateTimeOffset randomDateTime = GetRandomDateTime();
        DateTimeOffset dateTime = randomDateTime;
        Pet randomPet = CreateRandomPet(randomDateTime);
        Pet inputPet = randomPet;
        Pet storagePet = randomPet;
        Pet expectedPet = storagePet;

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTime()).Returns(dateTime);

        _storageBrokerMock.Setup(broker =>
            broker.InsertPetAsync(inputPet))
            .ReturnsAsync(storagePet);

        // Act
        Pet actualPet = await _petService.CreatePetAsync(inputPet);

        // Assert
        actualPet.Should().BeEquivalentTo(expectedPet);
        
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(), Times.Once);
        
        _storageBrokerMock.Verify(broker => 
            broker.InsertPetAsync(inputPet), Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
}