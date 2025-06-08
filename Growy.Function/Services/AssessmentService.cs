using Growy.Function.Models.Dtos;
using Growy.Function.Repositories.Interfaces;
using Growy.Function.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class AssessmentService(
    IAssessmentRepository assessmentRepository,
    ILogger<AssessmentService> logger)
    : IAssessmentService
{
    #region Assessments

    public async Task<Guid> SubmitDevelopmentQuotientReport(Guid homeId, DevelopmentReportRequest request)
    {
        logger.LogInformation(
            $"Submitting Development Quotient Report for Child Id {request.ChildId} by {request.ParentId}");

        logger.LogInformation(
            $"Month: {request.CandidateMonth}, Total Score : {request.TotalMentalAge}, Result : {request.DqResult}");

        // calculate the result save it, just aggregate all the points divide by 5 and devide by the month
        // points array are  0.5,0.5,0.5,1.5,3,3, etc.. has to have 261 items, and if not then validation should fail
        return await assessmentRepository.CreateReport(homeId, request);
    }

    #endregion
}