using FluentValidation;
using ProjectManagement.ProjectAPI.Abstractions;
using ProjectManagement.ProjectAPI.Domain.Entities;
using ProjectManagement.ProjectAPI.Domain.Specifications;

namespace ProjectManagement.ProjectAPI.Models.Validation;

public class UpdateProjectRequestModelValidator : AbstractValidator<UpdateProjectRequestModel>
{
    public UpdateProjectRequestModelValidator(IRepository<Project> repository)
    {
        RuleFor(p => p.Name)
            .NotNull()
            .MinimumLength(5)
            .MaximumLength(255)
            .MustAsync(async (model, name, cancellationToken) =>
            {
                Project? existingProject =
                    await repository.FirstOrDefaultAsync(new ProjectByNameSpec(name), cancellationToken);

                return existingProject == null || existingProject.Id == model.Id;
            })
            .WithMessage("Project with this name already exists");
    }
}