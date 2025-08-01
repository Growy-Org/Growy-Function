using Growy.Function;
using Growy.Function.Options;
using Growy.Function.Repositories;
using Growy.Function.Repositories.Interfaces;
using Growy.Function.Services;
using Growy.Function.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureEnvironmentAppSettings()
    .ConfigureServices((context, services) =>
    {
        // Configuration
        services.Configure<ConnectionStrings>(context.Configuration.GetSection(ConnectionStrings.KEY));

        // Services
        services.AddSingleton<IHomeService, HomeService>();
        services.AddSingleton<IParentService, ParentService>();
        services.AddSingleton<IChildService, ChildService>();
        services.AddSingleton<IAssignmentService, AssignmentService>();
        services.AddSingleton<IAchievementService, AchievementService>();
        services.AddSingleton<IWishService, WishService>();
        services.AddSingleton<IPenaltyService, PenaltyService>();
        services.AddSingleton<IAssessmentService, AssessmentService>();
        services.AddSingleton<IAnalyticService, AnalyticService>();
        services.AddSingleton<IAuthService, AuthService>();

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
        services.AddSingleton<IAnalyticRepository, AnalyticRepository>();
        services.AddSingleton<IAssessmentRepository, AssessmentRepository>();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Open Api - Swagger Config
        services.AddSingleton<DefaultOpenApiConfigurationOptions, CustomOpenApiConfigurationOptions>();
    })
    .ConfigureOpenApi()
    .Build();

host.Run();