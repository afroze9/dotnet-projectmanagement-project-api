using ProjectManagement.ProjectAPI.Common;

namespace ProjectManagement.ProjectAPI.Domain.Entities.Events;

public class ItemCompletedEvent : DomainEventBase
{
    public TodoItem Item { get; }

    public ItemCompletedEvent(TodoItem item)
    {
        Item = item;
    }
}