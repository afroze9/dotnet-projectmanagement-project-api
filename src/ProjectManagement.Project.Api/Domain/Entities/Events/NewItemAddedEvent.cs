using ProjectManagement.ProjectAPI.Common;

namespace ProjectManagement.ProjectAPI.Domain.Entities.Events;

public class NewItemAddedEvent : DomainEventBase
{
    public NewItemAddedEvent(Project project, TodoItem item)
    {
        Project = project;
        Item = item;
    }

    public Project Project { get; }

    public TodoItem Item { get; }
}