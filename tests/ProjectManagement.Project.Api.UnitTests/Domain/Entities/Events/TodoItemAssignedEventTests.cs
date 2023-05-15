using Moq;
using ProjectManagement.ProjectAPI.Domain.Entities;
using ProjectManagement.ProjectAPI.Domain.Entities.Events;

namespace ProjectManagement.ProjectAPI.UnitTests.Domain.Entities.Events;

[ExcludeFromCodeCoverage]
public class TodoItemAssignedEventTests
{
    [Fact]
    public void Constructor_Sets_TodoItem_And_AssignedToId_Properties()
    {
        // Arrange
        Mock<TodoItem>? mockTodoItem = new ();
        string? assignedToId = "user123";

        // Act
        TodoItemAssignedEvent? todoItemAssignedEvent = new (mockTodoItem.Object, assignedToId);

        // Assert
        Assert.Equal(mockTodoItem.Object, todoItemAssignedEvent.TodoItem);
        Assert.Equal(assignedToId, todoItemAssignedEvent.AssignedToId);
    }
}