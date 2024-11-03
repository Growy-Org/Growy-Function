using Dapper;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

public class AchievementRepository(IConnectionFactory connectionFactory) : IAchievementRepository
{
    private const string AchievementsTable = "inventory.achievements";

    public async Task<Guid> InsertAchievement(CreateAchievementRequest request)
    {
        var achievementEntity = request.ToAchievementEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {AchievementsTable} (Name, HomeId, IconCode, PointsGranted, Description, VisionaryId, AchieverId) VALUES (@Name, @HomeId, @IconCode, @PointsGranted, @Description, @VisionaryId, @AchieverId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, achievementEntity);
    }

}