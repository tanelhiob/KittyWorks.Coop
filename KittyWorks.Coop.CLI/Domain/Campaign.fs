module Campaign

type Campaign = {
    Id: int
    Name: string
    ExternalId: string
}

type CampaignProduct = {
    Id: int
    CampaignId: int
    ProductId: int
    Offer: decimal
}

type CampaignProductLocation = {
    Id: int
    CampaignProductId: int
    LocationId: int
}