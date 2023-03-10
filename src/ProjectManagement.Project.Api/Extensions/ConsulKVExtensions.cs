using ProjectManagement.Project.Api.Configuration;
using Winton.Extensions.Configuration.Consul;

namespace ProjectManagement.Project.Api.Extensions;

public static class ConsulKVExtensions
{
    public static void AddConsulKV(this IConfigurationBuilder builder, ConsulKVSettings settings)
    {
        builder.AddConsul(settings.Key, options =>
        {
            options.ConsulConfigurationOptions = config =>
            {
                config.Address = new Uri(settings.Url);
                config.Token = settings.Token;
            };

            options.Optional = true;
            options.ReloadOnChange = true;
        });
    }
}