using ProjectManagement.ProjectAPI.Domain.Entities;

namespace ProjectManagement.ProjectAPI.Models;

public record UpdateProjectRequestModel(int Id, string Name, int? CompanyId, Priority Priority = Priority.Medium);