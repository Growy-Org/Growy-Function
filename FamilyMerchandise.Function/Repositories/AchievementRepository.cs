using Dapper;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

public class AchievementRepository(IConnectionFactory connectionFactory) : IAchievementRepository
{
    private const string AchievementsTable = "inventory.achievements";
    public const string ChildrenTable = "inventory.children";
    public const string ParentTable = "inventory.parents";

    public async Task<List<Achievement>> GetAllAchievementsByHomeId(Guid homeId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            @$"
                SELECT *
                FROM {AchievementsTable} a
                LEFT JOIN {ChildrenTable} c ON a.AchieverId = c.Id
                LEFT JOIN {ParentTable} p ON a.VisionaryId = p.Id
                WHERE a.HomeId = @HomeId
            ";

        var achievements =
            await con.QueryAsync(query, _mapEntitiesToAchievementModel,
                new { HomeId = homeId });
        return achievements.ToList();
    }

    public async Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            @$"
                SELECT *
                FROM {AchievementsTable} a
                LEFT JOIN {ChildrenTable} c ON a.AchieverId = c.Id
                LEFT JOIN {ParentTable} p ON a.VisionaryId = p.Id
                WHERE a.AchieverId = @AchieverId
            ";

        var achievements =
            await con.QueryAsync(query, _mapEntitiesToAchievementModel,
                new { AchieverId = childId });
        return achievements.ToList();
    }

    private readonly Func<AchievementEntity, ChildEntity, ParentEntity, Achievement> _mapEntitiesToAchievementModel =
        (a, c, p) =>
        {
            var achievement = a.ToAchievement();
            achievement.Achiever = c.ToChild();
            achievement.Visionary = p.ToParent();
            return achievement;
        };


    public async Task<Guid> InsertAchievement(CreateAchievementRequest request)
    {
        var achievementEntity = request.ToAchievementEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {AchievementsTable} (Name, HomeId, IconCode, PointsGranted, Description, VisionaryId, AchieverId) VALUES (@Name, @HomeId, @IconCode, @PointsGranted, @Description, @VisionaryId, @AchieverId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, achievementEntity);
    }
}