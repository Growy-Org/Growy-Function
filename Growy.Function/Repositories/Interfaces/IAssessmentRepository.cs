using Growy.Function.Entities;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IAssessmentRepository
{
    public Task<int> GetDqAssessmentsCount(Guid homeId);
    public Task<DevelopmentQuotientResult> GetDqAssessment(Guid assessmentId);
    public Task<List<DevelopmentQuotientResult>> GetAllDqAssessmentsByHome(Guid homeId, int pageNumber, int pageSize);
    public Task<Guid> GetHomeIdByDqAssessmentId(Guid assessmentId);
    public Task<Guid> CreateDqReport(Guid homeId, DevelopmentReportRequest request);
    public Task<Guid> UpdateDqReport(Guid assessmentId, DevelopmentReportRequest request);
    public Task DeleteDqReport(Guid assessmentId);
}