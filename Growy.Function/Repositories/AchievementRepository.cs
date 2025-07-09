using Dapper;
using Growy.Function.Models.Dtos;
using Growy.Function.Entities;
using Growy.Function.Entities.EntityResponse;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;

namespace Growy.Function.Repositories;

public class AchievementRepository(IConnectionFactory connectionFactory) : IAchievementRepository
{
    private const string AchievementsTable = "inventory.achievements";
    private const string ChildrenTable = "inventory.children";
    private const string ParentTable = "inventory.parents";

    public async Task<Guid> GetHomeIdByAchievementId(Guid achievementId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT HomeId FROM {AchievementsTable} WHERE Id = @Id
             """;
        return await con.QuerySingleAsync<Guid>(query, new { Id = achievementId });
    }

    public async Task<int> GetAchievementsCount(Guid homeId, Guid? parentId, Guid? childId,
        bool showOnlyNotAchieved = false)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT COUNT(*) FROM {AchievementsTable} a WHERE a.HomeId = @HomeId {GetConditionQuery(parentId, childId, showOnlyNotAchieved)};
             """;
        return await con.QuerySingleAsync<int>(query, new { HomeId = homeId, ChildId = childId, ParentId = parentId });
    }

    public async Task<List<Achievement>> GetAllAchievements(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId,
        bool showOnlyNotAchieved = false)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {AchievementsTable} a
                 LEFT JOIN {ChildrenTable} c ON a.AchieverId = c.Id
                 LEFT JOIN {ParentTable} p ON a.VisionaryId = p.Id
                 WHERE a.HomeId = @HomeId
                 {GetConditionQuery(parentId, childId, showOnlyNotAchieved)}
                 ORDER BY a.CreatedDateUtc ASC
                 LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;

        var achievements =
            await con.QueryAsync(query, _mapEntitiesToAchievementModel,
                new { HomeId = homeId, ChildId = childId, ParentId = parentId });
        return achievements.ToList();
    }

    public async Task<Guid> InsertAchievement(Guid homeId, AchievementRequest request)
    {
        var achievementEntity = request.ToAchievementEntity();
        achievementEntity.HomeId = homeId;
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {AchievementsTable} (Name, HomeId, PointsGranted, Description, VisionaryId, AchieverId) VALUES (@Name, @HomeId, @PointsGranted, @Description, @VisionaryId, @AchieverId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, achievementEntity);
    }

    public async Task<Guid> EditAchievementByAchievementId(Guid achievementId, AchievementRequest request)
    {
        var achievementEntity = request.ToAchievementEntity();
        achievementEntity.Id = achievementId;
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                UPDATE {AchievementsTable} 
                SET Name = @Name,
                 Description = @Description,
                 PointsGranted = @PointsGranted,
                 VisionaryId = @VisionaryId,
                 AchieverId = @AchieverId
                WHERE Id = @Id
                RETURNING Id;
             """;
        return await con.ExecuteScalarAsync<Guid>(query, achievementEntity);
    }

    public async Task<EditAchievementEntityResponse> EditAchievementGrantByAchievementId(Guid achievementId,
        bool isAchievementGranted)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"UPDATE {AchievementsTable} SET AchievedDateUtc = @AchievedDateUtc WHERE Id = @Id RETURNING Id, AchieverId AS ChildId, PointsGranted AS Points;";
        return await con.QuerySingleAsync<EditAchievementEntityResponse>(query,
            new { Id = achievementId, AchievedDateUtc = isAchievementGranted ? DateTime.UtcNow : (DateTime?)null });
    }

    public async Task DeleteAchievementByAchievementId(Guid achievementId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query = $"DELETE FROM {AchievementsTable} where id = @Id;";
        await con.ExecuteScalarAsync<Guid>(query, new { Id = achievementId });
    }

    private string GetConditionQuery(Guid? parentId, Guid? childId, bool showOnlyNotAchieved)
    {
        var extraQuery = "";
        if (parentId != null) extraQuery += "AND a.VisionaryId = @ParentId ";
        if (childId != null) extraQuery += "AND a.AchieverId = @ChildId ";
        if (showOnlyNotAchieved) extraQuery += "AND a.AchievedDateUtc IS NULL";
        return extraQuery;
    }

    private readonly Func<AchievementEntity, ChildEntity, ParentEntity, Achievement> _mapEntitiesToAchievementModel =
        (a, c, p) =>
        {
            var achievement = a.ToAchievement();
            achievement.Achiever = c.ToChild();
            achievement.Visionary = p.ToParent();
            return achievement;
        };
}