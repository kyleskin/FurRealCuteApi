using Bogus.DataSets;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenPetIsNullAndLogItAsync()
    {
        // Arrange
        Pet? invalidPet = null;
        NullPetException nullPetException = new();
        PetValidationException expectedPetValidationException = new(nullPetException);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(invalidPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => modifyPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenPetIdIsInvalidAndLogItAsync()
    {
        // Arrange
        Guid invalidId = Guid.Empty;
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet invalidPet = randomPet;
        invalidPet.Id = invalidId;

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.Id),
            parameterValue: invalidPet.Id);

        PetValidationException expectedPetValidationException = new(invalidPetException);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(invalidPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => modifyPetTask.AsTask());

        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task ShouldThrowValidationExceptionOnModifyWhenPetNameIsInvalidAndLogItAsync(string invalidName)
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.Name = invalidName;

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.Name),
            parameterValue: inputPet.Name);

        PetValidationException expectedPetValidationException = new(invalidPetException);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => modifyPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenPetBirthdateIsInvalidAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.Birthdate = DateTimeOffset.Now.AddDays(1);

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.Birthdate),
            parameterValue: inputPet.Birthdate);

        PetValidationException expectedPetValidationException = new(invalidPetException);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => modifyPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenPetTypeIsInvalidAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.PetType = default;

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.PetType),
            parameterValue: inputPet.PetType);

        PetValidationException expectedPetValidationException = new(invalidPetException);
    
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
    
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() =>
            modifyPetTask.AsTask());
    
        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))), 
            Times.Once);
    
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenPetSizeIsInvalidAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.Size = default;

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.Size),
            parameterValue: inputPet.Size);

        PetValidationException expectedPetValidationException = new(invalidPetException);
    
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
    
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() =>
            modifyPetTask.AsTask());
    
        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))), 
            Times.Once);
    
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.CreatedDate = default;
        
        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.CreatedDate), 
            parameterValue: inputPet.CreatedDate);

        PetValidationException expectedPetValidationException = new(invalidPetException);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => 
            modifyPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.UpdatedDate = default;
        
        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.UpdatedDate), 
            parameterValue: inputPet.UpdatedDate);

        PetValidationException expectedPetValidationException = new(invalidPetException);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => 
            modifyPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsSameAsUpdatedDateAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.UpdatedDate), 
            parameterValue: inputPet.UpdatedDate);

        PetValidationException expectedPetValidationException = new(invalidPetException);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => 
            modifyPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenCreatedByIsInvalidAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.CreatedBy = default;

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.CreatedBy),
            parameterValue: inputPet.CreatedBy);

        PetValidationException expectedPetValidationException = new(invalidPetException);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => modifyPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenUpdatedByIsInvalidAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.UpdatedBy = Guid.Empty;
        
        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.UpdatedBy), 
            parameterValue: inputPet.UpdatedBy);

        PetValidationException expectedPetValidationException = new(invalidPetException);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => 
            modifyPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}