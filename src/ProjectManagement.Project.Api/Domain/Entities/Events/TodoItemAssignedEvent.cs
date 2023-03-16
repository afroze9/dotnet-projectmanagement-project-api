using ProjectManagement.ProjectAPI.Common;

namespace ProjectManagement.ProjectAPI.Domain.Entities.Events;

public class TodoItemAssignedEvent : DomainEventBase
{
    public TodoItemAssignedEvent(TodoItem todoItem, string assignedToId)
    {
        TodoItem = todoItem;
        AssignedToId = assignedToId;
    }

    public TodoItem TodoItem { get; }

    public string AssignedToId { get; }
}