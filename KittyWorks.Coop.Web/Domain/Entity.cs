namespace KittyWorks.Coop.Web.Domain;

public abstract record Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
}
