using Growy.Function.Exceptions;
using Growy.Function.Models;
using Growy.Function.Services;
using Growy.Function.Services.Interfaces;
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
        string id, [FromBody] Child child)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await childService.AddChildToHome(homeId, child);
            return new OkObjectResult(res);
        });
    }

    [Function("EditChild")]
    public async Task<IActionResult> EditChild(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "child/{id}")]
        HttpRequest req, string id, [FromBody] Child child)
    {
        if (!Guid.TryParse(id, out var childId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var homeId = await childService.GetHomeIdByChildId(childId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            child.Id = childId;
            var res = await childService.EditChild(child);
            return new OkObjectResult(res);
        });
    }

    [Function("DeleteChild")]
    public async Task<IActionResult> DeleteChild(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "child/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var childId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

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