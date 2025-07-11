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
    [Function("GetAllWishesByParent")]
    public async Task<IActionResult> GetAllWishesByParent(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "parent/{id}/wishes")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        var (err, parentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await parentService.GetHomeIdByParentId(parentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.GetAllWishesByParentId(parentId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllWishesByChild")]
    public async Task<IActionResult> GetAllWishesByChild(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "child/{id}/wishes")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        var (err, childId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await childService.GetHomeIdByChildId(childId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.GetAllWishesByChildId(childId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllWishesByHome")]
    public async Task<IActionResult> GetAllWishesByHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/wishes")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.GetAllWishesByHomeId(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
            return new OkObjectResult(res);
        });
    }

    [Function("GetWishById")]
    public async Task<IActionResult> GetWishById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "wish/{id}")]
        HttpRequest req, string id)
    {
        var (err, wishId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await wishService.GetHomeIdByWishId(wishId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.GetWishById(wishId);
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