using ProjectManagement.ProjectAPI.Domain.Entities;

namespace ProjectManagement.ProjectAPI.UnitTests.Domain.Entities;

[ExcludeFromCodeCoverage]
public class ProjectTests
{
    [Fact]
    public void ProjectProperties_ShouldReturnExpectedValues()
    {
        // Arrange
        string projectName = "Test Project";
        Priority priority = Priority.Low;
        int? companyId = null;
        string createdBy = "created by";
        string modifiedBy = "modified by";
        DateTime createdOn = DateTime.UtcNow;
        DateTime modifiedOn = DateTime.UtcNow;

        // Ensure the constructors correctly set values
        Project project = new (projectName, priority, companyId);
        project.CreatedBy = createdBy;
        project.ModifiedBy = modifiedBy;
        project.CreatedOn = createdOn;
        project.ModifiedOn = modifiedOn;

        // Assert
        Assert.Equal(projectName, project.Name);
        Assert.Equal(priority, project.Priority);
        Assert.Equal(companyId, project.CompanyId);
        Assert.Equal(createdBy, project.CreatedBy);
        Assert.Equal(createdOn, project.CreatedOn);
        Assert.Equal(modifiedBy, project.ModifiedBy);
        Assert.Equal(modifiedOn, project.ModifiedOn);

        // Change values
        string newName = "New Test Project";
        Priority newPriority = Priority.High;

        // Act
        project.UpdateName(newName);
        project.UpdatePriority(newPriority);

        // Assert
        Assert.Equal(newName, project.Name);
        Assert.Equal(newPriority, project.Priority);
    }

    [Fact]
    public void Constructor_ShouldInstantiateNewProject_WithGivenParameters()
    {
        // Arrange
        string projectName = "Test Project";
        Priority priority = Priority.Low;
        int? companyId = null;

        // Act
        Project project = new (projectName, priority, companyId);

        // Assert
        Assert.Equal(projectName, project.Name);
        Assert.Equal(priority, project.Priority);
        Assert.Equal(companyId, project.CompanyId);
    }

    [Fact]
    public void AddTodoItem_ShouldAddNewItemToTodoItemsList_AndRegisterEvent()
    {
        // Arrange
        TodoItem todoItem = new TodoItem { Title = "Test Todo Item" };
        Project project = new Project("Test Project", Priority.High, null);

        // Act
        project.AddTodoItem(todoItem);

        // Assert
        Assert.Single(project.TodoItems);
        Assert.Contains(todoItem, project.TodoItems);
    }

    [Fact]
    public void UpdateName_ShouldChangeNameToGivenValue()
    {
        // Arrange
        Project project = new ("Old Name", Priority.High, null);
        string newName = "New Name";

        // Act
        project.UpdateName(newName);

        // Assert
        Assert.Equal(newName, project.Name);
    }

    [Fact]
    public void UpdatePriority_ShouldChangePriorityToGivenValue()
    {
        // Arrange
        Project project = new ("Test Project", Priority.Low, null);
        Priority newPriority = Priority.High;

        // Act
        project.UpdatePriority(newPriority);

        // Assert
        Assert.Equal(newPriority, project.Priority);
    }

    [Fact]
    public void ProjectStatus_WhenAllTodosComplete_ShouldReturnCompleted()
    {
        // Arrange
        TodoItem todoItem1 = new TodoItem { Title = "Test Todo Item 1" };
        todoItem1.MarkComplete();

        TodoItem todoItem2 = new TodoItem { Title = "Test Todo Item 2" };
        todoItem2.MarkComplete();

        Project project = new Project("Test Project", Priority.High, null);
        project.AddTodoItem(todoItem1);
        project.AddTodoItem(todoItem2);

        // Act
        ProjectStatus projectStatus = project.Status;

        // Assert
        Assert.Equal(ProjectStatus.Completed, projectStatus);
    }

    [Fact]
    public void ProjectStatus_WhenSomeTodosComplete_ShouldReturnInProgress()
    {
        // Arrange
        TodoItem todoItem1 = new TodoItem { Title = "Test Todo Item 1" };
        todoItem1.MarkComplete();

        TodoItem todoItem2 = new TodoItem { Title = "Test Todo Item 2" };

        Project project = new Project("Test Project", Priority.High, null);
        project.AddTodoItem(todoItem1);
        project.AddTodoItem(todoItem2);

        // Act
        ProjectStatus projectStatus = project.Status;

        // Assert
        Assert.Equal(ProjectStatus.InProgress, projectStatus);
    }

    [Fact]
    public void ProjectStatus_WhenNoTodosComplete_ShouldReturnNotStarted()
    {
        // Arrange
        TodoItem todoItem1 = new TodoItem { Title = "Test Todo Item 1" };
        TodoItem todoItem2 = new TodoItem { Title = "Test Todo Item 2" };

        Project project = new Project("Test Project", Priority.High, null);
        project.AddTodoItem(todoItem1);
        project.AddTodoItem(todoItem2);

        // Act
        ProjectStatus projectStatus = project.Status;

        // Assert
        Assert.Equal(ProjectStatus.NotStarted, projectStatus);
    }
}