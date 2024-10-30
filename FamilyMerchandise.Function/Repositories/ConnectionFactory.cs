using System.Data;
using FamilyMerchandise.Function.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace FamilyMerchandise.Function.Repositories;

public class ConnectionFactory(IOptions<ConnectionStrings> connectionStrings) : IConnectionFactory
{
    private readonly string _connectionStrings = connectionStrings.Value.FamilyMerchandiseDB;

    public IDbConnection GetFamilyMerchandiseDBConnection()
    {
        return new NpgsqlConnection(_connectionStrings);
    }
}