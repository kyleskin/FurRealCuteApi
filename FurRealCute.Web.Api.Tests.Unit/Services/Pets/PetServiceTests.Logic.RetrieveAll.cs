using FluentAssertions;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Services.Pets;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public void ShouldRetrieveAllPets()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        IQueryable<Pet> randomPets = CreateRandomPets(dateTime);
        IQueryable<Pet> storagePets = randomPets;
        IQueryable<Pet> expectedPets = storagePets;

        _storageBrokerMock.Setup(broker =>
                broker.SelectAllPets())
            .Returns(storagePets);
        
        // Act
        IQueryable<Pet> actualPets = _petService.RetrieveAllPets();
        
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