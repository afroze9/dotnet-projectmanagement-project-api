using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Moq;
using ProjectManagement.ProjectAPI.Authorization;

namespace ProjectManagement.ProjectAPI.UnitTests.Authorization;

[ExcludeFromCodeCoverage]
public class ScopeRequirementHandlerTests
{
    [Theory]
    [InlineData("test.scope", "test.scope")]
    [InlineData("test.readonly", "test.readonly test.writeonly")]
    public async Task HandleRequirementAsync_ValidClaim_ShouldSucceed(string requiredScope, string userScope)
    {
        // Arrange
        ScopeRequirement requirement = new (requiredScope);
        ClaimsPrincipal user = new (new ClaimsIdentity(new Claim[] { new ("scope", userScope) }));
        Mock<AuthorizationHandlerContext> mockContext = new ();
        mockContext.CallBase = true;
        mockContext.SetupGet(x => x.User).Returns(user);
        AuthorizationHandlerContext context = new (new[] { requirement }, user, null);

        // Act
        ScopeRequirementHandler handler = new ();
        await handler.HandleAsync(context);

        // Assert
        Assert.True(context.HasSucceeded);
    }

    [Theory]
    [InlineData("test.scope", "test.unrelated")]
    [InlineData("test.readonly test.writeonly", "test.readonlyonly")]
    [InlineData("", "test.unrelated")]
    public async Task HandleRequirementAsync_InvalidClaim_ShouldFail(string requiredScope, string userScope)
    {
        // Arrange
        ScopeRequirement requirement = new (requiredScope);
        ClaimsPrincipal user = new (new ClaimsIdentity(new Claim[] { new ("scope", userScope) }));
        Mock<AuthorizationHandlerContext> mockContext = new ();
        mockContext.CallBase = true;
        mockContext.SetupGet(x => x.User).Returns(user);
        AuthorizationHandlerContext context = new (new[] { requirement }, user, null);

        // Act
        ScopeRequirementHandler handler = new ();
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }
}