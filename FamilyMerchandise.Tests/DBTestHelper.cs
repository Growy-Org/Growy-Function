using Dapper;
using FamilyMerchandise.Function.Entities;
using Bogus;

namespace FamilyMerchandise.Tests;

public class FamilyMerchandiseDBHelper : IClassFixture<FunctionTestFixture>
{
    private readonly FunctionTestFixture _fixture;
    private readonly Faker _faker;
    private const string HOME_TABLE = "inventory.homes";
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
    public async Task InsertHome()
    {
        using var con = _fixture.ConnectionFactory.GetFamilyMerchandiseDBConnection();
        var home = new HomeEntity()
        {
            Name = _faker.Address.FullAddress(),
        };

        var query =
            $"INSERT INTO {HOME_TABLE} (Name) VALUES (@Name) RETURNING Id";
        await con.ExecuteScalarAsync<Guid>(query, home);
    }

    [Fact]
    public async Task InsertParent()
    {
        using var con = _fixture.ConnectionFactory.GetFamilyMerchandiseDBConnection();
        var parent = new ParentEntity
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            HomeId = Guid.Parse("6271aec4-610a-4342-804f-c180904e735a"),
            DOB = _faker.Date.Past(18),
            Role = _faker.Random.Int(0, 1),
        };
        var query =
            $"INSERT INTO {PARENTS_TABLE} (Name, HomeId, IconCode, DOB, Role) VALUES (@Name, @HomeId, @IconCode, @DOB, @Role) RETURNING Id";
        await con.ExecuteScalarAsync<Guid>(query, parent);
    }
}