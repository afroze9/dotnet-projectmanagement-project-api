using ProjectManagement.Project.Api.Configuration;
using Winton.Extensions.Configuration.Consul;

namespace ProjectManagement.Project.Api.Extensions;

public static class ConsulKVExtensions
{
    public static void AddConsulKV(this IConfigurationBuilder builder, ConsulKVConfiguration configuration)
    {
        builder.AddConsul(configuration.Key, options =>
        {
            options.ConsulConfigurationOptions = config =>
            {
                config.Address = new Uri(configuration.Url);
                config.Token = configuration.Token;
            };

            options.Optional = true;
            options.ReloadOnChange = true;
        });
    }
}