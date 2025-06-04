using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IAssessmentRepository
{
    public Task<Guid> CreateReport(Guid homeId, DevelopmentReportRequest request);
}