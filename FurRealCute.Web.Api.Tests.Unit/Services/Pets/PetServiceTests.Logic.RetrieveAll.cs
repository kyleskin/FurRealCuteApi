using FluentAssertions;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Services.Pets;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllPets()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        IQueryable<Pet> randomPets = CreateRandomPets();
        IQueryable<Pet> storagePets = randomPets;
        IQueryable<Pet> expectedPets = storagePets;

        _storageBrokerMock.Setup(broker =>
                broker.SelectAllPets().Result)
            .Returns(storagePets);
        
        // Act
        IQueryable<Pet> actualPets = await _petService.RetrieveAllPetsAsync();
        
        // Assert
        actualPets.Should().BeEquivalentTo(expectedPets);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectAllPets(),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
}