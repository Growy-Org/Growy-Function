using Dapper;
using FamilyMerchandise.Function.Entities;
using Bogus;
using Bogus.DataSets;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories;

namespace FamilyMerchandise.Tests;

public class FamilyMerchandiseDbHelper(FunctionTestFixture fixture) : IClassFixture<FunctionTestFixture>
{
    private readonly Faker _faker = new();

    [Fact]
    public async Task InsertInitialData()
    {
        var homeRepo = new HomeRepository(fixture.ConnectionFactory);
        var childRepo = new ChildRepository(fixture.ConnectionFactory);
        var parentRepo = new ParentRepository(fixture.ConnectionFactory);

        var home = new Home()
        {
            Name = _faker.Address.FullAddress(),
        };
        var homeId = await homeRepo.InsertHome(home);

        var child = new Child()
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Gender = _faker.PickRandom(ChildGender.BOY, ChildGender.GIRL),
            PointsEarned = _faker.Random.Int(0, 9999),
        };
        await childRepo.InsertChild(homeId, child);

        var parent = new Parent
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Role = _faker.PickRandom(ParentRole.FATHER, ParentRole.MOTHER),
        };

        await parentRepo.InsertParent(homeId, parent);
    }
}