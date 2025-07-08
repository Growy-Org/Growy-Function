using System.Security.Authentication;
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

public class HomeController(
    IHomeService homeService,
    IAuthService authService)
{
    // Read
    [Function("GetHome")]
    public async Task<IActionResult> GetHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}")]
        HttpRequest req,
        string id)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await homeService.GetHomeInfoById(homeId);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllHomesByAppUserId")]
    public async Task<IActionResult> GetHomesByAppUserId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "homes")]
        HttpRequest req)
    {
        try
        {
            var appUserId = await authService.GetAppUserIdFromToken(req);
            var res = await homeService.GetHomesByAppUserId(appUserId);
            return new OkObjectResult(res);
        }
        catch (AuthenticationException e)
        {
            return new UnauthorizedObjectResult(e.Message);
        }
    }

    // Create
    [Function("AddHome")]
    public async Task<IActionResult> AddHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home")]
        HttpRequest req, [FromBody] HomeRequest request)
    {
        try
        {
            var appUserId = await authService.GetAppUserIdFromToken(req);
            var res = await homeService.CreateHome(appUserId, request);
            return new OkObjectResult(res);
        }
        catch (AuthenticationException e)
        {
            return new UnauthorizedObjectResult(e.Message);
        }
    }

    // Update
    [Function("EditHome")]
    public async Task<IActionResult> EditHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "home/{id}")]
        HttpRequest req, string id, [FromBody] HomeRequest request)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId,
            async () =>
            {
                var res = await homeService.EditHome(homeId, request);
                return new OkObjectResult(res);
            }
        );
    }

    // Delete
    [Function("DeleteHome")]
    public async Task<IActionResult> DeleteHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "home/{id}")]
        HttpRequest req, string id)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            try
            {
                await homeService.DeleteHome(homeId);
            }
            catch (DeletionFailureException _)
            {
                return new ConflictObjectResult(
                    $"Failed to delete Home with ID {homeId}, make sure all linked resources are deleted first");
            }

            return new NoContentResult();
        });
    }
}