using Microsoft.AspNetCore.Authorization;

namespace ProjectManagement.ProjectAPI.Authorization;

[ExcludeFromCodeCoverage]
public class ScopeRequirement : IAuthorizationRequirement
{
    public string Scope { get; }

    public ScopeRequirement(string scope)
    {
        Scope = scope;
    }
}