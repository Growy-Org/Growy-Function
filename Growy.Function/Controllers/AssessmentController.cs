using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class AssessmentController(
    ILogger<AssessmentController> logger,
    IAssessmentService assessmentService,
    IAuthService authService)
{
    // Read
    [Function("GetAllDqAssessmentsByHome")]
    public async Task<IActionResult> GetAllDqAssessmentsByHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/assessment/dqreports")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assessmentService.GetAllDqAssessmentsByHome(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
            return new OkObjectResult(res);
        });
    }

    // Read
    [Function("GetDqAssessment")]
    public async Task<IActionResult> GetDqAssessment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assessment/dqreport/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var assessmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var homeId = await assessmentService.GetHomeIdByDqAssessmentId(assessmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assessmentService.GetDqAssessment(assessmentId);
            return new OkObjectResult(res);
        });
    }

    [Function("GetDqAssessmentsCount")]
    public async Task<IActionResult> GetDqAssessmentsCount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/assessment/dqreports/count")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assessmentService.GetDqAssessmentsPageCount(homeId);
            return new OkObjectResult(res);
        });
    }

    // Create
    [Function("SubmitDqReport")]
    public async Task<IActionResult> SubmitDqReport(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "home/{id}/assessment/dqreport")]
        HttpRequest req, string id, [FromBody] DevelopmentReportRequest request)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assessmentService.SubmitDqReport(homeId, request);
            return new OkObjectResult(res);
        });
    }

    // Update
    [Function("UpdateDqReport")]
    public async Task<IActionResult> UpdateDqReport(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "assessment/dqreport/{id}")]
        HttpRequest req, string id, [FromBody] DevelopmentReportRequest request)
    {
        if (!Guid.TryParse(id, out var assessmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var homeId = await assessmentService.GetHomeIdByDqAssessmentId(assessmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assessmentService.UpdateDqReport(assessmentId, request);
            return new OkObjectResult(res);
        });
    }

    // Delete
    [Function("DeleteDqReport")]
    public async Task<IActionResult> DeleteDqReport(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "assessment/dqreport/{id}")]
        HttpRequest req, string id, [FromBody] DevelopmentReportRequest request)
    {
        if (!Guid.TryParse(id, out var assessmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var homeId = await assessmentService.GetHomeIdByDqAssessmentId(assessmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            await assessmentService.DeleteDqReport(assessmentId);
            return new NoContentResult();
        });
    }
}