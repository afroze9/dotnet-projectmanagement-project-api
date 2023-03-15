using Ardalis.Specification;
using ProjectManagement.ProjectAPI.Domain.Entities;

namespace ProjectManagement.ProjectAPI.Domain.Specifications;

public class AllProjectsWithTagsSpec : Specification<Project>
{
    public AllProjectsWithTagsSpec()
    {
        Query.Include(x => x.TodoItems);
    }
}