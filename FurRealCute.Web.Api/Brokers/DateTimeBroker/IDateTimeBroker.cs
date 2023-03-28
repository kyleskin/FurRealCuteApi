namespace FurRealCute.Web.Api.Brokers.DateTimeBroker;

public interface IDateTimeBroker
{
    DateTimeOffset GetCurrentDateTime();
}