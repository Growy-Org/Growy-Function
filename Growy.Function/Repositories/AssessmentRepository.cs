using System.Data;
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

    public async Task<List<DevelopmentQuotientResult>> GetAllReportsByHomeId(Guid homeId, int pageNumber, int pageSize)
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
                 LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize} 
             """;
        var assignmentEntities = await con.QueryAsync(query, _mapEntitiesToDqReportModel,
            new { HomeId = homeId });
        return assignmentEntities.ToList();
    }

    public async Task<Guid> CreateReport(Guid homeId, DevelopmentReportRequest request)
    {
        var dqResultEntity = request.ToDqAssessmentEntity();
        dqResultEntity.HomeId = homeId;
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {DqTable} (HomeId, CandidateId, ExaminerId, Answers, DqResult, TotalMentalAge, CandidateMonth) VALUES (@HomeId, @CandidateId, @ExaminerId, @Answers, @DqResult, @TotalMentalAge, @CandidateMonth) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, dqResultEntity);
    }

    public async Task<Guid> UpdateReport(Guid reportId, DevelopmentReportRequest request)
    {
        var dqResultEntity = request.ToDqAssessmentEntity();
        dqResultEntity.Id = reportId;
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
             UPDATE {DqTable} SET 
                CandidateId = @CandidateId, 
                ExaminerId = @CandidateId, 
                Answers = @Answers, 
                DqResult = @DqResult, 
                TotalMentalAge = @TotalMentalAge, 
                CandidateMonth = @CandidateMonth
             WHERE Id = @Id
             """;
        return await con.ExecuteScalarAsync<Guid>(query, dqResultEntity);
    }

    public async Task DeleteReportByReportId(Guid reportId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"DELETE FROM {DqTable} WHERE Id = @Id RETURNING Id;";
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