namespace KittyWorks.Coop.Web.Domain;

public abstract class Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
}
