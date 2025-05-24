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
            IdpId = idpUserId.ToString(),
            IdentityProvider = "AzureB2C"
        };

        await _appUserRepo.InsertIfNotExist(appUser);
        var homeRequest = new CreateHomeRequest()
        {
            HomeAddress = _faker.Address.FullAddress(),
            HomeName = $"{_faker.Random.Word()}'s Home",
            AppUserId = idpUserId
        };
        var homeId = await _homeRepo.InsertHome(homeRequest);

        var child = new Child()
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Gender = _faker.PickRandom(ChildGender.BOY, ChildGender.GIRL),
            PointsEarned = _faker.Random.Int(0, 9999),
        };
        var child2 = new Child()
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Gender = _faker.PickRandom(ChildGender.BOY, ChildGender.BOY),
            PointsEarned = _faker.Random.Int(0, 9999),
        };

        var childId = await _childRepo.InsertChild(homeId, child);
        var childId2 = await _childRepo.InsertChild(homeId, child2);
    }

    [Fact]
    public async Task InsertInitialData()
    {
        var idpUserId = Guid.NewGuid();
        var appUser = new AppUser()
        {
            Id = idpUserId,
            Email = _faker.Internet.Email(_faker.Name.FirstName(), _faker.Name.LastName()),
            IdpId = idpUserId.ToString(),
            IdentityProvider = "AzureB2C"
        };

        await _appUserRepo.InsertIfNotExist(appUser);

        var homeRequest = new CreateHomeRequest()
        {
            HomeAddress = _faker.Address.FullAddress(),
            HomeName = $"{_faker.Random.Word()}'s Home",
            AppUserId = idpUserId
        };
        var homeId = await _homeRepo.InsertHome(homeRequest);

        var child = new Child()
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Gender = _faker.PickRandom(ChildGender.BOY, ChildGender.GIRL),
            PointsEarned = _faker.Random.Int(0, 9999),
        };
        var child2 = new Child()
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Gender = _faker.PickRandom(ChildGender.BOY, ChildGender.BOY),
            PointsEarned = _faker.Random.Int(0, 9999),
        };

        var childId = await _childRepo.InsertChild(homeId, child);
        var childId2 = await _childRepo.InsertChild(homeId, child2);

        var parent = new Parent
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Role = _faker.PickRandom(ParentRole.FATHER, ParentRole.MOTHER),
        };
        var parent2 = new Parent
        {
            Name = _faker.Name.FullName(),
            IconCode = _faker.Random.Int(0, 100),
            DOB = _faker.Date.Past(18),
            Role = _faker.PickRandom(ParentRole.FATHER, ParentRole.FATHER),
        };

        var parentId = await _parentRepo.InsertParent(homeId, parent);
        var parentId2 = await _parentRepo.InsertParent(homeId, parent2);

        var assignmentRequest1 = new CreateAssignmentRequest
        {
            HomeId = homeId,
            ParentId = parentId,
            ChildId = childId,
            AssignmentName = "Assignment 1",
            AssignmentIconCode = _faker.Random.Int(0, 100),
            AssignmentDescription = _faker.Lorem.Sentence(50),
            DueDateUtc = _faker.Date.Recent(50),
            Points = _faker.Random.Int(100, 999)
        };

        var assignmentRequest2 = new CreateAssignmentRequest
        {
            HomeId = homeId,
            ParentId = parentId2,
            ChildId = childId2,
            AssignmentName = "Assignment 1",
            AssignmentDescription = _faker.Lorem.Sentence(50),
            AssignmentIconCode = _faker.Random.Int(0, 100),
            DueDateUtc = _faker.Date.Recent(50),
            Points = _faker.Random.Int(100, 999)
        };

        var assignmentId1 = await _assignmentRepo.InsertAssignment(assignmentRequest1);
        var assignmentId2 = await _assignmentRepo.InsertAssignment(assignmentRequest2);

        var stepRequest1 = new CreateStepRequest
        {
            AssignmentId = assignmentId1,
            StepOrder = 0,
            StepDescription = _faker.Lorem.Sentence(20),
        };

        var stepRequest2 = new CreateStepRequest
        {
            AssignmentId = assignmentId1,
            StepOrder = 1,
            StepDescription = _faker.Lorem.Sentence(20),
        };

        var stepRequest3 = new CreateStepRequest
        {
            AssignmentId = assignmentId2,
            StepOrder = 0,
            StepDescription = _faker.Lorem.Sentence(20),
        };
        await _stepRepo.InsertStep(stepRequest1);
        await _stepRepo.InsertStep(stepRequest2);
        await _stepRepo.InsertStep(stepRequest3);

        var wishRequest1 = new CreateWishRequest()
        {
            HomeId = homeId,
            ParentId = parentId,
            ChildId = childId,
            WishName = _faker.Random.Word(),
            WishDescription = _faker.Lorem.Sentence(50),
            WishIconCode = _faker.Random.Int(0, 100),
        };
        var wishRequest2 = new CreateWishRequest()
        {
            HomeId = homeId,
            ParentId = parentId2,
            ChildId = childId2,
            WishName = _faker.Random.Word(),
            WishDescription = _faker.Lorem.Sentence(50),
            WishIconCode = _faker.Random.Int(0, 100),
        };

        await _wishRepo.InsertWish(wishRequest1);
        await _wishRepo.InsertWish(wishRequest2);

        var achievementRequest1 = new CreateAchievementRequest()
        {
            HomeId = homeId,
            ParentId = parentId,
            ChildId = childId,
            AchievementName = "Achievement 1",
            AchievementDescription = _faker.Lorem.Sentence(50),
            AchievementIconCode = _faker.Random.Int(0, 100),
            AchievementPointsGranted = _faker.Random.Int(100, 9999),
        };
        var achievementRequest2 = new CreateAchievementRequest()
        {
            HomeId = homeId,
            ParentId = parentId2,
            ChildId = childId2,
            AchievementName = "Achievement 1",
            AchievementDescription = _faker.Lorem.Sentence(50),
            AchievementIconCode = _faker.Random.Int(0, 100),
            AchievementPointsGranted = _faker.Random.Int(100, 9999),
        };

        await _achievementRepo.InsertAchievement(achievementRequest1);
        await _achievementRepo.InsertAchievement(achievementRequest2);

        var penaltyRequest = new CreatePenaltyRequest()
        {
            HomeId = homeId,
            ParentId = parentId,
            ChildId = childId,
            PenaltyName = "Penalty 1",
            PenaltyReason = _faker.Lorem.Sentence(50),
            PenaltyIconCode = _faker.Random.Int(0, 100),
            PenaltyPointsDeducted = _faker.Random.Int(100, 500),
        };

        await _penaltyRepo.InsertPenalty(penaltyRequest);

        var dqReport = new SubmitDevelopmentReportRequest
        {
            HomeId = homeId,
            ExaminerId = parentId,
            CandidateId = childId,
            Answers = [1, 2, 34, 6],
            TotalScore = 230,
            DqResult = 100.2f,
            CandidateMonth = 5.2f
        };
        
        await _assessmentRepository.CreateReport(dqReport);
    }
}