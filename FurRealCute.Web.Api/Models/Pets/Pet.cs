namespace FurRealCute.Web.Api.Models.Pets;

public class Pet : IAuditable
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTimeOffset Birthdate { get; set; }
    public PetType PetType { get; set; }
    public Size Size { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}