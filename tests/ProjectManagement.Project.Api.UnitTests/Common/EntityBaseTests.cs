using Moq;
using ProjectManagement.ProjectAPI.Common;

namespace ProjectManagement.ProjectAPI.UnitTests.Common;

[ExcludeFromCodeCoverage]
public class EntityBaseTests
{
    [Fact]
    public void RegisterDomainEvent_AddsDomainEventToList()
    {
        // Arrange
        TestEntity entity = new ();
        Mock<DomainEventBase> mockDomainEvent = new ();

        // Act
        entity.AddCustomEvent(mockDomainEvent.Object);

        // Assert
        Assert.Contains(mockDomainEvent.Object, entity.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_RemovesAllDomainEventsFromList()
    {
        // Arrange
        TestEntity entity = new ();

        entity.AddCustomEvent(new Mock<DomainEventBase>().Object);
        entity.AddCustomEvent(new Mock<DomainEventBase>().Object);
        entity.AddCustomEvent(new Mock<DomainEventBase>().Object);

        // Act
        entity.ClearDomainEvents();

        // Assert
        Assert.Empty(entity.DomainEvents);
    }

    private class TestEntity : EntityBase
    {
        public void AddCustomEvent(DomainEventBase domainEvent)
        {
            RegisterDomainEvent(domainEvent);
        }
    }
}