using Growy.Function.Entities;
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

    // Read
    public async Task<int> GetDqAssessmentsPageCount(Guid homeId)
    {
        return await assessmentRepository.GetDqAssessmentsCount(homeId);
    }

    public async Task<List<DevelopmentQuotientResult>> GetAllDqAssessmentsByHome(Guid homeId, int pageNumber,
        int pageSize)
    {
        logger.LogInformation(
            $"Getting all dq assessments with homeId {homeId.ToString()}");
        return await assessmentRepository.GetAllDqAssessmentsByHome(homeId, pageNumber, pageSize);
    }

    public async Task<DevelopmentQuotientResult> GetDqAssessment(Guid assessmentId)
    {
        logger.LogInformation(
            $"Getting single dq assessment with id {assessmentId.ToString()}");
        return await assessmentRepository.GetDqAssessment(assessmentId);
    }

    public async Task<Guid> GetHomeIdByDqAssessmentId(Guid assessmentId)
    {
        return await assessmentRepository.GetHomeIdByDqAssessmentId(assessmentId);
    }

    // Create
    public async Task<Guid> SubmitDqReport(Guid homeId, DevelopmentReportRequest request)
    {
        logger.LogInformation(
            $"Submitting Development Quotient Report for Child Id {request.ChildId} by {request.ParentId}");

        logger.LogInformation(
            $"Month: {request.CandidateMonth}, Total Mental Age : {request.TotalMentalAge}, Result : {request.DqResult}");

        try
        {
            logger.LogInformation("Creating report");
            return await assessmentRepository.CreateDqReport(homeId, request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Guid();
        }
    }

    // Update
    public async Task<Guid> UpdateDqReport(Guid assessmentId, DevelopmentReportRequest request)
    {
        logger.LogInformation(
            $"Updating Development Quotient Report for {assessmentId}");

        return await assessmentRepository.UpdateDqReport(assessmentId, request);
    }

    // Delete
    public async Task DeleteDqReport(Guid assessmentId)
    {
        logger.LogInformation(
            $"Deleting Development Quotient Report {assessmentId}");
        await assessmentRepository.DeleteDqReport(assessmentId);
    }

    #endregion
}