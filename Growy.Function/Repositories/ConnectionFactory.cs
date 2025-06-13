using System.Data;
using Azure.Core;
using Azure.Identity;
using Growy.Function.Options;
using Growy.Function.Repositories.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Growy.Function.Repositories;

public class ConnectionFactory(IOptions<ConnectionStrings> connectionStrings, IHostEnvironment env) : IConnectionFactory
{
    public async Task<IDbConnection> GetDBConnection()
    {
        if (env.IsDevelopment())
        {
            return new NpgsqlConnection(connectionStrings.Value.GrowyDB);
        }

        // Prod
        var credential = new DefaultAzureCredential();
        var tokenRequestContext =
            new TokenRequestContext(new[] { "https://ossrdbms-aad.database.windows.net/.default" });

        var token = await credential.GetTokenAsync(tokenRequestContext);

        var connectionString =
            $"Host={connectionStrings.Value.GrowyHost};Port=5432;Database={connectionStrings.Value.GrowyDbName};Username={connectionStrings.Value.GrowyUser};Password={token.Token};Ssl Mode=Require;Trust Server Certificate=true";

        return new NpgsqlConnection(connectionString);
    }
}