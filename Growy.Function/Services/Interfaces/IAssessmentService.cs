using Growy.Function.Entities;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Services.Interfaces;

public interface IAssessmentService
{
    // Read 
    public Task<int> GetDqAssessmentsPageCount(Guid homeId);

    public Task<List<DevelopmentQuotientResult>> GetAllDqAssessmentsByHome(Guid homeId, int pageNumber, int pageSize);

    public Task<Guid> GetHomeIdByDqAssessmentId(Guid assessmentId);

    // Create
    public Task<Guid> SubmitDqReport(Guid homeId, DevelopmentReportRequest request);

    // Update
    public Task<Guid> UpdateDqReport(Guid assessmentId, DevelopmentReportRequest request);

    // Delete
    public Task DeleteDqReport(Guid assessmentId);
}