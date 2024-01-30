using KittyWorks.Coop.Web.Data;
using KittyWorks.Coop.Web.Domain.Campaign;
using KittyWorks.Coop.Web.Domain.Location;
using KittyWorks.Coop.Web.Domain.Product;
using Microsoft.EntityFrameworkCore;

namespace KittyWorks.Coop.Web;

public record ApplicationContext
    : CampaignModule.IAddCampaignContext
    , CampaignModule.IDeleteCampaignContext
    , CampaignModule.IDeleteCampaignProductContext
    , CampaignModule.IDeleteCampaignProductLocationContext
    , CampaignModule.IDeleteProductWithRelatedCampaignProducts
    , CampaignModule.IDeleteLocationWithRelatedCampaignProductLocations
    , LocationModule.IAddLocationContext
    , ProductModule.IAddProductContext
{
    public ApplicationContext(CoopDbContext db)
    {
        Commit = () => db.SaveChanges();
        AddCampaign = entity => db.Add(entity);
        AddCampaignProduct = entity => db.Add(entity);
        AddCampaignProductLocation = entity => db.Add(entity);
        GetCampaignByExternalId = externalId => db.Campaigns.FirstOrDefault(c => c.ExternalId == externalId);
        GetCampaignProductByCampaignIdAndProductId = (campaignId, productId) => db.CampaignProducts.FirstOrDefault(cp => cp.CampaignId == campaignId && cp.ProductId == productId);
        GetCampaignProductLocationByCampaignProductIdAndLocationId = (campaignProductId, locationId) => db.CampaignProductLocations.FirstOrDefault(cpl => cpl.CampaignProductId == campaignProductId && cpl.LocationId == locationId);
        GetCampaignProductWithCampaign = campaignProductId =>
        {
            var campaignProduct = db.CampaignProducts
                .Include(x => x.Campaign)
                .First(x => x.Id == campaignProductId);
            return (campaignProduct, campaignProduct.Campaign);
        };
        GetCampaignProductLocationWithCampaingProductAndCampaign = campaignProductLocationId =>
        {
            var campaignProductLocation = db.CampaignProductLocations
                .Include(x => x.CampaignProduct).ThenInclude(x => x.Campaign)
                .First(x => x.Id == campaignProductLocationId);
            return (campaignProductLocation, campaignProductLocation.CampaignProduct, campaignProductLocation.CampaignProduct.Campaign);
        };
        DeleteCampaignIfNoCampaignProducts = campaignId => db.RemoveRange(db.Campaigns.Where(x => x.Id == campaignId && x.CampaignProducts.Count == 0));
        DeleteCampaignProductIfNoCampaignProductLocations = campaignProductId => db.RemoveRange(db.CampaignProducts.Where(x => x.Id == campaignProductId && x.CampaignProductLocations.Count == 0));
        DeleteCampaignProductLocation = campaignProductLocationId => db.RemoveRange(db.CampaignProductLocations.Where(x => x.Id == campaignProductLocationId));
        DeleteCampaignProductWithCampaignProductLocations = campaignProductId =>
        {
            db.RemoveRange(db.CampaignProductLocations.Where(x => x.CampaignProductId == campaignProductId));
            db.RemoveRange(db.CampaignProducts.Where(x => x.Id == campaignProductId));
        };
        DeleteCampaignWithCampaignProductsAndCampaignProductLocations = campaignId =>
        {
            db.RemoveRange(db.CampaignProductLocations.Where(x => x.CampaignProduct.CampaignId == campaignId));
            db.RemoveRange(db.CampaignProducts.Where(x => x.CampaignId == campaignId));
            db.RemoveRange(db.Campaigns.Where(x => x.Id == campaignId));
        };
        GetCampaignIdsByProductId = productId => db.CampaignProducts.Where(x => x.ProductId == productId).Select(x => x.CampaignId).ToList();
        DeleteCampaignProductsWithChildrenByProductId = productId =>
        {
            db.RemoveRange(db.CampaignProductLocations.Where(x => x.CampaignProduct.ProductId == productId));
            db.RemoveRange(db.CampaignProducts.Where(x => x.ProductId == productId));
        };
        DeleteCampaignsIfNoCampaignProductsByIds = campaignIds => db.RemoveRange(db.Campaigns.Where(x => campaignIds.Contains(x.Id) && x.CampaignProducts.Count == 0));
        DeleteProductById = productId => db.RemoveRange(db.Products.Where(x => x.Id == productId));
        DeleteCampaignProductLocationsByLocationId = locationId => db.RemoveRange(db.CampaignProductLocations.Where(x => x.LocationId == locationId));
        GetCampaignProductIdsWithCampaignIdsByLocationId = locationId => db.CampaignProductLocations
            .Where(x => x.LocationId == locationId)
            .Select(x => new { CampaignProductId = x.CampaignProduct.Id, x.CampaignProduct.CampaignId })
            .ToList()
            .Select(x => (x.CampaignProductId, x.CampaignId))
            .ToList();
        DeleteCampaignProductsIfNoCampaignProductLocationsByIds = productIds => db.RemoveRange(db.CampaignProducts.Where(x => productIds.Contains(x.Id) && x.CampaignProductLocations.Count == 0));
        DeleteLocationById = locationId => db.RemoveRange(db.Locations.Where(x => x.Id == locationId));
        AddLocation = location => db.Locations.Add(location);

        AddProduct = (Product product) => db.Products.Add(product);
    }

    public Action Commit { get; }
    public Func<string, Campaign?> GetCampaignByExternalId { get; }
    public Func<Guid, Guid, CampaignProduct?> GetCampaignProductByCampaignIdAndProductId { get; }
    public Func<Guid, Guid, CampaignProductLocation?> GetCampaignProductLocationByCampaignProductIdAndLocationId { get; }
    public Action<Campaign> AddCampaign { get; }
    public Action<CampaignProduct> AddCampaignProduct { get; }
    public Action<CampaignProductLocation> AddCampaignProductLocation { get; }
    public Action<Guid> DeleteCampaignWithCampaignProductsAndCampaignProductLocations { get; }
    public Func<Guid, (CampaignProduct, Campaign)> GetCampaignProductWithCampaign { get; }
    public Action<Guid> DeleteCampaignProductWithCampaignProductLocations { get; }
    public Action<Guid> DeleteCampaignIfNoCampaignProducts { get; }
    public Func<Guid, (CampaignProductLocation, CampaignProduct, Campaign)> GetCampaignProductLocationWithCampaingProductAndCampaign { get; }
    public Action<Guid> DeleteCampaignProductLocation { get; }
    public Action<Guid> DeleteCampaignProductIfNoCampaignProductLocations { get; }
    public Func<Guid, List<Guid>> GetCampaignIdsByProductId { get; }
    public Action<Guid> DeleteCampaignProductsWithChildrenByProductId { get; }
    public Action<List<Guid>> DeleteCampaignsIfNoCampaignProductsByIds { get; }
    public Action<Guid> DeleteProductById { get; }
    public Action<Guid> DeleteCampaignProductLocationsByLocationId { get; }
    public Func<Guid, List<(Guid CampaignProductId, Guid CampaignId)>> GetCampaignProductIdsWithCampaignIdsByLocationId { get; }
    public Action<List<Guid>> DeleteCampaignProductsIfNoCampaignProductLocationsByIds { get; }
    public Action<Guid> DeleteLocationById { get; }
    public Action<Location> AddLocation { get; }
    public Action<Product> AddProduct { get; }
}