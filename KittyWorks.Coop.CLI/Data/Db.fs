module Db

open Product
open Location
open Campaign
open RelexPromo

open System.Collections.Generic

let mutable private id = 0
let generateId () = id <- id + 1; id

let products = new List<Product>()
products.Add({ Id = generateId(); Sku = "sku1" })
products.Add({ Id = generateId(); Sku = "sku2" })
products.Add({ Id = generateId(); Sku = "sku3" })

let locations = new List<Location>()
locations.Add({ Id = generateId(); Gln = "gln1" })
locations.Add({ Id = generateId(); Gln = "gln2" })
locations.Add({ Id = generateId(); Gln = "gln3" })

let campaigns = new List<Campaign>()
let campaignProducts = new List<CampaignProduct>()
let campaignProductLocations = new List<CampaignProductLocation>()

let getExistingSortedCampaigns (): CampaignDto seq =
    campaigns
        |> Seq.map (fun c -> campaignProducts |> Seq.filter (fun cp -> cp.CampaignId = c.Id) |> Seq.map (fun cp -> (c, cp))) |> Seq.concat
        |> Seq.map (fun (c, cp) -> campaignProductLocations |> Seq.filter (fun cpl -> cpl.CampaignProductId = cp.Id) |> Seq.map (fun cpl -> (c, cp, cpl))) |> Seq.concat
        |> Seq.map (fun (c, cp, cpl) -> {
            CampaignId = c.ExternalId
            CampaignName = c.Name
            ProductCode = (products |> Seq.find (fun p -> p.Id = cp.ProductId)).Sku
            Offer = cp.Offer
            LocationCode = (locations |> Seq.find (fun l -> l.Id = cpl.LocationId)).Gln
        })
        |> Seq.sortBy (fun dto -> dto.CampaignId, dto.ProductCode, dto.LocationCode) 
let getLocationIdByLocationGln gln: int = (locations |> Seq.find (fun l -> l.Gln = gln)).Id
let getProductIdByProductSku sku: int = (products |> Seq.find(fun p -> p.Sku = sku)).Id
let tryGetCampaignByRelexId relexId: Campaign option = campaigns |> Seq.tryFind (fun c -> c.ExternalId = relexId)
let tryGetCampaignProductBySku sku: CampaignProduct option = campaignProducts |> Seq.tryFind (fun cp -> cp.ProductId = getProductIdByProductSku sku)