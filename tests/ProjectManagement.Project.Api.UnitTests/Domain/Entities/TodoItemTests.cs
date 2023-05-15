using ProjectManagement.ProjectAPI.Domain.Entities;

namespace ProjectManagement.ProjectAPI.UnitTests.Domain.Entities;

[ExcludeFromCodeCoverage]
public class TodoItemTests
{
    [Fact]
    public void AllPropertiesTest()
    {
        // Arrange
        DateTime date = DateTime.UtcNow;
        TodoItem todoItem = new TodoItem
        {
            Id = 1,
            Title = "Test Title",
            Description = "Test Description",
            AssignedToId = "Test Assignee",
            CreatedBy = "Test Creator",
            CreatedOn = date,
            ModifiedBy = "Test Modifier",
            ModifiedOn = date,
        };

        // Act
        todoItem.MarkComplete();
        todoItem.AssignTodoItem("New Test Assignee");

        // Assert
        Assert.True(todoItem.IsCompleted);
        Assert.Equal(1, todoItem.Id);
        Assert.Equal("New Test Assignee", todoItem.AssignedToId);
        Assert.Equal("Test Title", todoItem.Title);
        Assert.Equal("Test Description", todoItem.Description);
        Assert.Equal("Test Creator", todoItem.CreatedBy);
        Assert.Equal("Test Modifier", todoItem.ModifiedBy);
        Assert.Equal(date, todoItem.CreatedOn);
        Assert.Equal(date, todoItem.ModifiedOn);
    }

    [Fact]
    public void MarkComplete_WhenTodoItemIsNotCompleted_ShouldSetIsCompletedToTrueAndRaiseEvent()
    {
        // Arrange
        TodoItem todoItem = new ()
            { Title = "T1" };

        // Act
        todoItem.MarkComplete();

        // Assert
        Assert.True(todoItem.IsCompleted);
        Assert.Single(todoItem.DomainEvents);
    }

    [Fact]
    public void AssignTodoItem_ShouldSetAssignedToIdAndRaiseEvent()
    {
        // Arrange
        TodoItem todoItem = new ()
            { Title = "T1" };

        string assignedToId = "testId";

        // Act
        todoItem.AssignTodoItem(assignedToId);

        // Assert
        Assert.Equal(assignedToId, todoItem.AssignedToId);
        Assert.Single(todoItem.DomainEvents);
    }
}