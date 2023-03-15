using ProjectManagement.ProjectAPI.Common;

namespace ProjectManagement.ProjectAPI.Abstractions;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<EntityBase> entitiesWithEvents);
}