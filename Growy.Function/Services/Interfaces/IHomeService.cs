using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Services.Interfaces;

public interface IHomeService
{
    // Read
    public Task<Home> GetHomeInfoById(Guid homeId);
    public Task<List<Home>> GetHomesByAppUserId(Guid appUserId);
    
    // Create
    public Task<Guid> CreateHome(Guid appUserId, HomeRequest home);
    
    // Update
    public Task<Guid> EditHome(Guid homeId, HomeRequest request);
    
    // Delete
    public Task DeleteHome(Guid homeId);
}