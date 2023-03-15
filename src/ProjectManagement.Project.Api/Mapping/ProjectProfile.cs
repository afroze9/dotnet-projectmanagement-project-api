using AutoMapper;
using ProjectManagement.ProjectAPI.Domain.Entities;
using ProjectManagement.ProjectAPI.Models;

namespace ProjectManagement.ProjectAPI.Mapping;

[ExcludeFromCodeCoverage]
public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectRequestModel>();
        CreateMap<ProjectRequestModel, Project>();
        CreateMap<TodoItemRequestModel, TodoItem>();
    }
}