using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.ProjectAPI.Abstractions;
using ProjectManagement.ProjectAPI.Domain.Entities;
using ProjectManagement.ProjectAPI.Domain.Specifications;
using ProjectManagement.ProjectAPI.Models;

namespace ProjectManagement.ProjectAPI.Endpoints;

public static class ProjectEndpoints
{
    public static void AddProjectEndpoints(this WebApplication app)
    {
        app.MapGet("api/v1/Project", GetAllProjects)
            .Produces<List<Project>>()
            .RequireAuthorization("read:project")
            .WithTags("Project");

        app.MapGet("api/v1/Project/{id}", GetProjectById)
            .Produces<Project>()
            .RequireAuthorization("read:project")
            .WithTags("Project");

        app.MapPost("api/v1/Project", CreateProject)
            .Produces<ActionResult<Project>>(StatusCodes.Status201Created)
            .Produces<ActionResult<List<ValidationFailure>>>(StatusCodes.Status400BadRequest)
            .RequireAuthorization("write:project")
            .WithTags("Project");

        app.MapPut("api/v1/Project/{id}", UpdateProject)
            .Produces<IActionResult>(StatusCodes.Status404NotFound)
            .Produces<ActionResult<Project>>()
            .Produces<ActionResult<List<ValidationFailure>>>(StatusCodes.Status400BadRequest)
            .RequireAuthorization("update:project")
            .WithTags("Project");

        app.MapDelete("api/v1/Project/{id}", DeleteProject)
            .Produces<IActionResult>(StatusCodes.Status204NoContent)
            .RequireAuthorization("delete:project")
            .WithTags("Project");
    }

    internal static async Task<IResult> GetAllProjects(IRepository<Project> repository, int? companyId)
    {
        List<Project> projects =
            await repository.ListAsync(new AllProjectsByCompanyIdWithTagsSpec(companyId));

        return projects.Count == 0 ? Results.NotFound() : Results.Ok(projects);
    }

    internal static async Task<IResult> GetProjectById(int id, IRepository<Project> repository)
    {
        return Results.Ok(await repository.FirstOrDefaultAsync(new ProjectByIdSpec(id)));
    }

    internal static async Task<IResult> DeleteProject(int id, IRepository<Project> repository)
    {
        Project? projectToDelete = await repository.GetByIdAsync(id);

        if (projectToDelete != null)
        {
            await repository.DeleteAsync(projectToDelete);
        }

        return Results.NoContent();
    }

    internal static async Task<IResult> UpdateProject(
        int id,
        IRepository<Project> repository,
        IValidator<UpdateProjectRequestModel> validator,
        UpdateProjectRequestModel req)
    {
        ValidationResult validationResult = await validator.ValidateAsync(req);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        Project? projectToUpdate = await repository.GetByIdAsync(id);

        if (projectToUpdate == null)
        {
            return Results.NotFound();
        }

        projectToUpdate.UpdateName(req.Name);
        projectToUpdate.UpdatePriority(req.Priority);

        await repository.SaveChangesAsync();
        return Results.Ok(projectToUpdate);
    }

    internal static async Task<IResult> CreateProject(
        IRepository<Project> repository,
        IMapper mapper,
        IValidator<ProjectRequestModel> validator,
        ProjectRequestModel req)
    {
        ValidationResult validationResult = await validator.ValidateAsync(req);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        Project? project = mapper.Map<Project>(req);
        Project result = await repository.AddAsync(project);

        return Results.Created($"api/v1/Project/{result.Id}", result);
    }
}