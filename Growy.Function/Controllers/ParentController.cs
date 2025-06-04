using Growy.Function.Exceptions;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class ParentController(
    ILogger<ChildController> logger,
    IParentService parentService,
    IAuthWrapper authWrapper)
{
    [Function("AddParentToHome")]
    public async Task<IActionResult> AddParentToHome(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/{id}/parent")]
        HttpRequest req,
        string id, [FromBody] Parent parent)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.AddParentToHome(homeId, parent);
        return new OkObjectResult(res);
    }

    [Function("EditParent")]
    public async Task<IActionResult> EditParent(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "parent")]
        HttpRequest req, [FromBody] EditParentRequest parent)
    {
        var res = await parentService.EditParent(parent);
        return new OkObjectResult(res);
    }


    [Function("DeleteParent")]
    public async Task<IActionResult> DeleteParent(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "parent/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var parentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        try
        {
            await parentService.DeleteParent(parentId);
        }
        catch (DeletionFailureException _)
        {
            return new ConflictObjectResult(
                $"Failed to delete Parent with ID {parentId}, make sure all linked resources are deleted first");
        }

        return new OkResult();
    }
}