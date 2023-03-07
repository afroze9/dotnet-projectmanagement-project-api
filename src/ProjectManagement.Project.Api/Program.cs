using ProjectManagement.Project.Api.Configuration;
using ProjectManagement.Project.Api.Extensions;
using Steeltoe.Discovery.Client;

namespace ProjectManagement.Project.Api;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
        // Settings for docker
        builder.Configuration.AddJsonFile("hostsettings.json", optional: true);

        // Settings for consul kv
        ConsulKVConfiguration consulKvConfig = new ();
        builder.Configuration.GetRequiredSection("ConsulKV").Bind(consulKvConfig);
        builder.Configuration.AddConsulKV(consulKvConfig);
        
        // Add services to the container.
        builder.Services.AddDiscoveryClient();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}
