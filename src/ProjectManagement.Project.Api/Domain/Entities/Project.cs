using ProjectManagement.ProjectAPI.Abstractions;
using ProjectManagement.ProjectAPI.Common;
using ProjectManagement.ProjectAPI.Domain.Entities.Events;

namespace ProjectManagement.ProjectAPI.Domain.Entities;

public class Project : EntityBase, IAggregateRoot, IAuditable<string>
{
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = string.Empty;

    public DateTime ModifiedOn { get; set; }

    public int? CompanyId { get; set; }
    
    public string Name { get; private set; }

    private readonly List<TodoItem> _todoItems = new ();

    public IEnumerable<TodoItem> TodoItems => _todoItems.AsReadOnly();

    public Priority Priority { get; private set; }

    public ProjectStatus Status =>
        _todoItems.All(x => x.IsCompleted) ? ProjectStatus.Completed :
        _todoItems.All(x => !x.IsCompleted) ? ProjectStatus.NotStarted : ProjectStatus.InProgress;

    public Project(string name, Priority priority, int? companyId)
    {
        Name = name;
        Priority = priority;
        CompanyId = companyId;
    }

    public void AddTodoItem(TodoItem item)
    {
        _todoItems.Add(item);

        NewItemAddedEvent @event = new (this, item);
        RegisterDomainEvent(@event);
    }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdatePriority(Priority priority)
    {
        Priority = priority;
    }
}