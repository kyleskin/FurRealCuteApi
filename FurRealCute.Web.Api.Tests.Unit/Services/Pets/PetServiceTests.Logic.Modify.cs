using FluentAssertions;
using FurRealCute.Web.Api.Models.Pets;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldModifyPetAsync()
    {
        // Arrange
        int randomNumber = GetRandomNumber();
        int randomDays = randomNumber;
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        Pet afterUpdateStoragePet = inputPet;
        Pet expectedPet = afterUpdateStoragePet;
        Pet beforeUpdateStoragePet = randomPet.DeepClone();
        inputPet.UpdatedDate = dateTime;
        Guid petId = inputPet.Id;

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(petId))
            .ReturnsAsync(beforeUpdateStoragePet);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);

        _storageBrokerMock.Setup(broker =>
                broker.UpdatePetAsync(inputPet))
            .ReturnsAsync(afterUpdateStoragePet);
        
        // Act
        Pet? actualPet = await _petService.ModifyPetAsync(inputPet);
        
        // Assert
        actualPet.Should().BeEquivalentTo(expectedPet);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectPetByIdAsync(petId),
            Times.Once);

        _storageBrokerMock.Verify(broker =>
            broker.UpdatePetAsync(inputPet),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}