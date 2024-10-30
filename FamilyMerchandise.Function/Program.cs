using FamilyMerchandise.Function.Options;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureEnvironmentAppSettings()
    .ConfigureServices((context, services) =>
    {
        services.Configure<ConnectionStrings>(context.Configuration.GetSection(ConnectionStrings.KEY));
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();