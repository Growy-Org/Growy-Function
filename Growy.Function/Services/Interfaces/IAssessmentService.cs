using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IAssessmentService
{
    // Read 
    // Create
    public Task<Guid> SubmitDevelopmentQuotientReport(Guid homeId, DevelopmentReportRequest request);
    // Updtae
    // Delete
}