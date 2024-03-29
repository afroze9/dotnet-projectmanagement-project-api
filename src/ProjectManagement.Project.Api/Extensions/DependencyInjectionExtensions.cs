﻿using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ProjectManagement.ProjectAPI.Abstractions;
using ProjectManagement.ProjectAPI.Authorization;
using ProjectManagement.ProjectAPI.Configuration;
using ProjectManagement.ProjectAPI.Data;
using ProjectManagement.ProjectAPI.Mapping;
using ProjectManagement.ProjectAPI.Services;
using Steeltoe.Connector.PostgreSql;
using Steeltoe.Connector.PostgreSql.EFCore;
using Steeltoe.Discovery.Client;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Endpoint.Refresh;

namespace ProjectManagement.ProjectAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
    private static readonly string[] Actions = { "read", "write", "update", "delete" };

    private static void AddActuators(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthActuator(configuration);
        services.AddInfoActuator(configuration);
        services.AddHealthChecks();
        services.AddRefreshActuator();
        services.ActivateActuatorEndpoints();
    }
    
    private static void AddApiDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Project API",
                Description = "Project Microservice",
            });

            string xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));

            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme.",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearer",
                        },
                    },
                    Array.Empty<string>()
                },
            });
        });
    }
    
    private static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
    }

    private static void AddConsulDiscovery(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDiscoveryClient(configuration);
    }

    private static void AddCrudPolicies(this AuthorizationOptions options, string resource)
    {
        foreach (string action in Actions)
        {
            options.AddPolicy($"{action}:{resource}",
                policy => policy.Requirements.Add(new ScopeRequirement($"{action}:{resource}")));
        }
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(configuration); });

        services.AddPostgresHealthContributor(configuration);
    }

    private static void AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        Auth0Settings auth0Settings = new ();
        configuration.GetRequiredSection(nameof(Auth0Settings)).Bind(auth0Settings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = auth0Settings.Authority;
            options.Audience = auth0Settings.Audience;
        });


        services.AddAuthorization(options => { options.AddCrudPolicies("project"); });
        services.AddSingleton<IAuthorizationHandler, ScopeRequirementHandler>();
    }
    
    private static void AddTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        TelemetrySettings telemetrySettings = new ();
        configuration.GetRequiredSection(nameof(TelemetrySettings)).Bind(telemetrySettings);

        services
            .AddOpenTelemetry()
            .WithTracing(builder =>
                {
                    builder
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .ConfigureResource(options =>
                        {
                            options.AddService(
                                telemetrySettings.ServiceName,
                                serviceVersion: telemetrySettings.ServiceVersion,
                                autoGenerateServiceInstanceId: true);
                        })
                        .AddOtlpExporter(options => { options.Endpoint = new Uri(telemetrySettings.Endpoint); });

                    if (telemetrySettings.EnableConsoleExporter)
                    {
                        builder.AddConsoleExporter();
                    }

                    if (telemetrySettings.EnableAlwaysOnSampler)
                    {
                        builder.SetSampler<AlwaysOnSampler>();
                    }
                    else
                    {
                        builder.SetSampler(new TraceIdRatioBasedSampler(telemetrySettings.SampleProbability));
                    }
                }
            )
            .WithMetrics(builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter()
                    .AddOtlpExporter(options => { options.Endpoint = new Uri(telemetrySettings.Endpoint); });
            });
    }
    
    public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddActuators(configuration);
        services.AddApiDocumentation();
        services.AddApplicationServices();
        services.AddAutoMapper(typeof(ProjectProfile));
        services.AddConsulDiscovery(configuration);
        services.AddControllers();
        services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddPersistence(configuration);
        services.AddSecurity(configuration);
        services.AddTelemetry(configuration);
        services.AddValidatorsFromAssemblyContaining(typeof(Program));

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
        });
    }
}
