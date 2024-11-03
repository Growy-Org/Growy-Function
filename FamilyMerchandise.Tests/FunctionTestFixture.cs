using FamilyMerchandise.Function.Options;
using FamilyMerchandise.Function.Repositories;
using FamilyMerchandise.Function.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace FamilyMerchandise.Tests;

public class FunctionTestFixture : IDisposable
{
    public IConfiguration Configuration { get; private set; }
    public IConnectionFactory ConnectionFactory { get; private set; }

    public FunctionTestFixture()
    {
        // Build configuration just like in the main app
        Configuration = new ConfigurationBuilder()
            .ConfigureAppSettings("Development")
            .Build();

        var connectionStrings = new ConnectionStrings();
        Configuration
            .GetSection(ConnectionStrings.KEY)
            .Bind(connectionStrings);
        var options = Options.Create(connectionStrings);
        ConnectionFactory = new ConnectionFactory(options);
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}