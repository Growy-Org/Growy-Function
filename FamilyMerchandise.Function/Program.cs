using FamilyMerchandise.Function.Options;
using FamilyMerchandise.Function.Repositories;
using FamilyMerchandise.Function.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureEnvironmentAppSettings()
    .ConfigureServices((context, services) =>
    {
        services.Configure<ConnectionStrings>(context.Configuration.GetSection(ConnectionStrings.KEY));
        services.AddSingleton<IConnectionFactory,ConnectionFactory>();
        services.AddSingleton<IHomeService,HomeService>();
        services.AddSingleton<IHomeRepository,HomeRepository>();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();