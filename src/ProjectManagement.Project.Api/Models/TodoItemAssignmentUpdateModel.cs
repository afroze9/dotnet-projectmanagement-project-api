namespace ProjectManagement.ProjectAPI.Models;

public record TodoItemAssignmentUpdateModel(bool MarkComplete, string AssignedToId);