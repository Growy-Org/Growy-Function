using Growy.Function.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Growy.Function.Services.Interfaces;

public interface IAuthService
{
    // Create
    public Task<Guid> RegisterUser(AppUser appUser);
    Task<IActionResult> SecureExecute(HttpRequest req, Guid homeId, Func<Task<IActionResult>> func);
    Task<Guid> GetAppUserIdFromToken(HttpRequest req);
    string GetIdpIdFromToken(HttpRequest req);
}