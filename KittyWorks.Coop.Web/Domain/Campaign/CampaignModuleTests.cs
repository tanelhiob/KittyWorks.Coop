using NSubstitute;
using Xunit;

namespace KittyWorks.Coop.Web.Domain.Campaign;

public static partial class CampaignModule
{
    class CreateCampaignTestContext : IAddCampaignContext
    {
        public required Action Commit { get; init; }
        public required Func<string, Campaign> GetCampaignByExternalId { get; init; }
        public required Func<Guid, Guid, CampaignProduct> GetCampaignProductByCampaignIdAndProductId { get; init; }
        public required Func<Guid, Guid, CampaignProductLocation> GetCampaignProductLocationByCampaignProductIdAndLocationId { get; init; }
        public required Action<Campaign> AddCampaign { get; init; }
        public required Action<CampaignProduct> AddCampaignProduct { get; init; }
        public required Action<CampaignProductLocation> AddCampaignProductLocation { get; init; }
    }

    [Theory]
    [InlineData("1", "kampaania", "8ca1b475-5f39-4757-aa7e-e86b7968ecc4", "13928890-baf6-4731-9bfd-65c87e4723c4", 10)]
    static void CreateCampaignTest(string externalId, string name, Guid productId, Guid locationId, decimal offer)
    {
        var addEntityMock = Substitute.For<Action<Entity>>();
        var commitMock = Substitute.For<Action>();
        var getCampaignByExternalIdMock = Substitute.For<Func<string, Campaign>>();
        var getCampaignProductByCampaignIdAndProductIdMock = Substitute.For<Func<Guid, Guid, CampaignProduct>>();
        var getCampaignProductLocationByCampaignProductIdAndLocationIdMock = Substitute.For<Func<Guid, Guid, CampaignProductLocation>>();

        var context = new CreateCampaignTestContext
        {
            Commit = commitMock,
            GetCampaignByExternalId = getCampaignByExternalIdMock,
            GetCampaignProductByCampaignIdAndProductId = getCampaignProductByCampaignIdAndProductIdMock,
            GetCampaignProductLocationByCampaignProductIdAndLocationId = getCampaignProductLocationByCampaignProductIdAndLocationIdMock,
            AddCampaign = addEntityMock,
            AddCampaignProduct = addEntityMock,
            AddCampaignProductLocation = addEntityMock,
        };

        var dto = new CampaignDto { ExternalId = externalId, Name = name, ProductId = productId, LocationId = locationId, Offer = offer };
        AddCampaign(context, dto);

        addEntityMock.Received(1).Invoke(Arg.Is<Campaign>(c => c.ExternalId == "1"));
        addEntityMock.Received(1).Invoke(Arg.Is<CampaignProduct>(cp => cp.ProductId == productId));
        addEntityMock.Received(1).Invoke(Arg.Is<CampaignProductLocation>(cpl => cpl.LocationId == locationId));
        commitMock.Received(1).Invoke();
    }
}
