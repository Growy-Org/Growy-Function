using Growy.Function.Exceptions;
using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class ParentController(
    IParentService parentService,
    IAuthService authService)
{
    [Function("AddParentToHome")]
    public async Task<IActionResult> AddParentToHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/parent")]
        HttpRequest req,
        string id, [FromBody] ParentRequest request)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await parentService.AddParentToHome(homeId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("EditParent")]
    public async Task<IActionResult> EditParent(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "parent/{id}")]
        HttpRequest req, string id, [FromBody] ParentRequest request)
    {
        var (err, parentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await parentService.GetHomeIdByParentId(parentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await parentService.EditParent(parentId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("DeleteParent")]
    public async Task<IActionResult> DeleteParent(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "parent/{id}")]
        HttpRequest req, string id)
    {
        var (err, parentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await parentService.GetHomeIdByParentId(parentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            try
            {
                await parentService.DeleteParent(parentId);
            }
            catch (DeletionFailureException _)
            {
                return new ConflictObjectResult(
                    $"Failed to delete Parent with ID {parentId}, make sure all linked resources are deleted first");
            }

            return new NoContentResult();
        });
    }
}