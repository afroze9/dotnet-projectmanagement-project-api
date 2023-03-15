using Ardalis.Specification;

namespace ProjectManagement.ProjectAPI.Abstractions;

public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class, IAggregateRoot
{
}