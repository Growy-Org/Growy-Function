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
    IAssessmentService assessmentService)
{
    [Function("SubmitDevelopmentQuotientReport")]
    public async Task<IActionResult> SubmitDevelopmentQuotientReport(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "assessment/developmentquotientreport")]
        HttpRequest req, [FromBody] SubmitDevelopmentReportRequest request)
    {
        var res = await assessmentService.SubmitDevelopmentQuotientReport(request);
        return new OkObjectResult(res);
    }
}