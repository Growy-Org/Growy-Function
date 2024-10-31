using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Models;

public static class ModelConversionExtension
{
    public static ChildEntity ToChildEntity(this Child c, Guid homeId)
    {
        return new ChildEntity
        {
            Name = c.Name,
            IconCode = c.IconCode ?? 15, // Default Child Avatar
            DOB = c.DOB,
            Gender = c.Gender.ToString(),
            HomeId = homeId,
            PointsEarned = c.PointsEarned ?? 0,
        };
    }

    public static HomeEntity ToHomeEntity(this Home h)
    {
        return new HomeEntity()
        {
            Name = h.Name
        };
    }
}