using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Growy.Function.Services;

public interface IAuthWrapper
{
    Task<IActionResult> SecureExecute(HttpRequest req, Func<Task<IActionResult>> func);
}