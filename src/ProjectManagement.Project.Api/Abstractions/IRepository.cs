using Ardalis.Specification;

namespace ProjectManagement.ProjectAPI.Abstractions;

public interface IRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot
{
}