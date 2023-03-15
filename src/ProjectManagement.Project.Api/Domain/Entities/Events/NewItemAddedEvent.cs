using ProjectManagement.ProjectAPI.Common;

namespace ProjectManagement.ProjectAPI.Domain.Entities.Events;

public class NewItemAddedEvent : DomainEventBase
{
    public Project Project { get; }

    public TodoItem Item { get; }

    public NewItemAddedEvent(Project project, TodoItem item)
    {
        Project = project;
        Item = item;
    }
}