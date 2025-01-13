using System.Data;
using Dapper;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

// TODO: make a dedicate table for this repository and have it updated asynchronous 
public class AnalyticRepository(IConnectionFactory connectionFactory) : IAnalyticRepository
{
    private const string AchievementsTable = "inventory.achievements";
    private const string WishesTable = "inventory.wishes";
    private const string AssignmentsTable = "inventory.assignments";
    private const string PenaltyTable = "inventory.penalties";

    public async Task<ParentAnalyticProfile> GetAllParentsToAllChildAnalytic(Guid homeId, int year)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var totalNumberOfAssignments =
            await GetTotalNumberById(con, AssignmentsTable, homeId, nameof(AssignmentEntity.HomeId), year);
        var totalNumberOfPenalties =
            await GetTotalNumberById(con, PenaltyTable, homeId, nameof(PenaltyEntity.HomeId), year);
        var totalNumberOfAchievementsGranted =
            await GetTotalNumberById(con, AchievementsTable, homeId, nameof(AchievementEntity.HomeId), year,
                nameof(AchievementEntity.AchievedDateUtc));
        var totalNumberOfWishesFulfilled =
            await GetTotalNumberById(con, WishesTable, homeId, nameof(WishEntity.HomeId), year,
                nameof(WishEntity.FullFilledDateUtc));
        return new ParentAnalyticProfile
        {
            ViewType = ParentAnalyticViewType.AllParentsToAllChildren,
            Year = year,
            AssignmentsAssigned = totalNumberOfAssignments,
            AchievementsGranted = totalNumberOfAchievementsGranted,
            WishesFulfilled = totalNumberOfWishesFulfilled,
            PenaltiesSignedOff = totalNumberOfPenalties,
        };
    }

    public async Task<ParentAnalyticProfile> GetAllParentsToOneChildAnalytic(Guid childId, int year)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var totalNumberOfAssignments =
            await GetTotalNumberById(con, AssignmentsTable, childId, nameof(AssignmentEntity.AssigneeId), year);
        var totalNumberOfPenalties =
            await GetTotalNumberById(con, PenaltyTable, childId, nameof(PenaltyEntity.ViolatorId), year);
        var totalNumberOfAchievementsGranted =
            await GetTotalNumberById(con, AchievementsTable, childId, nameof(AchievementEntity.AchieverId), year,
                nameof(AchievementEntity.AchievedDateUtc));
        var totalNumberOfWishesFulfilled =
            await GetTotalNumberById(con, WishesTable, childId, nameof(WishEntity.WisherId), year,
                nameof(WishEntity.FullFilledDateUtc));

        return new ParentAnalyticProfile
        {
            ViewType = ParentAnalyticViewType.AllParentsToOneChild,
            Year = year,
            AssignmentsAssigned = totalNumberOfAssignments,
            AchievementsGranted = totalNumberOfAchievementsGranted,
            WishesFulfilled = totalNumberOfWishesFulfilled,
            PenaltiesSignedOff = totalNumberOfPenalties,
        };
    }

    private async Task<int> GetTotalNumberById(IDbConnection con, string tableName, Guid id, string idColumnName,
        int year)
    {
        var query =
            $"SELECT COUNT(*) FROM {tableName} WHERE {idColumnName} = @Id AND EXTRACT(YEAR FROM CreatedDateUtc) = @Year";
        return await con.QuerySingleAsync<int>(query, new { Id = id, Year = year });
    }

    private async Task<int> GetTotalNumberById(IDbConnection con, string tableName, Guid id, string idColumnName,
        int year,
        string nonNullableColumnName)
    {
        var query =
            $"SELECT COUNT(*) FROM {tableName} WHERE {idColumnName} = @Id AND EXTRACT(YEAR FROM CreatedDateUtc) = @Year AND {nonNullableColumnName} IS NOT NULL";
        return await con.QuerySingleAsync<int>(query, new { Id = id, Year = year });
    }
}