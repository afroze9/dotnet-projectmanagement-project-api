using Ardalis.Specification;
using ProjectManagement.ProjectAPI.Domain.Entities;

namespace ProjectManagement.ProjectAPI.Domain.Specifications;

public class ProjectByNameSpec : Specification<Project>, ISingleResultSpecification
{
    public ProjectByNameSpec(string name)
    {
        Query.Where(x => x.Name == name);
    }
}