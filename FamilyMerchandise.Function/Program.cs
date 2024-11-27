using FamilyMerchandise.Function.Options;
using FamilyMerchandise.Function.Repositories;
using FamilyMerchandise.Function.Repositories.Interfaces;
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
        
        // Services
        services.AddSingleton<IAppUserService, AppUserService>();
        services.AddSingleton<IHomeService, HomeService>();
        services.AddSingleton<IParentService, ParentService>();
        services.AddSingleton<IChildService, ChildService>();

        // Repositories 
        services.AddSingleton<IConnectionFactory, ConnectionFactory>();
        services.AddSingleton<IAppUserRepository, AppUserRepository>();
        services.AddSingleton<IHomeRepository, HomeRepository>();
        services.AddSingleton<IChildRepository, ChildRepository>();
        services.AddSingleton<IWishRepository, WishRepository>();
        services.AddSingleton<IParentRepository, ParentRepository>();
        services.AddSingleton<IAssignmentRepository, AssignmentRepository>();
        services.AddSingleton<IAchievementRepository, AchievementRepository>();
        services.AddSingleton<IPenaltyRepository, PenaltyRepository>();
        services.AddSingleton<IStepRepository, StepRepository>();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();