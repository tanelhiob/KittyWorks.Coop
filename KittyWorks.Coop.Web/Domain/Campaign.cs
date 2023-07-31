namespace KittyWorks.Coop.Web.Domain;

public class Campaign : Entity
{
    public required string Name { get; set; }
    public required string ExternalId { get; set; }

    public ICollection<CampaignProduct> CampaignProducts { get; set; } = null!;
}