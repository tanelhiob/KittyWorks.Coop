namespace KittyWorks.Coop.Web.Domain.Product;

public record Product : Entity
{
    public required string Sku { get; set; }
}
