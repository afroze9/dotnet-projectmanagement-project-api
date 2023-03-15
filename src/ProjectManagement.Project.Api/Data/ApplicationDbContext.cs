using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.ProjectAPI.Abstractions;
using ProjectManagement.ProjectAPI.Common;
using ProjectManagement.ProjectAPI.Domain.Entities;

namespace ProjectManagement.ProjectAPI.Data;

[ExcludeFromCodeCoverage]
public class ApplicationDbContext : DbContext
{
    private readonly IDomainEventDispatcher? _dispatcher;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventDispatcher dispatcher)
        : base(options)
    {
        _dispatcher = dispatcher;
    }

    public DbSet<Project> Projects => Set<Project>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new ())
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        if (_dispatcher == null)
        {
            return result;
        }

        EntityBase[] entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);
        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}