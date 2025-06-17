using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IAnalyticRepository
{
    public Task<AnalyticProfile> GetAnalyticLive(Guid homeId, int year, Guid? childId = null);
}