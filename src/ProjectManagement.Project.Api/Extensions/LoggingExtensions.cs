using ProjectManagement.ProjectAPI.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace ProjectManagement.ProjectAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class LoggingExtensions
{
    public static void AddApplicationLogging(this ILoggingBuilder builder, IConfiguration configuration)
    {
        SerilogSettings serilogSettings = new ();
        configuration.GetRequiredSection(nameof(SerilogSettings)).Bind(serilogSettings);

        Logger logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(serilogSettings.ElasticSearchSettings.Uri))
            {
                ModifyConnectionSettings = c =>
                    c
                        .BasicAuthentication(serilogSettings.ElasticSearchSettings.Username,
                            serilogSettings.ElasticSearchSettings.Password)
                        .ServerCertificateValidationCallback((a, b, c, d) => true),
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                AutoRegisterTemplate = true,
                BatchAction = ElasticOpType.Create,
                BatchPostingLimit = 20,
                IndexFormat = serilogSettings.ElasticSearchSettings.IndexFormat,
                CustomFormatter = new ElasticsearchJsonFormatter(),
            })
            .CreateLogger();

        builder.ClearProviders().AddSerilog(logger);
    }
}