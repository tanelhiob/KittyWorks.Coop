namespace KittyWorks.Coop.Web.Domain;

public class CampaignProduct : Entity
{
    public required decimal Offer { get; set; }

    public required Guid ProductId { get; init; }
    public Product Product { get; set; } = null!;

    public required Guid CampaignId { get; init; }
    public Campaign Campaign { get; set; } = null!;

    public ICollection<CampaignProductLocation> CampaignProductLocations { get; set; } = null!;
}
