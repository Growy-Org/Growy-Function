using FamilyMerchandise.Function.Exceptions;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace FamilyMerchandise.Function.Controllers;

public class HomeCapabilityController(
    ILogger<HomeCapabilityController> logger,
    IHomeService homeService)
{
    #region Homes

    [Function("GetHome")]
    public async Task<IActionResult> GetHome(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "home/{id}")]
        HttpRequest req,
        string id)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await homeService.GetHomeInfoById(homeId);
        return new OkObjectResult(res);
    }

    [Function("AddHome")]
    public async Task<IActionResult> AddHome(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home")]
        HttpRequest req, [FromBody] CreateHomeRequest homeRequest)
    {
        var res = await homeService.CreateHome(homeRequest);
        return new OkObjectResult(res);
    }

    [Function("EditHome")]
    public async Task<IActionResult> EditHome(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "home")]
        HttpRequest req,
        string id, [FromBody] EditHomeRequest request)
    {
        var res = await homeService.EditHome(request);
        return new OkObjectResult(res);
    }

    [Function("DeleteHome")]
    public async Task<IActionResult> DeleteHome(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "home/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        try
        {
            await homeService.DeleteHome(homeId);
        }
        catch (DeletionFailureException _)
        {
            return new ConflictObjectResult(
                $"Failed to delete Home with ID {homeId}, make sure all linked resources are deleted first");
        }

        return new OkResult();
    }

    #endregion

    #region Children

    [Function("AddChildToHome")]
    public async Task<IActionResult> AddChildToHome(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/{id}/child")]
        HttpRequest req,
        string id, [FromBody] Child child)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await homeService.AddChildToHome(homeId, child);
        return new OkObjectResult(res);
    }

    [Function("EditChild")]
    public async Task<IActionResult> EditChild(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "child")]
        HttpRequest req,
        string id, [FromBody] EditChildRequest request)
    {
        var res = await homeService.EditChild(request);
        return new OkObjectResult(res);
    }

    [Function("DeleteChild")]
    public async Task<IActionResult> DeleteChild(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "child/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var childId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        try
        {
            await homeService.DeleteChild(childId);
        }
        catch (DeletionFailureException _)
        {
            return new ConflictObjectResult(
                $"Failed to delete Child with ID {childId}, make sure all linked resources are deleted first");
        }

        return new OkResult();
    }

    #endregion

    #region Parents

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

        var res = await homeService.AddParentToHome(homeId, parent);
        return new OkObjectResult(res);
    }

    [Function("EditParent")]
    public async Task<IActionResult> EditParent(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "parent")]
        HttpRequest req,
        string id, [FromBody] EditParentRequest parent)
    {
        var res = await homeService.EditParent(parent);
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
            await homeService.DeleteParent(parentId);
        }
        catch (DeletionFailureException _)
        {
            return new ConflictObjectResult(
                $"Failed to delete Parent with ID {parentId}, make sure all linked resources are deleted first");
        }

        return new OkResult();
    }

    #endregion

    #region Assignments

    [Function("GetAllAssignmentsByHome")]
    public async Task<IActionResult> GetAllAssignmentsByHome(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "home/{id}/assignments")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await homeService.GetAllAssignmentsByHomeId(homeId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    //  in the future, authorisation should prevent accessing ot other's home's assignment
    [Function("GetAssignmentById")]
    public async Task<IActionResult> GetAssignmentById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "assignment/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await homeService.GetAssignmentById(assignmentId);
        return new OkObjectResult(res);
    }

    #endregion

    #region Achievements

    [Function("GetAllAchievementsByHome")]
    public async Task<IActionResult> GetAllAchievementsByHome(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "home/{id}/achievements")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await homeService.GetAllAchievementByHomeId(homeId);
        return new OkObjectResult(res);
    }

    #endregion

    #region Wishes

    [Function("GetAllWishesByHome")]
    public async Task<IActionResult> GetAllWishesByHome(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "home/{id}/wishes")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await homeService.GetAllWishesByHomeId(homeId);
        return new OkObjectResult(res);
    }

    #endregion

    #region Penalties

    [Function("GetAllPenaltiesByHome")]
    public async Task<IActionResult> GetAllPenaltiesByHome(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "home/{id}/penalties")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await homeService.GetAllPenaltiesByHomeId(homeId);
        return new OkObjectResult(res);
    }

    #endregion
}