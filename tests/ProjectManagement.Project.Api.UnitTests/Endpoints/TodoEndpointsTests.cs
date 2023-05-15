using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using ProjectManagement.ProjectAPI.Abstractions;
using ProjectManagement.ProjectAPI.Domain.Entities;
using ProjectManagement.ProjectAPI.Endpoints;
using ProjectManagement.ProjectAPI.Models;

namespace ProjectManagement.ProjectAPI.UnitTests.Endpoints;

[ExcludeFromCodeCoverage]
public class TodoEndpointsTests
{
    [Fact]
    public void AddTodoEndpoints_AddsEndpoints()
    {
        WebApplication webapp = WebApplication.CreateBuilder().Build();

        webapp.AddTodoEndpoints();

        Assert.NotNull(webapp);
    }

    [Fact]
    public async Task UpdateTodo_ProjectDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int id = 1;
        Mock<IRepository<TodoItem>> repository = new Mock<IRepository<TodoItem>>();
        TodoItemAssignmentUpdateModel req = new TodoItemAssignmentUpdateModel(true, "2");
        TodoItem itemToUpdate = new TodoItem
            { Title = "test" };

        repository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoItem?) null);

        // Act
        IResult result = await TodoEndpoints.UpdateTodo(id, repository.Object, req);

        // Assert
        Assert.IsType<BadRequest>(result);
    }

    [Fact]
    public async Task UpdateTodo_ValidIdAndReq_ReturnsOkResult()
    {
        // Arrange
        int id = 1;
        Mock<IRepository<TodoItem>> repository = new Mock<IRepository<TodoItem>>();
        TodoItemAssignmentUpdateModel req = new TodoItemAssignmentUpdateModel(true, "2");
        TodoItem itemToUpdate = new TodoItem
            { Title = "test" };

        repository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemToUpdate);

        // Act
        IResult result = await TodoEndpoints.UpdateTodo(id, repository.Object, req);

        // Assert
        Assert.IsType<Ok<TodoItem>>(result);
        repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTodoById_ValidId_ReturnsOkResult()
    {
        // Arrange
        int id = 1;
        Mock<IRepository<TodoItem>> repository = new Mock<IRepository<TodoItem>>();
        TodoItem itemToGet = new TodoItem { Title = "test" };
        repository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(itemToGet);

        // Act
        IResult result = await TodoEndpoints.GetTodoById(id, repository.Object);

        // Assert
        Assert.IsType<Ok<TodoItem>>(result);
    }

    [Fact]
    public async Task GetTodosByProjectId_ValidIdAndReq_ReturnsCreatedResult()
    {
        // Arrange
        int id = 1;
        Mock<IRepository<Project>> repository = new Mock<IRepository<Project>>();
        Mock<IMapper> mapper = new Mock<IMapper>();
        TodoItemRequestModel req = new TodoItemRequestModel("Test", "Test", "Test");
        Project dbProject = new Project("project", Priority.Critical, 1) { Id = id };
        TodoItem todoItem = new TodoItem { Id = 2, Title = req.Title };
        repository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(dbProject);
        mapper.Setup(m => m.Map<TodoItem>(req)).Returns(todoItem);

        // Act
        IResult result = await TodoEndpoints.GetTodosByProjectId(id, req, repository.Object, mapper.Object);

        // Assert
        Assert.IsType<Created<TodoItem>>(result);
        repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTodosByProjectId_ProjectDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int id = 1;
        Mock<IRepository<Project>> repository = new Mock<IRepository<Project>>();
        Mock<IMapper> mapper = new Mock<IMapper>();
        TodoItemRequestModel req = new TodoItemRequestModel("Test", "Test", "Test");
        Project dbProject = new Project("project", Priority.Critical, 1) { Id = id };
        TodoItem todoItem = new TodoItem { Id = 2, Title = req.Title };
        repository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Project?) null);
        mapper.Setup(m => m.Map<TodoItem>(req)).Returns(todoItem);

        // Act
        IResult result = await TodoEndpoints.GetTodosByProjectId(id, req, repository.Object, mapper.Object);

        // Assert
        Assert.IsType<NotFound>(result);
    }
}