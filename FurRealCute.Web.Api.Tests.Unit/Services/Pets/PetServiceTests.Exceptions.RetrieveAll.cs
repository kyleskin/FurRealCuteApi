using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Moq;
using Npgsql;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
    {
        // Arrange
        PostgresException postgresException = GetPostgresException();
        PetDependencyException expectedPetDependencyException = new(postgresException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectAllPets())
            .Throws(postgresException);
        
        // Act
        Action retrieveAllPetsAction = () =>
            _petService.RetrieveAllPets();
        
        // Assert
        Assert.Throws<PetDependencyException>(retrieveAllPetsAction);
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(expectedPetDependencyException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker => 
            broker.SelectAllPets(),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
    {
        // Arrange
        Exception serviceException = new();
        FailedPetServiceException failedPetServiceException = new(serviceException);
        PetServiceException expectedPetServiceException = new(failedPetServiceException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectAllPets())
            .Throws(serviceException);
        
        // Act
        Action retrieveAllPetsAction = () => _petService.RetrieveAllPets();
        
        // Assert
        Assert.Throws<PetServiceException>(retrieveAllPetsAction);
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetServiceException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectAllPets(),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}