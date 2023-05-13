using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.ProjectAPI.Common;

/// <summary>
///     A base class for entities.
/// </summary>
public abstract class EntityBase
{
    private readonly List<DomainEventBase> _domainEvents = new ();

    /// <summary>
    ///     The identifier of the entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The collection of domain events associated with the entity.
    /// </summary>
    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    ///     Registers a specific domain event with the entity.
    /// </summary>
    /// <param name="domainEvent">The domain event to be registered.</param>
    protected void RegisterDomainEvent(DomainEventBase domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    ///     Clears the collection of domain events associated with the entity.
    /// </summary>
    internal void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}