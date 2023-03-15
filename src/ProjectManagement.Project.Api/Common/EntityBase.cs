using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.ProjectAPI.Common;

public abstract class EntityBase
{
    private readonly List<DomainEventBase> _domainEvents = new ();

    public int Id { get; set; }

    [NotMapped] public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    protected void RegisterDomainEvent(DomainEventBase domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    internal void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}