using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Growy.Function.Services.Interfaces;

public interface IAuthWrapper
{
    Task<IActionResult> SecureExecute(HttpRequest req, Guid homeId, Func<Task<IActionResult>> func);
}