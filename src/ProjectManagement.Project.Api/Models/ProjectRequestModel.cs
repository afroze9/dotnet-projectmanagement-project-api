using ProjectManagement.ProjectAPI.Domain.Entities;

namespace ProjectManagement.ProjectAPI.Models;

public record ProjectRequestModel(string Name, Priority Priority);