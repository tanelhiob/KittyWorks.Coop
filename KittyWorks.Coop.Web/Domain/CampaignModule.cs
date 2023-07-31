namespace KittyWorks.Coop.Web.Domain;

public static class CampaignModule
{
    public record CampaignDto
    {
        public required string ExternalId { get; init; }
        public required string Name { get; init; }
        public required Guid ProductId { get; init; }
        public required Guid LocationId { get; init; }
        public required decimal Offer { get; init; }
    }


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

    public static void AddCampaign(IAddCampaignContext context, CampaignDto dto)
    {
        var campaign = context.GetCampaignByExternalId(dto.ExternalId);
        if (campaign != null)
        {
            campaign.Name = dto.Name;

            var campaignProduct = context.GetCampaignProductByCampaignIdAndProductId(campaign.Id, dto.ProductId);
            if (campaignProduct != null)
            {
                campaignProduct.Offer = dto.Offer;

                var campaignProductLocation = context.GetCampaignProductLocationByCampaignProductIdAndLocationId(campaignProduct.Id, dto.LocationId);
                if (campaignProductLocation == null)
                {
                    CreateCampaignProductLocationFromDto(context, campaignProduct.Id, dto);
                }
            }
            else
            {
                var createdCampaignProduct = CreateCampaignProductFromDto(context, campaign.Id, dto);
                CreateCampaignProductLocationFromDto(context, createdCampaignProduct.Id, dto);
            }
        }
        else
        {
            var createdCampaign = CreateCampaignFromDto(context, dto);
            var createdCampaignProduct = CreateCampaignProductFromDto(context, createdCampaign.Id, dto);
            CreateCampaignProductLocationFromDto(context, createdCampaignProduct.Id, dto);
        }

        context.Commit();
    }


    public interface ICreateCampaignFromDtoContext
    {
        Action<Campaign> AddCampaign { get; }
    }

    public static Campaign CreateCampaignFromDto(ICreateCampaignFromDtoContext context, CampaignDto dto)
    {
        var campaign = new Campaign
        {
            Name = dto.Name,
            ExternalId = dto.ExternalId,
        };
        context.AddCampaign(campaign);

        return campaign;
    }


    public interface ICreateCampaignProductFromDtoContext
    {
        Action<CampaignProduct> AddCampaignProduct { get; }
    }

    public static CampaignProduct CreateCampaignProductFromDto(ICreateCampaignProductFromDtoContext context, Guid campaignId, CampaignDto dto)
    {
        var campaignProduct = new CampaignProduct
        {
            CampaignId = campaignId,
            ProductId = dto.ProductId,
            Offer = dto.Offer,
        };
        context.AddCampaignProduct(campaignProduct);

        return campaignProduct;
    }


    public interface ICreateCampaignProductLocationFromDtoContext
    {
        Action<CampaignProductLocation> AddCampaignProductLocation { get; }
    }

    public static CampaignProductLocation CreateCampaignProductLocationFromDto(ICreateCampaignProductLocationFromDtoContext context, Guid campaignProductId, CampaignDto dto)
    {
        var campaignProductLocation = new CampaignProductLocation
        {
            CampaignProductId = campaignProductId,
            LocationId = dto.LocationId,
        };
        context.AddCampaignProductLocation(campaignProductLocation);

        return campaignProductLocation;
    }


    public interface IDeleteCampaignContext
    {
        Action Commit { get; }
        Action<Guid> DeleteCampaignWithCampaignProductsAndCampaignProductLocations { get; }
    }

    public static void DeleteCampaign(IDeleteCampaignContext context, Guid campaignId)
    {
        context.DeleteCampaignWithCampaignProductsAndCampaignProductLocations(campaignId);
        context.Commit();
    }


    public interface IDeleteCampaignProductContext
    {
        Action Commit { get; }
        Func<Guid, (CampaignProduct, Campaign)> GetCampaignProductWithCampaign { get; }
        Action<Guid> DeleteCampaignProductWithCampaignProductLocations { get; }
        Action<Guid> DeleteCampaignIfNoCampaignProducts { get; }
    }

    public static void DeleteCampaignProduct(IDeleteCampaignProductContext context, Guid campaignProductId)
    {
        var (campaignProduct, campaign) = context.GetCampaignProductWithCampaign(campaignProductId);

        context.DeleteCampaignProductWithCampaignProductLocations(campaignProductId);
        context.Commit();

        context.DeleteCampaignIfNoCampaignProducts(campaign.Id);
        context.Commit();
    }


    public interface IDeleteCampaignProductLocationContext
    {
        Action Commit { get; }
        Func<Guid, (CampaignProductLocation, CampaignProduct, Campaign)> GetCampaignProductLocationWithCampaingProductAndCampaign { get; }
        Action<Guid> DeleteCampaignProductLocation { get; }
        Action<Guid> DeleteCampaignProductIfNoCampaignProductLocations { get; }
        Action<Guid> DeleteCampaignIfNoCampaignProducts { get; }
    }

    public static void DeleteCampaignProductLocation(IDeleteCampaignProductLocationContext context, Guid campaignProductLocationId)
    {
        var (campaignProductLocation, campaignProduct, campaign) = context.GetCampaignProductLocationWithCampaingProductAndCampaign(campaignProductLocationId);

        context.DeleteCampaignProductLocation(campaignProductLocation.Id);
        context.Commit();

        context.DeleteCampaignProductIfNoCampaignProductLocations(campaignProduct.Id);
        context.Commit();

        context.DeleteCampaignIfNoCampaignProducts(campaign.Id);
        context.Commit();
    }


    public interface IDeleteProductWithRelatedCampaignProducts
        : ProductModule.IDeleteProductContext
    {
        Func<Guid, List<Guid>> GetCampaignIdsByProductId { get; }
        Action<Guid> DeleteCampaignProductsWithChildrenByProductId { get; }
        Action<List<Guid>> DeleteCampaignsIfNoCampaignProductsByIds { get; }
    }

    public static void DeleteProductWithRelatedCampaignProducts(IDeleteProductWithRelatedCampaignProducts context, Guid productId)
    {
        var campaignIds = context.GetCampaignIdsByProductId(productId);

        context.DeleteCampaignProductsWithChildrenByProductId(productId);
        context.Commit();

        context.DeleteCampaignsIfNoCampaignProductsByIds(campaignIds);
        context.Commit();

        ProductModule.DeleteProduct(context, productId);            
    }


    public interface IDeleteLocationWithRelatedCampaignProductLocations
        : LocationModule.IDeleteLocationContext
    {
        Action<Guid> DeleteCampaignProductLocationsByLocationId { get; }
        Func<Guid, List<(Guid CampaignProductId, Guid CampaignId)>> GetCampaignProductIdsWithCampaignIdsByLocationId { get; }
        Action<List<Guid>> DeleteCampaignProductsIfNoCampaignProductLocationsByIds { get; }
        Action<List<Guid>> DeleteCampaignsIfNoCampaignProductsByIds { get; }
    }
    
    public static void DeleteLocationsWithRelatedCampaignProductLocations(IDeleteLocationWithRelatedCampaignProductLocations context, Guid locationId)
    {
        var campaignProductIdsWithCampaignIds = context.GetCampaignProductIdsWithCampaignIdsByLocationId(locationId);
        var campaignProductIds = campaignProductIdsWithCampaignIds.Select(x => x.CampaignProductId).ToList();
        var campaignIds = campaignProductIdsWithCampaignIds.Select(x => x.CampaignId).ToList();

        context.DeleteCampaignProductLocationsByLocationId(locationId);
        context.Commit();

        context.DeleteCampaignProductsIfNoCampaignProductLocationsByIds(campaignProductIds);
        context.Commit();

        context.DeleteCampaignsIfNoCampaignProductsByIds(campaignIds);
        context.Commit();

        LocationModule.DeleteLocation(context, locationId);
    }
}