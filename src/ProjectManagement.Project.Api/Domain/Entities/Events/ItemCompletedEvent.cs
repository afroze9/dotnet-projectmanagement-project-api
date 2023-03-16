using ProjectManagement.ProjectAPI.Common;

namespace ProjectManagement.ProjectAPI.Domain.Entities.Events;

public class ItemCompletedEvent : DomainEventBase
{
    public ItemCompletedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}