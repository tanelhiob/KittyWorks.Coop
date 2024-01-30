namespace KittyWorks.Coop.Web.Domain.Location;

public record Location : Entity
{
    public required string Gln { get; set; }
}
