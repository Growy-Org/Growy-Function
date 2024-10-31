using Dapper;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public class ChildRepository(IConnectionFactory connectionFactory) : IChildRepository
{
    public const string CHILDREN_TABLE = "inventory.children";

    public async Task<Guid> InsertChild(Guid homeId, Child child)
    {
        var childEntity = child.ToChildEntity(homeId);
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {CHILDREN_TABLE} (Name, HomeId, IconCode, DOB, Gender, PointsEarned) VALUES (@Name, @HomeId, @IconCode, @DOB, @Gender, @PointsEarned) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, childEntity);
    }
}