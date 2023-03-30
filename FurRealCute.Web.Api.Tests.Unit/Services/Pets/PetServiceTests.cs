using System.Data.Common;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Bogus;
using FurRealCute.Web.Api.Brokers.DateTimeBroker;
using FurRealCute.Web.Api.Brokers.Logging;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Services.Pets;
using FurRealCute.Web.Api.Storage;
using Moq;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    private readonly Mock<IStorageBroker> _storageBrokerMock;
    private readonly Mock<IDateTimeBroker> _dateTimeBrokerMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IPetService _petService;
    
    private static readonly Random Random = new();
    private static readonly Faker Faker = new();

    public PetServiceTests()
    {
        _storageBrokerMock = new Mock<IStorageBroker>();
        _dateTimeBrokerMock = new Mock<IDateTimeBroker>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();

        _petService = new PetService(
            _storageBrokerMock.Object,
            _dateTimeBrokerMock.Object,
            _loggingBrokerMock.Object);
    }

    public static TheoryData InvalidMinuteCases()
    {
        int randomMoreThanMinuteFromNow = GetRandomNumber();
        int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

        return new TheoryData<int>
        {
            randomMoreThanMinuteFromNow,
            randomMoreThanMinuteBeforeNow
        };
    }

    private static DateTimeOffset GetRandomDateTime() => 
        DateTimeOffset.UtcNow.AddDays(GetNegativeRandomNumber());
    
    private static int GetRandomNumber() => Random.Next(2, 10);
    private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
    
    private static Pet CreateRandomPet(DateTimeOffset dateTime) =>
        CreateRandomPetFiller(dateTime).Generate();

    private static IQueryable<Pet> CreateRandomPets(DateTimeOffset dateTime) =>
        CreateRandomPetFiller(dateTime).Generate(Faker.Random.Number(3)).AsQueryable();

    private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
    {
        return actualException =>
            actualException.Message == expectedException.Message
            && actualException.InnerException!.Message == expectedException.InnerException!.Message;
    }

    private static DbException GetDbException() =>
        (DbException)FormatterServices.GetUninitializedObject(typeof(DbException));
    
    private static Faker<Pet> CreateRandomPetFiller(DateTimeOffset dateTime)
    {
        var fakePet = new Faker<Pet>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.Birthdate, f => dateTime.AddDays(-1))
            .RuleFor(p => p.PetType, f => f.PickRandom<PetType>())
            .RuleFor(p => p.Size, f => f.PickRandom<Size>())
            .RuleFor(p => p.CreatedDate, f => dateTime)
            .RuleFor(p => p.UpdatedDate, (f, p) => p.CreatedDate)
            .RuleFor(p => p.CreatedBy, f => Guid.NewGuid())
            .RuleFor(p => p.UpdatedBy, (f, p) => p.CreatedBy);
        
        return fakePet;
    }
}