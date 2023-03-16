using Ardalis.Specification;
using ProjectManagement.ProjectAPI.Domain.Entities;

namespace ProjectManagement.ProjectAPI.Domain.Specifications;

public class AllProjectsByCompanyIdWithTagsSpec : Specification<Project>
{
    public AllProjectsByCompanyIdWithTagsSpec(int? companyId)
    {
        Query.Include(x => x.TodoItems);

        if (companyId.HasValue)
        {
            Query.Where(x => x.CompanyId == companyId);
        }
    }
}