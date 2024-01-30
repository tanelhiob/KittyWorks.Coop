using KittyWorks.Coop.Web.Domain.Location;
using KittyWorks.Coop.Web.Domain.Product;

namespace KittyWorks.Coop.Web.Domain.Campaign;

public static partial class CampaignModule
{
    public interface IAddCampaignContext
        : ICreateCampaignFromDtoContext
        , ICreateCampaignProductFromDtoContext
        , ICreateCampaignProductLocationFromDtoContext
    {
        Action Commit { get; }
        Func<string, Campaign?> GetCampaignByExternalId { get; }
        Func<Guid, Guid, CampaignProduct?> GetCampaignProductByCampaignIdAndProductId { get; }
        Func<Guid, Guid, CampaignProductLocation?> GetCampaignProductLocationByCampaignProductIdAndLocationId { get; }
    }

    public interface ICreateCampaignFromDtoContext
    {
        Action<Campaign> AddCampaign { get; }
    }

    public interface ICreateCampaignProductFromDtoContext
    {
        Action<CampaignProduct> AddCampaignProduct { get; }
    }

    public interface ICreateCampaignProductLocationFromDtoContext
    {
        Action<CampaignProductLocation> AddCampaignProductLocation { get; }
    }

    public interface IDeleteCampaignContext
    {
        Action Commit { get; }
        Action<Guid> DeleteCampaignWithCampaignProductsAndCampaignProductLocations { get; }
    }

    public interface IDeleteCampaignProductContext
    {
        Action Commit { get; }
        Func<Guid, (CampaignProduct, Campaign)> GetCampaignProductWithCampaign { get; }
        Action<Guid> DeleteCampaignProductWithCampaignProductLocations { get; }
        Action<Guid> DeleteCampaignIfNoCampaignProducts { get; }
    }

    public interface IDeleteCampaignProductLocationContext
    {
        Action Commit { get; }
        Func<Guid, (CampaignProductLocation, CampaignProduct, Campaign)> GetCampaignProductLocationWithCampaingProductAndCampaign { get; }
        Action<Guid> DeleteCampaignProductLocation { get; }
        Action<Guid> DeleteCampaignProductIfNoCampaignProductLocations { get; }
        Action<Guid> DeleteCampaignIfNoCampaignProducts { get; }
    }

    public interface IDeleteProductWithRelatedCampaignProducts
        : ProductModule.IDeleteProductContext
    {
        Func<Guid, List<Guid>> GetCampaignIdsByProductId { get; }
        Action<Guid> DeleteCampaignProductsWithChildrenByProductId { get; }
        Action<List<Guid>> DeleteCampaignsIfNoCampaignProductsByIds { get; }
    }

    public interface IDeleteLocationWithRelatedCampaignProductLocations
        : LocationModule.IDeleteLocationContext
    {
        Action<Guid> DeleteCampaignProductLocationsByLocationId { get; }
        Func<Guid, List<(Guid CampaignProductId, Guid CampaignId)>> GetCampaignProductIdsWithCampaignIdsByLocationId { get; }
        Action<List<Guid>> DeleteCampaignProductsIfNoCampaignProductLocationsByIds { get; }
        Action<List<Guid>> DeleteCampaignsIfNoCampaignProductsByIds { get; }
    }
}
