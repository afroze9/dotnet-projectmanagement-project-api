using MediatR;
using Moq;
using ProjectManagement.ProjectAPI.Common;
using ProjectManagement.ProjectAPI.Services;

namespace ProjectManagement.ProjectAPI.UnitTests.Services;

[ExcludeFromCodeCoverage]
public class DomainEventDispatcherTests
{
    private readonly Mock<IMediator> _mockMediator = new ();
    private readonly DomainEventDispatcher _sut;

    public DomainEventDispatcherTests()
    {
        _sut = new DomainEventDispatcher(_mockMediator.Object);
    }

    [Fact]
    public async Task DispatchAndClearEvents_ShouldDispatchAllEventsForEachEntity()
    {
        // Arrange
        TestEntityBase entityWithEvents = new ();
        entityWithEvents.AddEvent();

        TestEntityBase[] entities =
        {
            new (),
            new (),
            new (),
            entityWithEvents,
        };

        DomainEventBase[] expectedEvents = entities.SelectMany(x => x.DomainEvents).ToArray();

        // Act
        await _sut.DispatchAndClearEvents(entities);

        // Assert
        foreach (DomainEventBase expectedEvent in expectedEvents)
        {
            _mockMediator.Verify(x => x.Publish(expectedEvent, It.IsAny<CancellationToken>()), Times.Once());
        }
    }

    [Fact]
    public async Task DispatchAndClearEvents_ShouldClearAllDomainEventsForEachEntity()
    {
        // Arrange
        TestEntityBase[] entities =
        {
            new (),
            new (),
            new (),
        };

        // Act
        await _sut.DispatchAndClearEvents(entities);

        // Assert
        foreach (EntityBase entity in entities)
        {
            Assert.Empty(entity.DomainEvents);
        }
    }

    private class TestEntityBase : EntityBase
    {
        public void AddEvent()
        {
            RegisterDomainEvent(new TestDomainEvent());
        }
    }

    private class TestDomainEvent : DomainEventBase
    {
    }
}