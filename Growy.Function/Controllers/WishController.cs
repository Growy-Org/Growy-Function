using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class WishController(
    IWishService wishService,
    IParentService parentService,
    IChildService childService,
    IAuthService authService)
{
    // Read
    [Function("GetWishesCount")]
    public async Task<IActionResult> GetWishesCount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/wishes/count")]
        HttpRequest req, string id, [FromQuery] string? parentId, [FromQuery] string? childId,
        [FromQuery] string? showOnlyNotFulfilled)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        if (!bool.TryParse(showOnlyNotFulfilled, out var showOnlyNotFulfilledBool))
        {
            showOnlyNotFulfilledBool = false;
        }

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.GetWishesCount(homeId, parentIdGuid, childIdGuid,
                showOnlyNotFulfilledBool);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllWishes")]
    public async Task<IActionResult> GetAllAchievements(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/wishes")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize,
        [FromQuery] string? parentId, [FromQuery] string? childId, [FromQuery] string? showOnlyNotFulfilled)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        if (!bool.TryParse(showOnlyNotFulfilled, out var showOnlyNotFulfilledBool))
        {
            showOnlyNotFulfilledBool = false;
        }

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.GetAllWishes(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE, parentIdGuid, childIdGuid
                , showOnlyNotFulfilledBool);
            return new OkObjectResult(res);
        });
    }
  

    // Create
    [Function("CreateWish")]
    public async Task<IActionResult> CreateWish(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/wish")]
        HttpRequest req, string id, [FromBody] WishRequest request)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.CreateWish(homeId, request);
            return new OkObjectResult(res);
        });
    }

    // Update
    [Function("EditWish")]
    public async Task<IActionResult> EditWish(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "wish/{id}")]
        HttpRequest req, string id, [FromBody] WishRequest request)
    {
        var (err, wishId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await wishService.GetHomeIdByWishId(wishId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.EditWish(wishId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("FulFillWish")]
    public async Task<IActionResult> FulFillWish(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "wish/{id}/fulfill")]
        HttpRequest req, string id)
    {
        var (err, wishId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await wishService.GetHomeIdByWishId(wishId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.SetWishFulFilled(wishId, true);
            return new OkObjectResult(res);
        });
    }

    [Function("UnFulFillWish")]
    public async Task<IActionResult> UnFulFillWish(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "wish/{id}/unfulfill")]
        HttpRequest req, string id)
    {
        var (err, wishId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await wishService.GetHomeIdByWishId(wishId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.SetWishFulFilled(wishId, false);
            return new OkObjectResult(res);
        });
    }

    // Delete
    [Function("DeleteWish")]
    public async Task<IActionResult> DeleteWish(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "wish/{id}")]
        HttpRequest req, string id)
    {
        var (err, wishId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await wishService.GetHomeIdByWishId(wishId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            await wishService.DeleteWish(wishId);
            return new NoContentResult();
        });
    }
}