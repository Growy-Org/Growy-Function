using Dapper;
using FamilyMerchandise.Function.Entities;
using Bogus;

namespace FamilyMerchandise.Tests;

public class FamilyMerchandiseDBHelper : IClassFixture<FunctionTestFixture>
{
    private readonly FunctionTestFixture _fixture;
    private readonly Faker _faker;
    private const string CHILDREN_TABLE = "inventory.children";
    private const string PARENTS_TABLE = "inventory.parents";

    public FamilyMerchandiseDBHelper(FunctionTestFixture fixture)
    {
        _faker = new Faker();
        _fixture = fixture;
    }

    [Fact]
    public async Task InsertChild()
    {
        using var con = _fixture.ConnectionFactory.GetFamilyMerchandiseDBConnection();
        var child = new ChildEntity
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Gender = _faker.Random.Int(0, 1),
            PointsEarned = _faker.Random.Int(0, 9999),
        };
        var query =
            $"INSERT INTO {CHILDREN_TABLE} (Name, IconCode, DOB, Gender, PointsEarned) VALUES (@Name, @IconCode, @DOB, @Gender, @PointsEarned) RETURNING Id";
        await con.ExecuteScalarAsync<Guid>(query, child);
    }

    [Fact]
    public async Task InsertParent()
    {
        using var con = _fixture.ConnectionFactory.GetFamilyMerchandiseDBConnection();
        var parent = new ParentEntity
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Role = _faker.Random.Int(0, 1),
        };
        var parent2 = new ParentEntity
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Role = _faker.Random.Int(0, 1),
        };
        var query =
            $"INSERT INTO {PARENTS_TABLE} (Name, IconCode, DOB, Role) VALUES (@Name, @IconCode, @DOB, @Role) RETURNING Id";
        await con.ExecuteScalarAsync<Guid>(query, parent);
        await con.ExecuteScalarAsync<Guid>(query, parent2);
    }
}