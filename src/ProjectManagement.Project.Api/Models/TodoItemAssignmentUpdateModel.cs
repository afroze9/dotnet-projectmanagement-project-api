namespace ProjectManagement.ProjectAPI.Models;

[ExcludeFromCodeCoverage]
public record TodoItemAssignmentUpdateModel(bool MarkComplete, string AssignedToId);