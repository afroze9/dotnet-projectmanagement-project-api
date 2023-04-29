using Ardalis.Specification;
using ProjectManagement.ProjectAPI.Domain.Entities;

namespace ProjectManagement.ProjectAPI.Domain.Specifications;

public class ProjectByIdSpec : Specification<Project>, ISingleResultSpecification
{
    public ProjectByIdSpec(int id)
    {
        Query.Where(x => x.Id == id).Include(x => x.TodoItems);
    }
}