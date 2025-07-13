using Bogus;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Repositories;

namespace GrowyFunction.Tests;

public class DBTestHelper(FunctionTestFixture fixture) : IClassFixture<FunctionTestFixture>
{
    private readonly Faker _faker = new();

    private readonly AppUserRepository _appUserRepo = new(fixture.ConnectionFactory);
    private readonly HomeRepository _homeRepo = new(fixture.ConnectionFactory);
    private readonly ChildRepository _childRepo = new(fixture.ConnectionFactory);
    private readonly ParentRepository _parentRepo = new(fixture.ConnectionFactory);
    private readonly AssignmentRepository _assignmentRepo = new(fixture.ConnectionFactory);
    private readonly StepRepository _stepRepo = new(fixture.ConnectionFactory);
    private readonly WishRepository _wishRepo = new(fixture.ConnectionFactory);
    private readonly AchievementRepository _achievementRepo = new(fixture.ConnectionFactory);
    private readonly PenaltyRepository _penaltyRepo = new(fixture.ConnectionFactory);
    private readonly AssessmentRepository _assessmentRepository = new(fixture.ConnectionFactory);

    [Fact]
    public async Task CustomInsert()
    {
        var idpUserId = Guid.NewGuid();
        var appUser = new AppUser()
        {
            Id = idpUserId,
            Email = _faker.Internet.Email(_faker.Name.FirstName(), _faker.Name.LastName()),
            DisplayName = _faker.Name.FirstName(),
            IdpId = idpUserId.ToString(),
            IdentityProvider = "AzureB2C"
        };

        await _appUserRepo.InsertIfNotExist(appUser);
        var homeRequest = new HomeRequest()
        {
            Address = _faker.Address.FullAddress(),
            Name = $"{_faker.Random.Word()}'s Home",
        };
        var homeId = await _homeRepo.InsertHome(idpUserId, homeRequest);

        var child = new ChildRequest()
        {
            Name = _faker.Name.FullName(),

            DOB = _faker.Date.Past(18),
            Gender = _faker.PickRandom(ChildGender.BOY, ChildGender.GIRL),
        };
        var child2 = new ChildRequest()
        {
            Name = _faker.Name.FullName(),
            DOB = _faker.Date.Past(18),
            Gender = _faker.PickRandom(ChildGender.BOY, ChildGender.BOY),
        };

        var childId = await _childRepo.InsertChild(homeId, child);
        var childId2 = await _childRepo.InsertChild(homeId, child2);
    }

    [Fact]
    public async Task InsertInitialData()
    {
        var oid = Guid.NewGuid();
        var name = $"{_faker.Name.FirstName()} {_faker.Name.LastName()}";
        var appUser = new AppUser()
        {
            Email = _faker.Internet.Email(_faker.Name.FirstName(), _faker.Name.LastName()),
            DisplayName = name,
            IdpId = oid.ToString(),
            IdentityProvider = "AzureB2C",
            Sku = AppSku.Premium,
        };

        var idpUserId = await _appUserRepo.InsertIfNotExist(appUser);

        var homeRequest = new HomeRequest()
        {
            Address = _faker.Address.FullAddress(),
            Name = $"{_faker.Random.Word()}'s Home",
        };
        var homeId = await _homeRepo.InsertHome(idpUserId, homeRequest);

        var child = new ChildRequest()
        {
            Name = _faker.Name.FullName(),
            DOB = _faker.Date.Past(18),
            Gender = _faker.PickRandom(ChildGender.BOY, ChildGender.GIRL),
        };
        var child2 = new ChildRequest()
        {
            Name = _faker.Name.FullName(),
            DOB = _faker.Date.Past(18),
            Gender = _faker.PickRandom(ChildGender.BOY, ChildGender.BOY),
        };

        var childId = await _childRepo.InsertChild(homeId, child);
        var childId2 = await _childRepo.InsertChild(homeId, child2);

        var parent = new ParentRequest()
        {
            Name = _faker.Name.FullName(),
            DOB = _faker.Date.Past(18),
            Role = _faker.PickRandom(ParentRole.FATHER, ParentRole.MOTHER),
        };

        var parent2 = new ParentRequest
        {
            Name = _faker.Name.FullName(),
            DOB = _faker.Date.Past(18),
            Role = _faker.PickRandom(ParentRole.FATHER, ParentRole.FATHER),
        };

        var parentId = await _parentRepo.InsertParent(homeId, parent);
        var parentId2 = await _parentRepo.InsertParent(homeId, parent2);

        var assignmentRequest1 = new AssignmentRequest
        {
            ParentId = parentId,
            ChildId = childId,
            Name = "Assignment 1",
            Description = _faker.Lorem.Sentence(50),
            DueDateUtc = _faker.Date.Recent(50),
            Points = _faker.Random.Int(100, 999)
        };

        var assignmentRequest2 = new AssignmentRequest
        {
            ParentId = parentId2,
            ChildId = childId2,
            Name = "Assignment 1",
            Description = _faker.Lorem.Sentence(50),
            DueDateUtc = _faker.Date.Recent(50),
            Points = _faker.Random.Int(100, 999)
        };

        var assignmentId1 = await _assignmentRepo.InsertAssignment(homeId, assignmentRequest1);
        var assignmentId2 = await _assignmentRepo.InsertAssignment(homeId, assignmentRequest2);

        var stepRequest1 = new StepRequest
        {
            StepOrder = 0,
            Description = _faker.Lorem.Sentence(20),
        };

        var stepRequest2 = new StepRequest
        {
            StepOrder = 1,
            Description = _faker.Lorem.Sentence(20),
        };

        var stepRequest3 = new StepRequest
        {
            StepOrder = 0,
            Description = _faker.Lorem.Sentence(20),
        };
        await _stepRepo.InsertStep(assignmentId1, stepRequest1);
        await _stepRepo.InsertStep(assignmentId1, stepRequest2);
        await _stepRepo.InsertStep(assignmentId2, stepRequest3);

        var wishRequest1 = new WishRequest()
        {
            ParentId = parentId,
            ChildId = childId,
            Name = _faker.Random.Word(),
            Description = _faker.Lorem.Sentence(50),
            PointsCost = _faker.Random.Int(100, 9999),
        };
        var wishRequest2 = new WishRequest()
        {
            ParentId = parentId2,
            ChildId = childId2,
            Name = _faker.Random.Word(),
            Description = _faker.Lorem.Sentence(50),
            PointsCost = _faker.Random.Int(100, 9999),
        };

        await _wishRepo.InsertWish(homeId, wishRequest1);
        await _wishRepo.InsertWish(homeId, wishRequest2);

        var achievementRequest1 = new AchievementRequest()
        {
            ParentId = parentId,
            ChildId = childId,
            Name = "Achievement 1",
            Description = _faker.Lorem.Sentence(50),
            PointsGranted = _faker.Random.Int(100, 9999),
        };
        var achievementRequest2 = new AchievementRequest()
        {
            ParentId = parentId2,
            ChildId = childId2,
            Name = "Achievement 1",
            Description = _faker.Lorem.Sentence(50),
            PointsGranted = _faker.Random.Int(100, 9999),
        };

        await _achievementRepo.InsertAchievement(homeId, achievementRequest1);
        await _achievementRepo.InsertAchievement(homeId, achievementRequest2);

        var penaltyRequest = new PenaltyRequest()
        {
            ParentId = parentId,
            ChildId = childId,
            Name = "Penalty 1",
            Description = _faker.Lorem.Sentence(50),
            PointsDeducted = _faker.Random.Int(100, 500),
        };

        var con = await fixture.ConnectionFactory.GetDBConnection();
        await _penaltyRepo.InsertPenalty(homeId, penaltyRequest, con, con.BeginTransaction());

        var dqReport = new DevelopmentReportRequest
        {
            Answers = [1, 2, 34, 6],
            TotalMentalAge = 23.2f,
            ChildId = childId,
            ParentId = parentId,
            DqResult = 100.2f,
            CandidateMonth = 5.2f
        };

        await _assessmentRepository.CreateDqReport(homeId, dqReport);
    }
}