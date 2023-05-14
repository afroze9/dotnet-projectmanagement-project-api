using Moq;
using ProjectManagement.ProjectAPI.Domain.Entities;
using ProjectManagement.ProjectAPI.Domain.Entities.Events;

namespace ProjectManagement.ProjectAPI.UnitTests.Domain.Entities.Events;

public class ItemCompletedEventTests
{
    [Fact]
    public void Constructor_SetsItemProperty()
    {
        // Arrange
        Mock<TodoItem>? mockTodoItem = new ();
        mockTodoItem.SetupAllProperties();

        // Act
        ItemCompletedEvent? itemCompletedEvent = new (mockTodoItem.Object);

        // Assert
        Assert.Equal(mockTodoItem.Object, itemCompletedEvent.Item);
    }
}