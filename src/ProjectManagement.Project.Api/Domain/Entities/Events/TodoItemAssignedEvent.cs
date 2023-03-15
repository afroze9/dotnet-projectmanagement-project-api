using ProjectManagement.ProjectAPI.Common;

namespace ProjectManagement.ProjectAPI.Domain.Entities.Events;

public class TodoItemAssignedEvent : DomainEventBase
{
    public TodoItem TodoItem { get; }

    public string AssignedToId { get; }

    public TodoItemAssignedEvent(TodoItem todoItem, string assignedToId)
    {
        TodoItem = todoItem;
        AssignedToId = assignedToId;
    }
}