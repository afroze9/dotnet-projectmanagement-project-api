using ProjectManagement.ProjectAPI.Domain.Entities;

namespace ProjectManagement.ProjectAPI.Models;

[ExcludeFromCodeCoverage]
public record ProjectRequestModel(string Name, int? CompanyId, Priority Priority = Priority.Medium);