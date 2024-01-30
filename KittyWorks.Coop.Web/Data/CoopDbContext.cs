using KittyWorks.Coop.Web.Domain.Campaign;
using KittyWorks.Coop.Web.Domain.Location;
using KittyWorks.Coop.Web.Domain.Product;
using Microsoft.EntityFrameworkCore;

namespace KittyWorks.Coop.Web.Data;

public class CoopDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<CampaignProduct> CampaignProducts => Set<CampaignProduct>();
    public DbSet<CampaignProductLocation> CampaignProductLocations => Set<CampaignProductLocation>();

    public CoopDbContext(DbContextOptions<CoopDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(e =>
        {
            e.HasData(new[]
            {
                new { Id = Guid.Parse("c9c41700-a061-4a11-bdb6-b770f7287382"), Sku = "toode 1" },
                new { Id = Guid.Parse("fd264cf1-0bb2-4e67-b75f-ac7a26192bbc"), Sku = "toode 2" },
                new { Id = Guid.Parse("47531129-8502-455f-aba1-7002c026921c"), Sku = "toode 3" },
            });
        });

        modelBuilder.Entity<Location>(e =>
        {
            e.HasData(new[]
            {
                new { Id = Guid.Parse("e8c38d14-2848-46bb-a97d-0bccc3b01111"), Gln = "asukoht 1" },
                new { Id = Guid.Parse("8b35bcef-9540-4e2e-9d26-414c32e1dfb1"), Gln = "asukoht 2" },
                new { Id = Guid.Parse("0e728f04-9269-430b-9ea8-c3c7e6a2840f"), Gln = "asukoht 3" },
            });
        });

        modelBuilder.Entity<Campaign>(e =>
        {
            e.HasData(new[]
            {
                new { Id = Guid.Parse("8cb7211c-e51d-477d-a9aa-c1ce4603734b"), Name = "kampaania 1", ExternalId = "kampaania 1" },
                new { Id = Guid.Parse("bb2ca9d6-2a05-4bbb-ae7c-79a67c7153a8"), Name = "kampaania 2", ExternalId = "kampaania 2" },
            });
        });

        modelBuilder.Entity<CampaignProduct>(e =>
        {
            e.HasData(new[]
            {
                new { Id = Guid.Parse("3e370673-5cbb-48c6-af7f-4903e62792e1"), Offer = 11.11m, CampaignId = Guid.Parse("8cb7211c-e51d-477d-a9aa-c1ce4603734b"), ProductId = Guid.Parse("c9c41700-a061-4a11-bdb6-b770f7287382") },
                new { Id = Guid.Parse("c3d69935-9b60-4e62-9af2-e90cd61ce20a"), Offer = 22.22m, CampaignId = Guid.Parse("8cb7211c-e51d-477d-a9aa-c1ce4603734b"), ProductId = Guid.Parse("fd264cf1-0bb2-4e67-b75f-ac7a26192bbc") },
                new { Id = Guid.Parse("c0674e8d-9a3a-4504-8fe7-45253dd4b2ec"), Offer = 33.33m, CampaignId = Guid.Parse("bb2ca9d6-2a05-4bbb-ae7c-79a67c7153a8"), ProductId = Guid.Parse("47531129-8502-455f-aba1-7002c026921c") },
            });
        });

        modelBuilder.Entity<CampaignProductLocation>(e =>
        {
            e.HasData(new[]
            {
                new { Id = Guid.Parse("17b7d7c7-624b-487a-b276-c0e880b15595"), CampaignProductId = Guid.Parse("3e370673-5cbb-48c6-af7f-4903e62792e1"), LocationId = Guid.Parse("e8c38d14-2848-46bb-a97d-0bccc3b01111") },
                new { Id = Guid.Parse("87325a98-6731-40d7-ada6-36be533bb7aa"), CampaignProductId = Guid.Parse("3e370673-5cbb-48c6-af7f-4903e62792e1"), LocationId = Guid.Parse("8b35bcef-9540-4e2e-9d26-414c32e1dfb1") },
                new { Id = Guid.Parse("c19ac22a-6c14-40c8-b162-42cf18bbebe4"), CampaignProductId = Guid.Parse("c3d69935-9b60-4e62-9af2-e90cd61ce20a"), LocationId = Guid.Parse("e8c38d14-2848-46bb-a97d-0bccc3b01111") },
                new { Id = Guid.Parse("7415b603-b10d-426b-8726-af27097161f7"), CampaignProductId = Guid.Parse("c0674e8d-9a3a-4504-8fe7-45253dd4b2ec"), LocationId = Guid.Parse("0e728f04-9269-430b-9ea8-c3c7e6a2840f") },
                new { Id = Guid.Parse("9047c99f-e8c6-4595-892b-027a6f83b07b"), CampaignProductId = Guid.Parse("c0674e8d-9a3a-4504-8fe7-45253dd4b2ec"), LocationId = Guid.Parse("e8c38d14-2848-46bb-a97d-0bccc3b01111") },
            });
        });
    }
}