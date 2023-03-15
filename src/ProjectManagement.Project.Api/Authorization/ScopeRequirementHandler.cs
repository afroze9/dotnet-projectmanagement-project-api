using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ProjectManagement.ProjectAPI.Authorization;

public class ScopeRequirementHandler : AuthorizationHandler<ScopeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
    {
        Claim? scopeClaim =
            context.User.Claims.FirstOrDefault(c => string.Equals(c.Type, "scope", StringComparison.OrdinalIgnoreCase));

        if (scopeClaim is not null && scopeClaim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Any(c => string.Equals(c, requirement.Scope, StringComparison.OrdinalIgnoreCase)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}