using Dapper;
using FamilyMerchandise.Function.Entities;
using Bogus;
using Bogus.DataSets;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;
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
        var assignmentRepo = new AssignmentRepository(fixture.ConnectionFactory);
        var wishRepo = new WishRepository(fixture.ConnectionFactory);

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
        var childId = await childRepo.InsertChild(homeId, child);

        var parent = new Parent
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Role = _faker.PickRandom(ParentRole.FATHER, ParentRole.MOTHER),
        };

        var parentId = await parentRepo.InsertParent(homeId, parent);

        var assignmentRequest1 = new CreateAssignmentRequest
        {
            HomeId = homeId,
            ParentId = parentId,
            ChildId = childId,
            AssignmentName = "Assignment 1",
            AssignmentIconCode = _faker.Random.Int(0, 100),
            AssignmentDescription = _faker.Lorem.Sentence(50),
            DueDate = _faker.Date.Recent(50),
            Points = _faker.Random.Int(100, 999)
        };
        
        var assignmentRequest2 = new CreateAssignmentRequest
        {
            HomeId = homeId,
            ParentId = parentId,
            ChildId = childId,
            AssignmentName = "Assignment 2",
            AssignmentDescription = _faker.Lorem.Sentence(50),
            AssignmentIconCode = _faker.Random.Int(0, 100),
            DueDate = _faker.Date.Recent(50),
            Points = _faker.Random.Int(100, 999)
        };

        await assignmentRepo.InsertAssignment(assignmentRequest1);
        await assignmentRepo.InsertAssignment(assignmentRequest2);
        
        var wishRequest = new CreateWishRequest()
        {
            HomeId = homeId,
            ParentId = parentId,
            ChildId = childId,
            WishName = _faker.Random.Word(),
            WishDescription = _faker.Lorem.Sentence(50),
            WishIconCode = _faker.Random.Int(0, 100),
        };
        
        await wishRepo.InsertWish(wishRequest);
    }
}