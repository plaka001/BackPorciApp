namespace Dominio.Abstractions;

public abstract class Entity<TEntityId> : IDomainEventSource, IEntity
{


    protected Entity()
    {

    }

    protected Entity(TEntityId id)
    {
        Id = id;
    }

    public TEntityId? Id { get; init; }

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
