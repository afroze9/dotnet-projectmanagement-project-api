using Ardalis.Specification.EntityFrameworkCore;
using ProjectManagement.ProjectAPI.Abstractions;

namespace ProjectManagement.ProjectAPI.Data;

[ExcludeFromCodeCoverage]
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public EfRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}