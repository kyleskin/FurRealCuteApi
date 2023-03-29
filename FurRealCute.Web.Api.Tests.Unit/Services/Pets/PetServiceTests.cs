using System.Linq.Expressions;
using Bogus;
using FurRealCute.Web.Api.Brokers.DateTimeBroker;
using FurRealCute.Web.Api.Brokers.Logging;
using FurRealCute.Web.Api.Models.Pets;
using FurRealCute.Web.Api.Services.Pets;
using FurRealCute.Web.Api.Storage;
using Moq;
using Type = FurRealCute.Web.Api.Models.Pets.Type;

namespace FurRealCute.Web.Api.Tests.Unit.Services.Pets;

public partial class PetServiceTests
{
    private readonly Mock<IStorageBroker> _storageBrokerMock;
    private readonly Mock<IDateTimeBroker> _dateTimeBrokerMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IPetService _petService;
    
    private static readonly Random Random = new();

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

    private static DateTimeOffset GetRandomDateTime()
    {
        int randomOffset = Random.Next(15);
        return DateTimeOffset.UtcNow.AddDays(-randomOffset);
    }
    
    private static Pet CreateRandomPet(DateTimeOffset dateTime) =>
        CreateRandomPetFiller(dateTime).Generate();

    private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
    {
        return actualException =>
            actualException.Message == expectedException.Message
            && actualException.InnerException!.Message == expectedException.InnerException!.Message;
    }
    
    private static Faker<Pet> CreateRandomPetFiller(DateTimeOffset dateTime)
    {
        var fakePet = new Faker<Pet>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.Birthdate, f => f.Date.RecentOffset())
            .RuleFor(p => p.Type, f => f.PickRandom<Type>())
            .RuleFor(p => p.Size, f => f.PickRandom<Size>())
            .RuleFor(p => p.CreatedDate, f => f.Date.RecentOffset())
            .RuleFor(p => p.UpdatedBy, (f, p) => p.CreatedBy)
            .RuleFor(p => p.CreatedBy, f => Guid.NewGuid())
            .RuleFor(p => p.UpdatedBy, (f, p) => p.CreatedBy);
        
        return fakePet;
    }
}