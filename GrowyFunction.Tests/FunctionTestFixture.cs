using Growy.Function.Options;
using Growy.Function.Repositories;
using Growy.Function.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;

namespace GrowyFunction.Tests;

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
        ConnectionFactory = new ConnectionFactory(options,
            new HostingEnvironment() { EnvironmentName = "Development" });
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}