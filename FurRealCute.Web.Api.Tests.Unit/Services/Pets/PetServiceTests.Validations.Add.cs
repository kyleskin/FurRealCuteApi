using EntityFramework.Exceptions.Common;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Models.Pets.Exceptions;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddWhenPetIsNullAndLogItAsync()
    {
        // Arrange
        Pet? invalidPet = null;

        NullPetException nullPetException = new();

        PetValidationException expectedPetValidationException = new(nullPetException);
        
        // Act
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(invalidPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() =>
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(
                expectedPetValidationException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker => 
            broker.InsertPetAsync(It.IsAny<Pet>()),
            Times.Never);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddWhenIdIsInvalidAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.Id = Guid.Empty;

        InvalidPetException invalidPetInputException = new(
            parameterName: nameof(Pet.Id),
            parameterValue: inputPet.Id);

        PetValidationException expectedPetValidationException = new(invalidPetInputException);
        
        // Act
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() =>
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.InsertPetAsync(It.IsAny<Pet>()),
            Times.Never);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task ShouldThrowValidationExceptionWhenPetNameIsInvalidAndLogItAsync(string invalidPetName)
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.Name = invalidPetName;
        
        InvalidPetException invalidPetInputException = new(
            parameterName: nameof(Pet.Name),
            parameterValue: inputPet.Name);

        PetValidationException expectedPetValidationException = new(invalidPetInputException);
        
        // Act
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() =>
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
                broker.InsertPetAsync(It.IsAny<Pet>()),
            Times.Never);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionWhenPetBirthdateIsInvalidAndLogItAsync()
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
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);

        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() =>
            createPetTask.AsTask());

        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);

        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionWhenPetTypeIsInvalidAndLogItAsync()
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
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
    
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() =>
            createPetTask.AsTask());
    
        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))), 
            Times.Once);
    
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionWhenPetSizeIsInvalidAndLogItAsync()
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
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
    
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() =>
            createPetTask.AsTask());
    
        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))), 
            Times.Once);
    
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddWhenCreatedDateIsInvalidAndLogItAsync()
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
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => 
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsInvalidAndLogItAsync()
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
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => 
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddWhenCreatedDateIsNotSameAsUpdatedDateAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.UpdatedDate = dateTime.AddDays(1);
        
        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.UpdatedDate), 
            parameterValue: inputPet.UpdatedDate);

        PetValidationException expectedPetValidationException = new(invalidPetException);
        
        // Act
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => 
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddWhenCreatedByIsInvalidAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.CreatedBy = Guid.Empty;
        
        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.CreatedBy), 
            parameterValue: inputPet.CreatedBy);

        PetValidationException expectedPetValidationException = new(invalidPetException);
        
        // Act
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => 
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddWhenUpdatedByIsInvalidAndLogItAsync()
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
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => 
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddWhenCreatedByIsNotSameAsUpdatedByAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.UpdatedBy = Guid.NewGuid();
        
        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.UpdatedBy),
            parameterValue: inputPet.UpdatedBy);

        PetValidationException expectedPetValidationException = new(invalidPetException);
        
        // Act
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => 
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [MemberData(nameof(InvalidMinuteCases))]
    public async Task ShouldThrowValidationExceptionOnAddWhenCreatedDateIsNotRecentAndLogItAsync(int minutes)
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.CreatedDate = dateTime.AddMinutes(minutes);
        inputPet.UpdatedDate = inputPet.CreatedDate;

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.CreatedDate),
            parameterValue: inputPet.CreatedDate);

        PetValidationException expectedInvalidPetException = new(invalidPetException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTime()).Returns(dateTime);
        
        // Act
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() =>
            createPetTask.AsTask());
        
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(),
            Times.Once);
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedInvalidPetException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddWhenPetAlreadyExistsAndLogItAsync()
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet existingPet = randomPet;

        UniqueConstraintException uniqueConstraintException = new();
        DuplicatePetException duplicatePetException = new(uniqueConstraintException);
        PetValidationException expectedPetValidationException = new(duplicatePetException);

        _dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTime()).Returns(dateTime);

        _storageBrokerMock.Setup(broker =>
            broker.InsertPetAsync(existingPet)).ThrowsAsync(uniqueConstraintException);
        
        // Act
        ValueTask<Pet?> createPetTask = _petService.CreatePetAsync(existingPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() =>
            createPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(), 
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.InsertPetAsync(existingPet),
            Times.Once);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}