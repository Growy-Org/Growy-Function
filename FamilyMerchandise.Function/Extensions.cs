using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureEnvironmentAppSettings(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureAppConfiguration((context, config) =>
        {
            var env = context.HostingEnvironment.EnvironmentName;
            config.ConfigureAppSettings(env);
        });
    }

    public static IConfigurationBuilder ConfigureAppSettings(this IConfigurationBuilder configuration, string env)
    {
        return configuration
            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appSettings.{env}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}