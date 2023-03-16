using FluentValidation;
using ProjectManagement.ProjectAPI.Abstractions;
using ProjectManagement.ProjectAPI.Domain.Entities;
using ProjectManagement.ProjectAPI.Domain.Specifications;

namespace ProjectManagement.ProjectAPI.Models.Validation;

[ExcludeFromCodeCoverage]
public class ProjectRequestModelValidator : AbstractValidator<ProjectRequestModel>
{
    public ProjectRequestModelValidator(IRepository<Project> repository)
    {
        RuleFor(p => p.Name)
            .NotNull()
            .MinimumLength(5)
            .MaximumLength(255)
            .MustAsync(async (name, cancellationToken) =>
            {
                bool exists = await repository.AnyAsync(new ProjectByNameSpec(name), cancellationToken);
                return !exists;
            })
            .WithMessage("Project with this name already exists");
    }
}