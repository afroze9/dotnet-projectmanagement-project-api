using MediatR;

namespace ProjectManagement.ProjectAPI.Common;

public abstract class DomainEventBase : INotification
{
    protected DomainEventBase()
    {
        DateOccurred = DateTime.UtcNow;
    }

    public DateTime DateOccurred { get; }
}