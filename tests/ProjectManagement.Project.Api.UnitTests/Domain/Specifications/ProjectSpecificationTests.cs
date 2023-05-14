using ProjectManagement.ProjectAPI.Domain.Entities;
using ProjectManagement.ProjectAPI.Domain.Specifications;

namespace ProjectManagement.ProjectAPI.UnitTests.Domain.Specifications;

public class ProjectSpecificationTests
{
    private readonly List<Project> _projects = new ()
    {
        new ("c1p1", Priority.Low, 1) { Id = 1 },
        new ("c1p2", Priority.Low, 1) { Id = 2 },
        new ("c2p1", Priority.Low, 2) { Id = 3 },
        new ("c3p1", Priority.Low, 3) { Id = 4 },
    };


    [Fact]
    public void ReturnCorrectProjects_WhenAllProjectsByCompanyIdWithTagsSpecCalled()
    {
        // Arrange
        int companyId = 1;
        AllProjectsByCompanyIdWithTagsSpec spec = new (companyId);

        // Act
        IEnumerable<Project> result = spec.Evaluate(_projects);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, x => Assert.Equal(companyId, x.CompanyId));
    }

    [Fact]
    public void ReturnCorrectProjects_WhenAllAllProjectsWithTagsSpecCalled()
    {
        // Arrange
        Project project = new ("p1", Priority.Critical, 1);
        project.AddTodoItem(new TodoItem { Title = "t1" });

        List<Project> projects = new ()
            { project };

        AllProjectsWithTagsSpec spec = new ();

        // Act
        List<Project> result = spec.Evaluate(projects).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.All(result, x => Assert.NotEmpty(x.TodoItems));
    }

    [Fact]
    public void ReturnCorrectProjects_WhenProjectByIdSpecCalled()
    {
        // Arrange
        ProjectByIdSpec spec = new (1);

        // Act
        List<Project> result = spec.Evaluate(_projects).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.All(result, x => Assert.Equal(1, x.Id));
    }

    [Fact]
    public void ReturnCorrectProjects_WhenProjectByNameSpecCalled()
    {
        // Arrange
        string projectName = "c1p1";
        ProjectByNameSpec spec = new (projectName);

        // Act
        List<Project> result = spec.Evaluate(_projects).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.All(result, x => Assert.Equal(projectName, x.Name));
    }
}