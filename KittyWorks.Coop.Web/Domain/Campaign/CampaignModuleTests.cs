using NSubstitute;
using Xunit;

namespace KittyWorks.Coop.Web.Domain.Campaign;

public static partial class CampaignModule
{
    class CreateCampaignTestContext : IAddCampaignContext
    {
        public required Action Commit { get; init; }
        public required Func<string, Campaign?> GetCampaignByExternalId { get; init; }
        public required Func<Guid, Guid, CampaignProduct?> GetCampaignProductByCampaignIdAndProductId { get; init; }
        public required Func<Guid, Guid, CampaignProductLocation?> GetCampaignProductLocationByCampaignProductIdAndLocationId { get; init; }
        public required Action<Campaign> AddCampaign { get; init; }
        public required Action<CampaignProduct> AddCampaignProduct { get; init; }
        public required Action<CampaignProductLocation> AddCampaignProductLocation { get; init; }
    }

    [Theory]
    [InlineData("1", "kampaania", "8ca1b475-5f39-4757-aa7e-e86b7968ecc4", "13928890-baf6-4731-9bfd-65c87e4723c4", 10)]
    static void AddCampaignTestNSubstitute(string externalId, string name, Guid productId, Guid locationId, decimal offer)
    {
        var context = Substitute.For<IAddCampaignContext>();
        context.Commit.Returns(Substitute.For<Action>());
        context.GetCampaignByExternalId.Returns(Substitute.For<Func<string, Campaign>>());
        context.GetCampaignProductByCampaignIdAndProductId.Returns(Substitute.For<Func<Guid, Guid, CampaignProduct>>());
        context.GetCampaignProductLocationByCampaignProductIdAndLocationId.Returns(Substitute.For<Func<Guid, Guid, CampaignProductLocation>>());
        context.AddCampaign.Returns(Substitute.For<Action<Entity>>());
        context.AddCampaignProduct.Returns(Substitute.For<Action<Entity>>());
        context.AddCampaignProductLocation.Returns(Substitute.For<Action<Entity>>());

        var dto = new CampaignDto { ExternalId = externalId, Name = name, ProductId = productId, LocationId = locationId, Offer = offer };
        AddCampaign(context, dto);

        context.AddCampaign.Received(1).Invoke(Arg.Is<Campaign>(c => c.ExternalId == "1"));
        context.AddCampaignProduct.Received(1).Invoke(Arg.Is<CampaignProduct>(cp => cp.ProductId == productId));
        context.AddCampaignProductLocation.Received(1).Invoke(Arg.Is<CampaignProductLocation>(cpl => cpl.LocationId == locationId));
        context.Commit.Received(1).Invoke();
    }

    [Theory]
    [InlineData("1", "kampaania", "8ca1b475-5f39-4757-aa7e-e86b7968ecc4", "13928890-baf6-4731-9bfd-65c87e4723c4", 10)]
    static void AddCampaignTestNatural(string externalId, string name, Guid productId, Guid locationId, decimal offer)
    {
        var existingCampaign = new Campaign
        {
            ExternalId = externalId,
            Name = "eksisteerib",
            CampaignProducts = [],
        };

        var existingCampaignProduct = new CampaignProduct
        {
            Offer = 5,
            ProductId = productId,
            CampaignId = existingCampaign.Id,
            Campaign = existingCampaign,
            CampaignProductLocations = [],
        };
        existingCampaign.CampaignProducts.Add(existingCampaignProduct);

        var campaignAdded = false;
        var campaignProductAdded = false;
        var campaignProductLocationAdded = false;

        var context = new CreateCampaignTestContext
        {
            Commit = () => { },
            GetCampaignByExternalId = externalId =>
                externalId == existingCampaign.ExternalId ? existingCampaign : null,
            GetCampaignProductByCampaignIdAndProductId = (campaignId, productId) =>
                campaignId == existingCampaignProduct.CampaignId && productId == existingCampaignProduct.ProductId
                ? existingCampaignProduct
                : null,
            GetCampaignProductLocationByCampaignProductIdAndLocationId = (_, _) => null,
            AddCampaign = _ => campaignAdded = true,
            AddCampaignProduct = _ => campaignProductAdded = true,
            AddCampaignProductLocation = _ => campaignProductLocationAdded = true,
        };

        var dto = new CampaignDto { ExternalId = externalId, Name = name, ProductId = productId, LocationId = locationId, Offer = offer };
        AddCampaign(context, dto);

        Assert.False(campaignAdded);
        Assert.False(campaignProductAdded);
        Assert.True(campaignProductLocationAdded);

        Assert.Equal(name, existingCampaign.Name);
        Assert.Equal(offer, existingCampaignProduct.Offer);
    }
}
