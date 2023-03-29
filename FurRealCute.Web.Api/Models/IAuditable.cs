namespace FurRealCute.Web.Api.Models;

public interface IAuditable
{
    DateTimeOffset CreatedDate { get; set; }
    DateTimeOffset UpdatedDate { get; set; }
    Guid CreatedBy { get; set; }
    Guid UpdatedBy { get; set; }
}