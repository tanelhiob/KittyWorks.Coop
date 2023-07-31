﻿namespace KittyWorks.Coop.Web.Domain;

public class CampaignProductLocation : Entity
{
    public required Guid LocationId { get; init; }
    public Location Location { get; set; } = null!;

    public required Guid CampaignProductId { get; init; }
    public CampaignProduct CampaignProduct { get; set; } = null!;
}
