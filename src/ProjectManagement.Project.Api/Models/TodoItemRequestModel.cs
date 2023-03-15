namespace ProjectManagement.ProjectAPI.Models;

public record TodoItemRequestModel(string Title, string? Description, string? AssignedToId);