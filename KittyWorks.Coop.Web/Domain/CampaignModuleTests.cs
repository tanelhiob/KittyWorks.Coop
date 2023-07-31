using Moq;
using Xunit;

namespace KittyWorks.Coop.Web.Domain;

public static class CampaignModuleTests
{
    class CreateCampaignTestContext : CampaignModule.IAddCampaignContext
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
        var addEntityMock = new Mock<Action<Entity>>();
        var commitMock = new Mock<Action>();
        var getCampaignByExternalIdMock = new Mock<Func<string, Campaign>>();
        var getCampaignProductByCampaignIdAndProductIdMock = new Mock<Func<Guid, Guid, CampaignProduct>>();
        var getCampaignProductLocationByCampaignProductIdAndLocationIdMock = new Mock<Func<Guid, Guid, CampaignProductLocation>>();

        var context = new CreateCampaignTestContext
        {
            Commit = commitMock.Object,
            GetCampaignByExternalId = getCampaignByExternalIdMock.Object,
            GetCampaignProductByCampaignIdAndProductId = getCampaignProductByCampaignIdAndProductIdMock.Object,
            GetCampaignProductLocationByCampaignProductIdAndLocationId = getCampaignProductLocationByCampaignProductIdAndLocationIdMock.Object,
            AddCampaign = addEntityMock.Object,
            AddCampaignProduct = addEntityMock.Object,
            AddCampaignProductLocation = addEntityMock.Object,
        };

        var dto = new CampaignModule.CampaignDto { ExternalId = externalId, Name = name, ProductId = productId, LocationId = locationId, Offer = offer };
        CampaignModule.AddCampaign(context, dto);

        addEntityMock.Verify(x => x.Invoke(It.Is<Campaign>(c => c.ExternalId == "1")), Times.Once);
        addEntityMock.Verify(x => x.Invoke(It.Is<CampaignProduct>(cp => cp.ProductId == productId)), Times.Once);
        addEntityMock.Verify(x => x.Invoke(It.Is<CampaignProductLocation>(cpl => cpl.LocationId == locationId)), Times.Once);
        commitMock.Verify(x => x.Invoke(), Times.Once);
    }
}
