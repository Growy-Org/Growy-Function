using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Entities;

public static class EntityConversionExtension
{
    public static Home ToHome(this HomeEntity e)
    {
        return new Home
        {
            Id = e.Id,
            Name = e.Name,
        };
    }
}