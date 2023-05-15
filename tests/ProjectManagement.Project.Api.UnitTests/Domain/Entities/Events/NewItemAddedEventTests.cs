using Moq;
using ProjectManagement.ProjectAPI.Domain.Entities;
using ProjectManagement.ProjectAPI.Domain.Entities.Events;

namespace ProjectManagement.ProjectAPI.UnitTests.Domain.Entities.Events;

[ExcludeFromCodeCoverage]
public class NewItemAddedEventTests
{
    [Fact]
    public void Constructor_SetsProjectAndItemProperties()
    {
        // Arrange
        Mock<Project> mockProject = new ("project", Priority.Medium, 1);
        Mock<TodoItem> mockTodoItem = new ();

        // Act
        NewItemAddedEvent newItemAddedEvent = new (mockProject.Object, mockTodoItem.Object);

        // Assert
        Assert.Equal(mockProject.Object, newItemAddedEvent.Project);
        Assert.Equal(mockTodoItem.Object, newItemAddedEvent.Item);
    }
}