using Growy.Function.Models.Dtos;
using Growy.Function.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class ParentCapabilityController(
    ILogger<ParentCapabilityController> logger,
    IParentService parentService)
{
    #region Assignments

    [Function("GetAllAssignmentsByParent")]
    public async Task<IActionResult> GetAllAssignmentsByParent(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "parent/{id}/assignments")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var parentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.GetAllAssignmentsByParentId(parentId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    [Function("CreateAssignment")]
    public async Task<IActionResult> CreateAssignmentToHome(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/assignment")]
        HttpRequest req, [FromBody] CreateAssignmentRequest assignmentRequest)
    {
        var res = await parentService.CreateAssignment(assignmentRequest);
        return new OkObjectResult(res);
    }

    [Function("EditAssignment")]
    public async Task<IActionResult> EditAssignment(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "assignment")]
        HttpRequest req, [FromBody] EditAssignmentRequest request)
    {
        var res = await parentService.EditAssignment(request);
        return new OkObjectResult(res);
    }

    [Function("CompleteAssignment")]
    public async Task<IActionResult> CompleteAssignment(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "assignment/{id}/complete")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.EditAssignmentCompleteStatus(assignmentId, true);
        return new OkObjectResult(res);
    }

    [Function("UnCompleteAssignment")]
    public async Task<IActionResult> UnCompleteAssignment(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "assignment/{id}/incomplete")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.EditAssignmentCompleteStatus(assignmentId, false);
        return new OkObjectResult(res);
    }

    [Function("DeleteAssignment")]
    public async Task<IActionResult> DeleteAssignment(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "assignment/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        await parentService.DeleteAssignment(assignmentId);
        return new OkResult();
    }


    [Function("CreateStep")]
    public async Task<IActionResult> CreateStep(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "step")]
        HttpRequest req, [FromBody] CreateStepRequest stepRequest)
    {
        var res = await parentService.CreateStepToAssignment(stepRequest);
        return new OkObjectResult(res);
    }


    [Function("EditStep")]
    public async Task<IActionResult> EditStep(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "step")]
        HttpRequest req, [FromBody] EditStepRequest request)
    {
        var res = await parentService.EditStep(request);
        return new OkObjectResult(res);
    }


    [Function("CompleteStep")]
    public async Task<IActionResult> CompleteStep(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "step/{id}/complete")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var stepId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.EditStepCompleteStatus(stepId, true);
        return new OkObjectResult(res);
    }

    [Function("UnCompleteStep")]
    public async Task<IActionResult> UnCompleteStep(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "step/{id}/incomplete")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var stepId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.EditStepCompleteStatus(stepId, false);
        return new OkObjectResult(res);
    }

    [Function("DeleteStep")]
    public async Task<IActionResult> DeleteStep(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "step/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var stepId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        await parentService.DeleteStep(stepId);
        return new OkResult();
    }

    #endregion

    #region Achivements

    [Function("GetAllAchievementsByParent")]
    public async Task<IActionResult> GetAllAchievementsByParent(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "parent/{id}/achievements")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var parentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.GetAllAchievementByParentId(parentId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    [Function("CreateAchievement")]
    public async Task<IActionResult> CreateAchievement(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/achievement")]
        HttpRequest req, [FromBody] CreateAchievementRequest achievementRequest)
    {
        var res = await parentService.CreateAchievement(achievementRequest);
        return new OkObjectResult(res);
    }

    [Function("EditAchievement")]
    public async Task<IActionResult> EditAchievement(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "achievement")]
        HttpRequest req, [FromBody] EditAchievementRequest request)
    {
        var res = await parentService.EditAchievement(request);
        return new OkObjectResult(res);
    }


    [Function("GrantedAchievement")]
    public async Task<IActionResult> GrantedAchievement(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "achievement/{id}/grant")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var achievementId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.EditAchievementGrants(achievementId, true);
        return new OkObjectResult(res);
    }

    [Function("RevokeGrantedAchievement")]
    public async Task<IActionResult> RevokeGrantedAchievement(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "achievement/{id}/revoke-grant")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var achievementId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.EditAchievementGrants(achievementId, false);
        return new OkObjectResult(res);
    }

    [Function("DeleteAchievement")]
    public async Task<IActionResult> DeleteAchievement(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "achievement/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var achievementId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        await parentService.DeleteAchievement(achievementId);
        return new OkResult();
    }

    #endregion

    #region Wishes

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

        var res = await parentService.GetAllWishesByParentId(parentId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    [Function("CreateWishFromParent")]
    public async Task<IActionResult> CreateWishFromParent(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "parent/wish")]
        HttpRequest req, [FromBody] CreateWishRequest wishRequest)
    {
        // this is the same from backend POV for now as the children, Parent's create wish allow including cost
        // TODO: Add validation for the request
        var res = await parentService.CreateWish(wishRequest);
        return new OkObjectResult(res);
    }

    // can only edit points nothing else, UI controls the 
    [Function("EditWishFromParent")]
    public async Task<IActionResult> EditWishFromParent(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "parent/wish")]
        HttpRequest req, [FromBody] EditWishRequest request)
    {
        // this is the same from backend POV for now as the children
        var res = await parentService.EditWish(request);
        return new OkObjectResult(res);
    }

    [Function("FulFillWishFromParent")]
    public async Task<IActionResult> FulFillWishFromParent(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "parent/wish/{id}/fulfill")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var wishId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.SetWishFulFilled(wishId, true);
        return new OkObjectResult(res);
    }

    [Function("UnFulFillWishFromParent")]
    public async Task<IActionResult> UnFulFillWishFromParent(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "parent/wish/{id}/unfulfill")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var wishId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.SetWishFulFilled(wishId, false);
        return new OkObjectResult(res);
    }

    [Function("DeleteWishFromParent")]
    public async Task<IActionResult> DeleteWishFromParent(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "parent/wish/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var wishId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        await parentService.DeleteWish(wishId);
        return new OkResult();
    }

    #endregion

    #region Penalties

    [Function("GetAllPenaltiesByParent")]
    public async Task<IActionResult> GetAllPenaltiesByParent(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "parent/{id}/penalties")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.GetAllPenaltiesByParentId(homeId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    [Function("CreatePenalty")]
    public async Task<IActionResult> CreatePenalty(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/penalty")]
        HttpRequest req, [FromBody] CreatePenaltyRequest penaltyRequest)
    {
        var res = await parentService.CreatePenalty(penaltyRequest);
        return new OkObjectResult(res);
    }

    [Function("EditPenalty")]
    public async Task<IActionResult> EditPenalty(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "penalty")]
        HttpRequest req, [FromBody] EditPenaltyRequest request)
    {
        var res = await parentService.EditPenalty(request);
        return new OkObjectResult(res);
    }

    [Function("DeletePenalty")]
    public async Task<IActionResult> DeletePenalty(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "penalty/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var penaltyId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        await parentService.DeletePenalty(penaltyId);
        return new OkResult();
    }

    #endregion

    #region DevelopmentQuotient
    [Function("SubmitDevelopmentQuotientReport")]
    public async Task<IActionResult> SubmitDevelopmentQuotientReport(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "assessment/developmentquotientreport")]
        HttpRequest req, [FromBody] SubmitDevelopmentReportRequest request)
    {
        var res = await parentService.SubmitDevelopmentQuotientReport(request);
        return new OkObjectResult(res);
    }
    #endregion
}