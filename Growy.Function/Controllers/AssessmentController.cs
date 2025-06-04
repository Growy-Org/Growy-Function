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
    [Function("SubmitDevelopmentQuotientReport")]
    public async Task<IActionResult> SubmitDevelopmentQuotientReport(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/assessment/developmentquotientreport")]
        HttpRequest req, string id, [FromBody] DevelopmentReportRequest request)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assessmentService.SubmitDevelopmentQuotientReport(homeId, request);
            return new OkObjectResult(res);
        });
    }
}