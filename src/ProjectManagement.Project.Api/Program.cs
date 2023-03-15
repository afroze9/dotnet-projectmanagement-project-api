using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.ProjectAPI.Abstractions;
using ProjectManagement.ProjectAPI.Domain.Entities;
using ProjectManagement.ProjectAPI.Domain.Specifications;
using ProjectManagement.ProjectAPI.Extensions;
using ProjectManagement.ProjectAPI.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddApplicationConfiguration();
builder.Logging.AddApplicationLogging(builder.Configuration);
builder.Services.RegisterDependencies(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app
    .MapGet("api/v1/Project",
        async (IRepository<Project> repository) =>
            Results.Ok(await repository.ListAsync(new AllProjectsWithTagsSpec())))
    .Produces<List<Project>>();

app
    .MapGet("api/v1/Project/{id}",
        async (int id, IRepository<Project> repository) => Results.Ok(await repository.GetByIdAsync(id)))
    .Produces<Project>();

app
    .MapPost("api/v1/Project", async (IRepository<Project> repository, IMapper mapper, ProjectRequestModel req) =>
    {
        Project? project = mapper.Map<Project>(req);
        Project result = await repository.AddAsync(project);

        return Results.Created($"api/v1/Project/{result.Id}", result);
    })
    .Produces<ActionResult<Project>>(StatusCodes.Status201Created);

app.MapPut("api/v1/Project/{id}", async (int id, IRepository<Project> repository, ProjectRequestModel req) =>
    {
        Project? projectToUpdate = await repository.GetByIdAsync(id);

        if (projectToUpdate == null)
        {
            return Results.NotFound();
        }

        projectToUpdate.UpdateName(req.Name);
        projectToUpdate.UpdatePriority(req.Priority);

        await repository.SaveChangesAsync();
        return Results.Ok(projectToUpdate);
    })
    .Produces<IActionResult>(StatusCodes.Status404NotFound)
    .Produces<ActionResult<Project>>();

app.MapDelete("api/v1/Project/{id}", async (int id, IRepository<Project> repository) =>
    {
        Project? projectToDelete = await repository.GetByIdAsync(id);

        if (projectToDelete != null)
        {
            await repository.DeleteAsync(projectToDelete);
        }

        return Results.NoContent();
    })
    .Produces<IActionResult>(StatusCodes.Status204NoContent);

app.MapPost("api/v1/Project/{id}/Todo",
    async (int id, TodoItemRequestModel req, IRepository<Project> repository, IMapper mapper) =>
    {
        Project? dbProject = await repository.GetByIdAsync(id);

        if (dbProject == null)
        {
            return Results.NotFound();
        }

        TodoItem? todoItem = mapper.Map<TodoItem>(req);
        dbProject.AddTodoItem(todoItem);

        await repository.SaveChangesAsync();
        return Results.Created($"api/v1/Todo/{todoItem.Id}", todoItem);
    });

app.MapGet("api/v1/Todo/{id}",
    async (int id, IRepository<TodoItem> repository) => Results.Ok(await repository.GetByIdAsync(id)));

app.Run();

