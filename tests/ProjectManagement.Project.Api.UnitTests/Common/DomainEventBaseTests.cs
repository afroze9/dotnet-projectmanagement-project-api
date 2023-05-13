using ProjectManagement.ProjectAPI.Common;

namespace ProjectManagement.Project.Api.UnitTests.Common;

[ExcludeFromCodeCoverage]
public class DomainEventBaseTests
{
    [Fact]
    public void DateOccurred_NotNull()
    {
        TestDomainEvent domainEvent = new ();
        DateTime refDate = new (2000, 01, 01);
        Assert.True(domainEvent.DateOccurred > refDate);
    }

    private class TestDomainEvent : DomainEventBase
    {
    }
}