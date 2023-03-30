using FluentAssertions;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Moq;
using Npgsql;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        PostgresException postgresException = GetPostgresException();

        PetDependencyException expectedPetDependencyException = new(postgresException);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);

        _storageBrokerMock.Setup(broker =>
                broker.InsertPetAsync(inputPet))
            .ThrowsAsync(postgresException);
        
        // Act
        ValueTask<Pet> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetDependencyException>(() =>
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(
                expectedPetDependencyException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.InsertPetAsync(inputPet),
            Times.Once);
        
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
        
    }
}