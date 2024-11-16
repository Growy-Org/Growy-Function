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
        HttpRequest req, [FromBody] Home home)
    {
        var res = await homeService.CreateHome(home);
        return new OkObjectResult(res);
    }

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
    
    [Function("EditHome")]
    public async Task<IActionResult> EditHome(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "home")]
        HttpRequest req,
        string id, [FromBody] EditHomeRequest request)
    {
        var res = await homeService.EditHome(request);
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
    
    [Function("EditParent")]
    public async Task<IActionResult> EditParent(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "parent")]
        HttpRequest req,
        string id, [FromBody] EditParentRequest parent)
    {
        var res = await homeService.EditParent(parent);
        return new OkObjectResult(res);
    }
}