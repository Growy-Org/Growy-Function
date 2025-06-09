using Dapper;
using Growy.Function.Entities;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Repositories.Interfaces;

namespace Growy.Function.Repositories;

public class AssessmentRepository(IConnectionFactory connectionFactory) : IAssessmentRepository
{
    private const string DqTable = "assessment.DevelopmentQuotientResult";
    private const string ChildrenTable = "inventory.children";
    private const string ParentTable = "inventory.parents";

    public async Task<int> GetDqAssessmentsCount(Guid homeId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT COUNT(*) FROM {DqTable} WHERE HomeId = @HomeId;
             """;
        return await con.QuerySingleAsync<int>(query, new { HomeId = homeId });
    }

    public async Task<Guid> GetHomeIdByDqAssessmentId(Guid assessmentId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT HomeId FROM {DqTable} WHERE Id = @Id;
             """;
        return await con.QuerySingleAsync<Guid>(query, new { Id = assessmentId });
    }

    public async Task<List<DevelopmentQuotientResult>> GetAllDqAssessmentsByHome(Guid homeId, int pageNumber,
        int pageSize)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {DqTable} d
                 LEFT JOIN {ChildrenTable} c ON d.CandidateId = c.Id
                 LEFT JOIN {ParentTable} p ON d.ExaminerId = p.Id
                 WHERE d.HomeId = @HomeId
                 ORDER BY d.CreatedDateUtc ASC
                 LIMIT @PageSize OFFSET (@PageNumber - 1) * @PageSize;
             """;
        var assignmentEntities = await con.QueryAsync(query, _mapEntitiesToDqReportModel,
            new { HomeId = homeId, PageSize = pageSize, PageNumber = pageNumber });
        return assignmentEntities.ToList();
    }

    public async Task<Guid> CreateDqReport(Guid homeId, DevelopmentReportRequest request)
    {
        var dqResultEntity = request.ToDqAssessmentEntity();
        dqResultEntity.HomeId = homeId;
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {DqTable} (HomeId, CandidateId, ExaminerId, Answers, DqResult, TotalMentalAge, CandidateMonth) VALUES (@HomeId, @CandidateId, @ExaminerId, @Answers, @DqResult, @TotalMentalAge, @CandidateMonth) RETURNING Id;";
        return await con.ExecuteScalarAsync<Guid>(query, dqResultEntity);
    }

    public async Task<Guid> UpdateDqReport(Guid reportId, DevelopmentReportRequest request)
    {
        var dqResultEntity = request.ToDqAssessmentEntity();
        dqResultEntity.Id = reportId;
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
             UPDATE {DqTable} SET 
                CandidateId = @CandidateId, 
                ExaminerId = @ExaminerId, 
                Answers = @Answers, 
                DqResult = @DqResult, 
                TotalMentalAge = @TotalMentalAge, 
                CandidateMonth = @CandidateMonth
             WHERE Id = @Id;
             """;
        return await con.ExecuteScalarAsync<Guid>(query, dqResultEntity);
    }

    public async Task DeleteDqReport(Guid reportId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"DELETE FROM {DqTable} WHERE Id = @Id;";
        await con.ExecuteScalarAsync<Guid>(query, new { Id = reportId });
    }

    private readonly Func<DevelopmentQuotientResultEntity, ChildEntity, ParentEntity, DevelopmentQuotientResult>
        _mapEntitiesToDqReportModel =
            (d, c, p) =>
            {
                var report = d.ToDevelopmentQuotientResult();
                report.Candidate = c.ToChild();
                report.Examiner = p.ToParent();
                return report;
            };
}