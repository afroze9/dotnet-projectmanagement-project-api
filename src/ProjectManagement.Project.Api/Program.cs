using Microsoft.EntityFrameworkCore;
using ProjectManagement.ProjectAPI.Data;
using ProjectManagement.ProjectAPI.Endpoints;
using ProjectManagement.ProjectAPI.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddApplicationConfiguration();
builder.Logging.AddApplicationLogging(builder.Configuration);
builder.Services.RegisterDependencies(builder.Configuration);

WebApplication app = builder.Build();

using IServiceScope scope = app.Services.CreateScope();
ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
db.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.AddProjectEndpoints();
app.AddTodoEndpoints();

app.Run();
