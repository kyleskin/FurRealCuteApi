using System.Runtime.CompilerServices;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Moq;
using Npgsql;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
    {
        // Arrange
        Guid randomId = Guid.NewGuid();
        Guid inputId = randomId;
        
        PostgresException sqlException = GetPostgresException();
        PetDependencyException expectedPetDependencyException = new(sqlException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(inputId))
            .ThrowsAsync(sqlException);
        
        // Act
        ValueTask<Pet> removePetTask = _petService.RemovePetByIdAsync(inputId);
        
        // Assert
        await Assert.ThrowsAsync<PetDependencyException>(() => removePetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(expectedPetDependencyException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectPetByIdAsync(inputId),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}