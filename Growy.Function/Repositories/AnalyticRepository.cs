using System.Data;
using Dapper;
using Growy.Function.Entities;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;

namespace Growy.Function.Repositories;

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
                nameof(WishEntity.FulFilledDateUtc));
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
                nameof(WishEntity.FulFilledDateUtc));

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

    public async Task<ChildAnalyticProfile> GetChildAnalyticByChildId(Guid childId, int year)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();

        var (pointsReduced, totalNumberOfPenalties) =
            await GetSumNumberById(con, PenaltyTable, nameof(PenaltyEntity.PointsDeducted), childId,
                nameof(PenaltyEntity.ViolatorId), year);

        var (pointsEarned, totalNumberOfAssignmentsCompleted) =
            await GetSumNumberById(con, AssignmentsTable, nameof(AssignmentEntity.Points), childId,
                nameof(AssignmentEntity.AssigneeId), year, nameof(AssignmentEntity.CompletedDateUtc));

        var (pointsGranted, totalNumberOfAchievementsAchieved) =
            await GetSumNumberById(con, AchievementsTable, nameof(AchievementEntity.PointsGranted), childId,
                nameof(AchievementEntity.AchieverId), year, nameof(AchievementEntity.AchievedDateUtc));

        var (pointsSpent, totalNumberOfWishesRealised) =
            await GetSumNumberById(con, WishesTable, nameof(WishEntity.PointsCost), childId,
                nameof(WishEntity.WisherId), year, nameof(WishEntity.FulFilledDateUtc));

        return new ChildAnalyticProfile
        {
            ViewType = ChildAnalyticViewType.ByChild,
            Year = year,
            PenaltyReceived = totalNumberOfPenalties,
            AssignmentsCompleted = totalNumberOfAssignmentsCompleted,
            AchievementsAchieved = totalNumberOfAchievementsAchieved,
            WishesRealised = totalNumberOfWishesRealised,
            PointsSpent = pointsSpent,
            PointsGained = pointsEarned + pointsGranted,
            PointsReduced = pointsReduced
        };
    }

    private async Task<(int, int)> GetSumNumberById(IDbConnection con, string tableName, string aggregatedColumnName, Guid id,
        string idColumnName,
        int year)
    {
        var query =
            $"SELECT SUM({aggregatedColumnName}), COUNT(*) FROM {tableName} WHERE {idColumnName} = @Id AND EXTRACT(YEAR FROM CreatedDateUtc) = @Year";
        return await con.QuerySingleAsync<(int, int)>(query, new { Id = id, Year = year });
    }

    private async Task<(int, int)> GetSumNumberById(IDbConnection con, string tableName, string aggregatedColumnName, Guid id,
        string idColumnName,
        int year, string nonNullableColumnName)
    {
        var query =
            $"SELECT SUM({aggregatedColumnName}), COUNT(*) FROM {tableName} WHERE {idColumnName} = @Id AND EXTRACT(YEAR FROM CreatedDateUtc) = @Year AND {nonNullableColumnName} IS NOT NULL";
        return await con.QuerySingleAsync<(int, int)>(query, new { Id = id, Year = year });
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