using Microsoft.AspNetCore.Authorization;

namespace ProjectManagement.ProjectAPI.Authorization;

[ExcludeFromCodeCoverage]
public class ScopeRequirement : IAuthorizationRequirement
{
    public ScopeRequirement(string scope)
    {
        Scope = scope;
    }

    public string Scope { get; }
}