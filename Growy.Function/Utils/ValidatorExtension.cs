

namespace Growy.Function.Utils;

public static class ValidatorExtension
{
    public static async Task<(string errorMessage, Guid? okResult)> VerifyIdFromHome(this string? id, Guid homeId,
        Func<Guid, Task<Guid>> retrieveHomeFunc, bool ensureIdExist = true)
    {
        if (string.IsNullOrEmpty(id))
        {
            return (ensureIdExist ? "id can not be empty or null" : string.Empty, null);
        }

        var (err, idGuid) = id.VerifyId();
        if (err != string.Empty) return (err, null);

        var retrievedHomeId = await retrieveHomeFunc(idGuid);
        if (retrievedHomeId != homeId)
        {
            return ($"id {id} does not belongs to the home {homeId}", null);
        }
        
        return (string.Empty, idGuid);
    }

    public static (string errorMessage, Guid okResult) VerifyId(this string id)
    {
        if (!Guid.TryParse(id, out var idGuid))
        {
            return ($"Invalid ID format: {id}", Guid.Empty);
        }

        return (string.Empty, idGuid);
    }
}