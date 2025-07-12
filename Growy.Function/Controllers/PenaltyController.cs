using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class PenaltyController(
    IPenaltyService penaltyService,
    IParentService parentService,
    IChildService childService,
    IAuthService authService)
{
    // Read
    [Function("GetPenaltiesCount")]
    public async Task<IActionResult> GetPenaltiesCount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/penalties/count")]
        HttpRequest req, string id, [FromQuery] string? parentId, [FromQuery] string? childId)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await penaltyService.GetPenaltiesCount(homeId, parentIdGuid, childIdGuid);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllPenalties")]
    public async Task<IActionResult> GetAllPenalties(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/penalties")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize,
        [FromQuery] string? parentId, [FromQuery] string? childId)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await penaltyService.GetAllPenalties(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE, parentIdGuid, childIdGuid);
            return new OkObjectResult(res);
        });
    }

    // Create
    [Function("CreatePenalty")]
    public async Task<IActionResult> CreatePenalty(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/penalty")]
        HttpRequest req, string id, [FromBody] PenaltyRequest penaltyRequest)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await penaltyService.CreatePenalty(homeId, penaltyRequest);
            return new OkObjectResult(res);
        });
    }

    // Update
    [Function("EditPenalty")]
    public async Task<IActionResult> EditPenalty(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "penalty/{id}")]
        HttpRequest req, string id, [FromBody] PenaltyRequest request)
    {
        var (err, penaltyId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await penaltyService.GetHomeIdByPenaltyId(penaltyId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await penaltyService.EditPenalty(penaltyId, request);
            return new OkObjectResult(res);
        });
    }

    // Delete
    [Function("DeletePenalty")]
    public async Task<IActionResult> DeletePenalty(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "penalty/{id}")]
        HttpRequest req, string id)
    {
        var (err, penaltyId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await penaltyService.GetHomeIdByPenaltyId(penaltyId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            await penaltyService.DeletePenalty(penaltyId);
            return new NoContentResult();
        });
    }
}