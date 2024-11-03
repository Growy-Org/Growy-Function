using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace FamilyMerchandise.Function.Controllers;

public class ChildCapabilityController(
    ILogger<ChildCapabilityController> logger,
    IChildService childService)
{
    
    [Function("CreateWish")]
    public async Task<IActionResult> CreateWish(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/wish")]
        HttpRequest req, [FromBody] CreateWishRequest wishRequest)
    {
        var res = await childService.CreateWish(wishRequest);
        return new OkObjectResult(res);
    }

}