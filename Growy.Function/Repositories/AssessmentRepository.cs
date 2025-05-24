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

    public async Task<Guid> CreateReport(SubmitDevelopmentReportRequest request)
    {
        var dqResultEntity = request.ToDqAssessmentEntity();
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {DqTable} (HomeId, CandidateId, ExaminerId, Answer, DqResult, TotalScore, CandidateMonth) VALUES (@HomeId, @CandidateId, @ExaminerId, @Answer, @DqResult, @TotalScore, @CandidateMonth) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, dqResultEntity);
    }
}