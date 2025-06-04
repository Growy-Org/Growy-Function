using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Growy.Function.Controllers;

public class WishController(
    ILogger<WishController> logger,
    IWishService wishService,
    IAuthService authService)
{
    // Read
    [Function("GetAllWishesByParent")]
    public async Task<IActionResult> GetAllWishesByParent(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "parent/{id}/wishes")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var parentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await wishService.GetAllWishesByParentId(parentId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    [Function("GetAllWishesByChild")]
    public async Task<IActionResult> GetAllWishesByChild(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "child/{id}/wishes")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var childId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await wishService.GetAllWishesByChildId(childId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    [Function("GetAllWishesByHome")]
    public async Task<IActionResult> GetAllWishesByHome(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "home/{id}/wishes")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await wishService.GetAllWishesByHomeId(homeId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    [Function("GetWishById")]
    public async Task<IActionResult> GetWishById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "wish/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var wishId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await wishService.GetWishById(wishId);
        return new OkObjectResult(res);
    }


    // Create
    [Function("CreateWish")]
    public async Task<IActionResult> CreateWish(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "wish")]
        HttpRequest req, [FromBody] CreateWishRequest wishRequest)
    {
        var res = await wishService.CreateWish(wishRequest);
        return new OkObjectResult(res);
    }

    // Update
    [Function("EditWish")]
    public async Task<IActionResult> EditWish(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "wish")]
        HttpRequest req, [FromBody] EditWishRequest request)
    {
        // this is the same from backend POV for now as the children
        var res = await wishService.EditWish(request);
        return new OkObjectResult(res);
    }

    [Function("FulFillWish")]
    public async Task<IActionResult> FulFillWish(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "wish/{id}/fulfill")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var wishId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await wishService.SetWishFulFilled(wishId, true);
        return new OkObjectResult(res);
    }

    [Function("UnFulFillWish")]
    public async Task<IActionResult> UnFulFillWish(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "wish/{id}/unfulfill")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var wishId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await wishService.SetWishFulFilled(wishId, false);
        return new OkObjectResult(res);
    }

    // Delete
    [Function("DeleteWish")]
    public async Task<IActionResult> DeleteWish(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "wish/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var wishId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        await wishService.DeleteWish(wishId);
        return new OkResult();
    }
}