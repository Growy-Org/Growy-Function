using FamilyMerchandise.Function.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyMerchandise.Function.Controllers;

public class HomeController(ILogger<HomeController> logger, IHomeService homeService)
{
    [Function("Home")]
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

        logger.LogInformation($"Getting Home By Id: {homeId}");
        var res = await homeService.GetHomeInfoById(homeId);
        return new OkObjectResult(res);
    }
}