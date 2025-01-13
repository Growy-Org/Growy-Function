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
        var totalNumberOfAssignments = await GetTotalNumbersByHomeId(con, AssignmentsTable, homeId, year);
        var totalNumberOfPenalties = await GetTotalNumbersByHomeId(con, PenaltyTable, homeId, year);
        var totalNumberOfAchievementsGranted =
            await GetTotalNumbersByHomeId(con, AchievementsTable, homeId, year, "AchievedDateUtc");
        var totalNumberOfWishesFulfilled =
            await GetTotalNumbersByHomeId(con, WishesTable, homeId, year, "FullFilledDateUtc");
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

    private async Task<int> GetTotalNumbersByHomeId(IDbConnection con, string tableName, Guid homeId, int year)
    {
        var query =
            $"SELECT COUNT(*) FROM {tableName} WHERE HomeId = @Id AND EXTRACT(YEAR FROM CreatedDateUtc) = @Year";
        return await con.QuerySingleAsync<int>(query, new { Id = homeId, Year = year });
    }

    private async Task<int> GetTotalNumbersByHomeId(IDbConnection con, string tableName, Guid homeId, int year,
        string nonNullableColumnName)
    {
        var query =
            $"SELECT COUNT(*) FROM {tableName} WHERE HomeId = @Id AND EXTRACT(YEAR FROM CreatedDateUtc) = @Year AND {nonNullableColumnName} IS NOT NULL";
        return await con.QuerySingleAsync<int>(query, new { Id = homeId, Year = year });
    }
}