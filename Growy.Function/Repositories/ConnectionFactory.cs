using System.Data;
using Growy.Function.Options;
using Growy.Function.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Growy.Function.Repositories;

public class ConnectionFactory(IOptions<ConnectionStrings> connectionStrings) : IConnectionFactory
{
    private readonly string _connectionStrings = connectionStrings.Value.GrowyDB;

    public IDbConnection GetDBConnection()
    {
        return new NpgsqlConnection(_connectionStrings);
    }
}