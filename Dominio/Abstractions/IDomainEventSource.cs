namespace Dominio.Abstractions;

public interface IDomainEventSource
{
    IReadOnlyList<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}