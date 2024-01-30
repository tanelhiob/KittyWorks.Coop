
type CampaignProductLocation = {
    Id: int
    LocationId: int
}

type CampaignProduct = {
    Id: int
    CampaignProductLocations: CampaignProductLocation list
}

let groupCampaignProductsByLocations (campaignProducts: CampaignProduct list) =    
    campaignProducts |> List.groupBy (fun cp -> cp.CampaignProductLocations |> List.map (fun cpl -> cpl.LocationId))



let campaignProducts = [
    {
        Id = 1
        CampaignProductLocations = [
            { Id = 1; LocationId = 1 }
            { Id = 2; LocationId = 2 }
            { Id = 3; LocationId = 3 }
            { Id = 4; LocationId = 4 }
        ]
    }
    {
        Id = 2
        CampaignProductLocations = [
            { Id = 7; LocationId = 3 }
            { Id = 8; LocationId = 4 }
            { Id = 9; LocationId = 5 }
            { Id = 10; LocationId = 6 }
        ]
    }
    {
        Id = 3
        CampaignProductLocations = [
            { Id = 15; LocationId = 1 }
            { Id = 16; LocationId = 2 }
            { Id = 17; LocationId = 3 }
            { Id = 18; LocationId = 4 }
        ]
    }
]

let result = groupCampaignProductsByLocations campaignProducts