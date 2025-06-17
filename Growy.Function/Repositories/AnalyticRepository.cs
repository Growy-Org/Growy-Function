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

    public async Task<AnalyticProfile> GetAnalyticLive(Guid homeId, int year, Guid? childId = null)
    {
        using var con = await connectionFactory.GetDBConnection();
        var totalNumberOfAssignments =
            await GetTotalNumberById(con, AssignmentsTable, homeId, year, childId, nameof(AssignmentEntity.AssigneeId));
        var totalNumberOfAchievementsCreated = await GetTotalNumberById(con, AchievementsTable, homeId, year, childId,
            nameof(AchievementEntity.AchieverId));
        var totalNumberOfWishesCreated =
            await GetTotalNumberById(con, WishesTable, homeId, year, childId, nameof(WishEntity.WisherId));

        // Sum
        // Created Date of penalty won't be null
        var (pointsReduced, totalNumberOfPenalties) =
            await GetSumNumberById(con, PenaltyTable, nameof(PenaltyEntity.PointsDeducted), childId ?? homeId,
                childId == null ? nameof(PenaltyEntity.HomeId) : nameof(PenaltyEntity.ViolatorId), year,
                nameof(PenaltyEntity.CreatedDateUtc));

        var (pointsEarned, totalNumberOfAssignmentsCompleted) =
            await GetSumNumberById(con, AssignmentsTable, nameof(AssignmentEntity.Points), childId ?? homeId,
                childId == null ? nameof(PenaltyEntity.HomeId) : nameof(AssignmentEntity.AssigneeId), year,
                nameof(AssignmentEntity.CompletedDateUtc));

        var (pointsGranted, totalNumberOfAchievementsAchieved) =
            await GetSumNumberById(con, AchievementsTable, nameof(AchievementEntity.PointsGranted), childId ?? homeId,
                childId == null ? nameof(PenaltyEntity.HomeId) : nameof(AchievementEntity.AchieverId), year,
                nameof(AchievementEntity.AchievedDateUtc));

        var (pointsSpent, totalNumberOfWishesRealised) =
            await GetSumNumberById(con, WishesTable, nameof(WishEntity.PointsCost), childId ?? homeId,
                childId == null ? nameof(PenaltyEntity.HomeId) : nameof(WishEntity.WisherId), year,
                nameof(WishEntity.FulFilledDateUtc));
        return new AnalyticProfile
        {
            Year = year,
            NumberOfAssignmentsAssigned = totalNumberOfAssignments,
            NumberOfAssignmentsCompleted = totalNumberOfAssignmentsCompleted,
            NumberOfAchievementsCreated = totalNumberOfAchievementsCreated,
            NumberOfAchievementsGranted = totalNumberOfAchievementsAchieved,
            NumberOfWishesCreated = totalNumberOfWishesCreated,
            NumberOfWishesRealised = totalNumberOfWishesRealised,
            NumberOfPenaltyReceived = totalNumberOfPenalties,
            PointsSpent = pointsSpent,
            PointsEarnedInAssignment = pointsEarned,
            PointsGrantedInAchievement = pointsGranted,
            PointsReduced = pointsReduced,
        };
    }

    private async Task<(int, int)> GetSumNumberById(IDbConnection con, string tableName, string aggregatedColumnName,
        Guid id,
        string idColumnName,
        int year, string nonNullableColumnName)
    {
        var query =
            $"SELECT SUM({aggregatedColumnName}), COUNT(*) FROM {tableName} WHERE {idColumnName} = @Id AND EXTRACT(YEAR FROM CreatedDateUtc) = @Year AND {nonNullableColumnName} IS NOT NULL";
        return await con.QuerySingleAsync<(int, int)>(query, new { Id = id, Year = year });
    }

    private async Task<int> GetTotalNumberById(IDbConnection con, string tableName, Guid homeId,
        int year, Guid? childId = null, string? childIdColumnName = "")
    {
        var childExtraClause = childId != null ? $"AND {childIdColumnName} = @ChildId" : "";
        var query =
            $"SELECT COUNT(*) FROM {tableName} WHERE HomeId = @HomeId {childExtraClause} AND EXTRACT(YEAR FROM CreatedDateUtc) = @Year";
        return await con.QuerySingleAsync<int>(query, new { HomeId = homeId, ChildId = childId, Year = year });
    }
}