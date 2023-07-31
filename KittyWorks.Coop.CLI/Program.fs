open RelexPromo

type ApplicationContext() =
    interface IImportCampaignsContext with 
        member _.getExistingSortedCampaigns() = Db.getExistingSortedCampaigns()
        member _.getImportedSortedCampaigns() = getImportedSortedCampaigns()
    interface ICreateCampaignProductLocationFromDtoContext with
        member _.generateNextCampaignProductLocationId() = Db.generateId()
        member _.getLocationIdByLocationGln gln = Db.getLocationIdByLocationGln gln
    interface ICreateCampaignProductFromDtoContext with
        member _.generateNextCampaignProductId() = Db.generateId()
        member _.getProductIdByProductSku sku = Db.getProductIdByProductSku sku
    interface ICreateCampaignFromDtoContext with
        member _.generateNextCampaignId() = Db.generateId()
    interface IAddContext with
        member _.tryGetCampaignByRelexId relexId = Db.tryGetCampaignByRelexId relexId
        member _.tryGetCampaignProductBySku sku = Db.tryGetCampaignProductBySku sku
        member _.addCampaign campaign = Db.campaigns.Add campaign
        member _.addCampaignProduct campaignProduct = Db.campaignProducts.Add campaignProduct
        member _.addCampaignProductLocation campaignProductLocation = Db.campaignProductLocations.Add campaignProductLocation
        member _.saveChanges() = ignore()
    interface IUpdateContext with
        member _.saveChanges() = ignore()
    interface IDeleteContext with
        member _.saveChanges() = ignore()
    interface ICleanupContext with
        member _.saveChanges() = ignore()

importCampaigns (ApplicationContext())