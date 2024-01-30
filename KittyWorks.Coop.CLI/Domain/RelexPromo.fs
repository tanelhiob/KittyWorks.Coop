module RelexPromo

open Campaign
open Utilities

type CampaignDto = {
    CampaignId: string
    CampaignName: string
    ProductCode: string
    Offer: decimal
    LocationCode: string
}

let compareCampaignDtos (left: CampaignDto) (right: CampaignDto) : int =
    0

type ICreateCampaignFromDtoContext =
    abstract generateNextCampaignId: unit -> int
let createCampaignFromDto (context: #ICreateCampaignFromDtoContext) (dto: CampaignDto): Campaign =
    { Id = context.generateNextCampaignId(); Name = dto.CampaignName; ExternalId = dto.CampaignId }

type ICreateCampaignProductFromDtoContext =
    abstract generateNextCampaignProductId: unit -> int
    abstract getProductIdByProductSku: string -> int
let createCampaignProductFromDto (context: #ICreateCampaignProductFromDtoContext) (campaignId: int) (dto: CampaignDto): CampaignProduct =
    { Id = context.generateNextCampaignProductId(); CampaignId = campaignId; ProductId = context.getProductIdByProductSku dto.ProductCode; Offer = dto.Offer }

type ICreateCampaignProductLocationFromDtoContext =
    abstract generateNextCampaignProductLocationId: unit -> int
    abstract getLocationIdByLocationGln: string -> int        
let createCampaignProductLocationFromDto (context: #ICreateCampaignProductLocationFromDtoContext) (campaignProductId: int) (dto: CampaignDto): CampaignProductLocation =
    { Id = context.generateNextCampaignProductLocationId(); CampaignProductId = campaignProductId; LocationId = context.getLocationIdByLocationGln dto.LocationCode }

type IAddContext =
    abstract tryGetCampaignByRelexId: string -> Campaign option
    abstract tryGetCampaignProductBySku: string -> CampaignProduct option    
    abstract addCampaign: Campaign -> unit
    abstract addCampaignProduct: CampaignProduct -> unit
    abstract addCampaignProductLocation: CampaignProductLocation -> unit
    abstract saveChanges: unit -> unit
let add (context: #IAddContext) (dto: CampaignDto): unit =
    match context.tryGetCampaignByRelexId dto.CampaignId with
    | Some campaign ->
        match context.tryGetCampaignProductBySku dto.ProductCode with
        | Some campaignProduct ->                       
            let campaignProductLocation = createCampaignProductLocationFromDto context campaignProduct.Id dto
            context.addCampaignProductLocation campaignProductLocation
        | None -> 
            let campaignProduct = createCampaignProductFromDto context campaign.Id dto
            let campaignProductLocation = createCampaignProductLocationFromDto context campaignProduct.Id dto
            context.addCampaignProduct campaignProduct
            context.addCampaignProductLocation campaignProductLocation
    | None ->
        let campaign = createCampaignFromDto context dto        
        let campaignProduct = createCampaignProductFromDto context campaign.Id dto        
        let campaignProductLocation = createCampaignProductLocationFromDto context campaignProduct.Id dto
        context.addCampaign campaign
        context.addCampaignProduct campaignProduct
        context.addCampaignProductLocation campaignProductLocation

    context.saveChanges()

type IUpdateContext =
    abstract saveChanges: unit -> unit
let update (context: #IUpdateContext) (existingDto: CampaignDto) (importedDto: CampaignDto): unit =
    context.saveChanges()

type IDeleteContext =
    abstract saveChanges: unit -> unit
let delete (context: #IDeleteContext) (dto: CampaignDto): unit =
    context.saveChanges()

type ICleanupContext =
    abstract saveChanges: unit -> unit
let cleanup (context: #ICleanupContext): unit =
    context.saveChanges()

type IImportCampaignsContext =
    abstract getExistingSortedCampaigns: unit -> CampaignDto seq
    abstract getImportedSortedCampaigns: unit -> CampaignDto seq

let importCampaigns (context: #IImportCampaignsContext) =
    let existingCampaigns = context.getExistingSortedCampaigns ()
    let importedCampaigns = context.getImportedSortedCampaigns ()
    let changes = compareSequences existingCampaigns importedCampaigns compareCampaignDtos

    for change in changes do
        match change with
        | Addition importedDto -> add context importedDto
        | Update (existingDto, importedDto) -> update context existingDto importedDto
        | Deletion existingDto -> delete context existingDto

    cleanup context

let getImportedSortedCampaigns (): CampaignDto seq =     
    Seq.empty

type FooEnv = {
    add: int -> int -> int
    divide: int -> int -> float
}

let bar env arg1 arg2 =
    let a = env.divide arg1 arg2
    a

let foo env arg1 arg2 =
    let a = env.add arg1 arg2
    let b = bar env a arg1
    b