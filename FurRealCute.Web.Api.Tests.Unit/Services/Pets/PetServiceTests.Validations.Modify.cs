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

    [Theory]
    [MemberData(nameof(InvalidMinuteCases))]
    public async Task ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(int minutes)
    {
        // Arrange
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet inputPet = randomPet;
        inputPet.UpdatedDate = dateTime.AddMinutes(minutes);

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.UpdatedDate),
            parameterValue: inputPet.UpdatedDate);

        PetValidationException expectedPetValidationException = new(invalidPetException);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(inputPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => modifyPetTask.AsTask());
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfPetDoesntExistAndLogItAsync()
    {
        // Arrange
        int randomNegativeMinutes = GetNegativeRandomNumber();
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet nonExistentPet = randomPet;
        nonExistentPet.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
        Pet? nullPet = null;

        NotFoundPetException notFoundPetException = new(nonExistentPet.Id);
        PetValidationException expectedPetValidationException = new(notFoundPetException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(nonExistentPet.Id))
            .ReturnsAsync(nullPet);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(nonExistentPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => modifyPetTask.AsTask());
        
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(),
            Times.Once);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectPetByIdAsync(nonExistentPet.Id),
            Times.Once);
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
    {
        // Arrange
        int randomNumber = GetRandomNumber();
        int randomMinutes = randomNumber;
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet invalidPet = randomPet;
        Pet storagePet = randomPet.DeepClone();
        Guid petId = invalidPet.Id;
        invalidPet.CreatedDate = storagePet.CreatedDate.AddMinutes(randomMinutes);

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.CreatedDate),
            parameterValue: invalidPet.CreatedDate);

        PetValidationException expectedPetValidationException = new(invalidPetException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(petId))
            .ReturnsAsync(storagePet);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(invalidPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => modifyPetTask.AsTask());
        
        _dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(),
            Times.Once);
        
        _storageBrokerMock.Verify(broker => 
            broker.SelectPetByIdAsync(petId));
        
        _loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
    {
        // Arrange
        int randomNumber = GetNegativeRandomNumber();
        int randomMinutes = randomNumber;
        Guid differentCreatedBy = Guid.NewGuid();
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet invalidPet = randomPet;
        invalidPet.CreatedDate = invalidPet.CreatedDate.AddMinutes(randomMinutes);
        Pet storagePet = invalidPet.DeepClone();
        invalidPet.CreatedBy = differentCreatedBy;
        Guid petId = invalidPet.Id;

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.CreatedBy),
            parameterValue: invalidPet.CreatedBy);

        PetValidationException expectedPetValidationException = new(invalidPetException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(petId))
            .ReturnsAsync(storagePet);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(invalidPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => modifyPetTask.AsTask());
        
        _dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Once);
        
        _storageBrokerMock.Verify(broker => 
            broker.SelectPetByIdAsync(petId));
        
        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowValidationExceptionIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
    {
        // Arrange
        int randomNumber = GetNegativeRandomNumber();
        int randomMinutes = randomNumber;
        DateTimeOffset dateTime = GetRandomDateTime();
        Pet randomPet = CreateRandomPet(dateTime);
        Pet invalidPet = randomPet;
        invalidPet.CreatedDate = invalidPet.CreatedDate.AddMinutes(randomMinutes);
        Pet storagePet = randomPet.DeepClone();
        Guid petId = invalidPet.Id;

        InvalidPetException invalidPetException = new(
            parameterName: nameof(Pet.UpdatedDate),
            parameterValue: invalidPet.UpdatedDate);

        PetValidationException expectedPetValidationException = new(invalidPetException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectPetByIdAsync(petId))
            .ReturnsAsync(storagePet);

        _dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
            .Returns(dateTime);
        
        // Act
        ValueTask<Pet?> modifyPetTask = _petService.ModifyPetAsync(invalidPet);
        
        // Assert
        await Assert.ThrowsAsync<PetValidationException>(() => modifyPetTask.AsTask());
        
        _dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Once);
        
        _storageBrokerMock.Verify(broker => 
            broker.SelectPetByIdAsync(petId));
        
        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPetValidationException))),
            Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}