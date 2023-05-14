using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using ProjectManagement.ProjectAPI.Abstractions;
using ProjectManagement.ProjectAPI.Domain.Entities;
using ProjectManagement.ProjectAPI.Domain.Specifications;
using ProjectManagement.ProjectAPI.Endpoints;
using ProjectManagement.ProjectAPI.Models;

namespace ProjectManagement.ProjectAPI.UnitTests.Endpoints;

[ExcludeFromCodeCoverage]
public class ProjectEndpointsTests
{
    private readonly int? _companyId;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IValidator<ProjectRequestModel>> _mockProjectRequestModelValidator;
    private readonly Mock<IRepository<Project>> _mockRepo;
    private readonly Mock<IValidator<UpdateProjectRequestModel>> _mockUpdateProjectRequestValidator;
    private readonly List<Project> _projects;

    public ProjectEndpointsTests()
    {
        _mapper = new Mock<IMapper>();
        _mockRepo = new Mock<IRepository<Project>>();
        _mockProjectRequestModelValidator = new Mock<IValidator<ProjectRequestModel>>();
        _mockUpdateProjectRequestValidator = new Mock<IValidator<UpdateProjectRequestModel>>();
        _companyId = 1;
        _projects = new List<Project>
        {
            new ("Project 1", Priority.High, 1) { Id = 1 },
            new ("Project 2", Priority.High, 1) { Id = 2 },
        };
    }

    [Fact]
    public void AddProjectEndpoints_AddsEndpoints()
    {
        WebApplication webapp = WebApplication.CreateBuilder().Build();

        webapp.AddProjectEndpoints();

        Assert.NotNull(webapp);
    }

    [Fact]
    public async Task GetAllProjects_ReturnsOKWithListOfProjects_WhenProjectsExistForCompany()
    {
        // Arrange
        _mockRepo.Setup(x =>
                x.ListAsync(It.IsNotNull<AllProjectsByCompanyIdWithTagsSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_projects);

        // Act
        IResult result = await ProjectEndpoints.GetAllProjects(_mockRepo.Object, _companyId);

        // Assert
        Ok<List<Project>> okResult = Assert.IsType<Ok<List<Project>>>(result);
        List<Project> projects = Assert.IsType<List<Project>>(okResult.Value);
        projects.Should().HaveCount(_projects.Count);
    }

    [Fact]
    public async Task GetAllProjects_ReturnsNotFound_WhenNoProjectsExistForCompany()
    {
        // Arrange
        _mockRepo.Setup(x =>
                x.ListAsync(It.IsNotNull<AllProjectsByCompanyIdWithTagsSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Project>());

        // Act
        IResult result = await ProjectEndpoints.GetAllProjects(_mockRepo.Object, _companyId);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetProjectById_ReturnsOKWithProject_WhenProjectExists()
    {
        // Arrange
        int projectId = 1;

        _mockRepo.Setup(x => x.FirstOrDefaultAsync(It.IsNotNull<ProjectByIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_projects.First(x => x.Id == projectId));

        // Act
        IResult result = await ProjectEndpoints.GetProjectById(projectId, _mockRepo.Object);

        // Assert
        Ok<Project?> okResult = Assert.IsType<Ok<Project?>>(result);
        Project? project = Assert.IsType<Project?>(okResult.Value);
        Assert.NotNull(project);
        project.Id.Should().Be(projectId);
    }

    [Fact]
    public async Task DeleteProject_ReturnsNoContent()
    {
        // Arrange
        int projectId = 1;

        _mockRepo.Setup(x => x.GetByIdAsync(It.IsNotNull<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_projects.First(x => x.Id == projectId));

        _mockRepo.Setup(x => x.DeleteAsync(It.IsNotNull<Project>(), It.IsAny<CancellationToken>()))
            .Returns<Project, CancellationToken>((_, _) => Task.CompletedTask);

        // Act
        IResult result = await ProjectEndpoints.DeleteProject(projectId, _mockRepo.Object);

        // Assert
        Assert.IsType<NoContent>(result);
    }

    [Fact]
    public async Task UpdateProject_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        int projectId = 1;

        _mockUpdateProjectRequestValidator.Setup(x =>
                x.ValidateAsync(It.IsNotNull<UpdateProjectRequestModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult
            {
                Errors = new List<ValidationFailure>
                {
                    new ("Name", "Invalid"),
                },
            });

        // Act
        IResult result = await ProjectEndpoints.UpdateProject(
            projectId,
            _mockRepo.Object,
            _mockUpdateProjectRequestValidator.Object,
            new UpdateProjectRequestModel(1, "test", 1, Priority.Critical)
        );

        // Assert
        Assert.IsType<BadRequest<List<ValidationFailure>>>(result);
    }

    [Fact]
    public async Task UpdateProject_ReturnsNotFound_WhenProjectNotFound()
    {
        // Arrange
        int projectId = 1;
        _mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?) null);

        _mockUpdateProjectRequestValidator.Setup(x =>
                x.ValidateAsync(It.IsNotNull<UpdateProjectRequestModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        IResult result = await ProjectEndpoints.UpdateProject(
            projectId,
            _mockRepo.Object,
            _mockUpdateProjectRequestValidator.Object,
            new UpdateProjectRequestModel(1, "test", 1, Priority.Critical)
        );

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task UpdateProject_ReturnsOk_WhenProjectUpdated()
    {
        // Arrange
        int projectId = 1;
        Project project = new ("p1", Priority.Low, 1);

        _mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        _mockUpdateProjectRequestValidator.Setup(x =>
                x.ValidateAsync(It.IsNotNull<UpdateProjectRequestModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        string updatedName = "p2";
        Priority updatedPriority = Priority.Critical;

        IResult result = await ProjectEndpoints.UpdateProject(
            projectId,
            _mockRepo.Object,
            _mockUpdateProjectRequestValidator.Object,
            new UpdateProjectRequestModel(1, updatedName, 1, updatedPriority)
        );

        // Assert
        Ok<Project> updatedProject = Assert.IsType<Ok<Project>>(result);
        Assert.NotNull(updatedProject.Value);
        Assert.Equal(updatedName, updatedProject.Value.Name);
        Assert.Equal(updatedPriority, updatedProject.Value.Priority);
    }

    [Fact]
    public async Task CreateProject_ReturnsBadRequest_WhenProjectInvalid()
    {
        // Arrange
        Project project = new ("p1", Priority.Critical, 1);

        _mockRepo.Setup(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        _mockProjectRequestModelValidator.Setup(x =>
                x.ValidateAsync(It.IsNotNull<ProjectRequestModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult
            {
                Errors = new List<ValidationFailure>
                {
                    new ("Name", "Invalid"),
                },
            });

        _mapper.Setup(x => x.Map<Project>(It.IsAny<ProjectRequestModel>()))
            .Returns(project);

        // Act
        IResult result = await ProjectEndpoints.CreateProject(
            _mockRepo.Object,
            _mapper.Object,
            _mockProjectRequestModelValidator.Object,
            new ProjectRequestModel("p1", 1, Priority.Critical)
        );

        // Assert
        Assert.IsType<BadRequest<List<ValidationFailure>>>(result);
    }

    [Fact]
    public async Task CreateProject_ReturnsCreated_WhenProjectValid()
    {
        // Arrange
        string projectName = "p1";
        Project project = new (projectName, Priority.Critical, 1);

        _mockRepo.Setup(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        _mockProjectRequestModelValidator.Setup(x =>
                x.ValidateAsync(It.IsNotNull<ProjectRequestModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapper.Setup(x => x.Map<Project>(It.IsAny<ProjectRequestModel>()))
            .Returns(project);

        // Act
        IResult result = await ProjectEndpoints.CreateProject(
            _mockRepo.Object,
            _mapper.Object,
            _mockProjectRequestModelValidator.Object,
            new ProjectRequestModel(projectName, 1, Priority.Critical)
        );

        // Assert
        Created<Project> createdResult = Assert.IsType<Created<Project>>(result);
        Project createdProject = Assert.IsType<Project>(createdResult.Value);
        Assert.Equal(projectName, createdProject.Name);
    }
}