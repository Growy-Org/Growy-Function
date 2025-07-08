using Growy.Function.Exceptions;
using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class ChildController(
    ILogger<ChildController> logger,
    IChildService childService,
    IAuthService authService)
{
    [Function("AddChildToHome")]
    public async Task<IActionResult> AddChildToHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/child")]
        HttpRequest req,
        string id, [FromBody] ChildRequest request)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await childService.AddChildToHome(homeId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("EditChild")]
    public async Task<IActionResult> EditChild(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "child/{id}")]
        HttpRequest req, string id, [FromBody] ChildRequest request)
    {
        var (err, childId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await childService.GetHomeIdByChildId(childId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await childService.EditChild(childId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("DeleteChild")]
    public async Task<IActionResult> DeleteChild(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "child/{id}")]
        HttpRequest req, string id)
    {
        var (err, childId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await childService.GetHomeIdByChildId(childId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            try
            {
                await childService.DeleteChild(childId);
            }
            catch (DeletionFailureException _)
            {
                return new ConflictObjectResult(
                    $"Failed to delete Child with ID {childId}, make sure all linked resources are deleted first");
            }

            return new NoContentResult();
        });
    }
}