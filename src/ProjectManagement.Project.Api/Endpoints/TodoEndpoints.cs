using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.ProjectAPI.Abstractions;
using ProjectManagement.ProjectAPI.Domain.Entities;
using ProjectManagement.ProjectAPI.Models;

namespace ProjectManagement.ProjectAPI.Endpoints;

public static class TodoEndpoints
{
    public static void AddTodoEndpoints(this WebApplication app)
    {
        app.MapPost("api/v1/Project/{id}/Todo", GetTodosByProjectId)
            .RequireAuthorization("write:project")
            .WithTags("Todo");

        app.MapGet("api/v1/Todo/{id}", GetTodoById)
            .Produces<ActionResult<TodoItem>>()
            .RequireAuthorization("read:project")
            .WithTags("Todo");

        app.MapPut("api/v1/Todo/{id}", UpdateTodo)
            .Produces<ActionResult<TodoItem>>()
            .Produces<IActionResult>(StatusCodes.Status400BadRequest)
            .RequireAuthorization("update:project")
            .WithTags("Todo");
    }

    internal static async Task<IResult> UpdateTodo(int id, IRepository<TodoItem> repository,
        TodoItemAssignmentUpdateModel req)
    {
        TodoItem? itemToUpdate = await repository.GetByIdAsync(id);

        if (itemToUpdate == null)
        {
            return Results.BadRequest();
        }

        itemToUpdate.AssignTodoItem(req.AssignedToId);

        if (req.MarkComplete)
        {
            itemToUpdate.MarkComplete();
        }

        await repository.SaveChangesAsync();

        return Results.Ok(itemToUpdate);
    }

    internal static async Task<IResult> GetTodoById(int id, IRepository<TodoItem> repository)
    {
        return Results.Ok(await repository.GetByIdAsync(id));
    }

    internal static async Task<IResult> GetTodosByProjectId(int id, TodoItemRequestModel req,
        IRepository<Project> repository, IMapper mapper)
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
    }
}
