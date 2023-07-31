namespace KittyWorks.Coop.Web.Domain;

public static class LocationModule
{
    public interface IAddLocationContext
    {
        Action Commit { get; }
        Action<Location> AddLocation { get; }
    }

    public static void AddLocation(IAddLocationContext context, string gln)
    {
        var location = new Location
        {
            Gln = gln,
        };

        context.AddLocation(location);
        context.Commit();
    }


    public interface IDeleteLocationContext
    {
        Action Commit { get; }
        Action<Guid> DeleteLocationById { get; }
    }

    public static void DeleteLocation(IDeleteLocationContext context, Guid locationId)
    {
        context.DeleteLocationById(locationId);
        context.Commit();
    }
}
