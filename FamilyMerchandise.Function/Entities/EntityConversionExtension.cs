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

    public static void HydrateParents(this Home h, List<Parent> parents)
    {
        h.Parents.AddRange(parents);
    }

    public static void HydrateChildren(this Home h, List<Child> children)
    {
        h.Children.AddRange(children);
    }

    public static Parent ToParent(this ParentEntity p)
    {
        return new Parent
        {
            Id = p.Id,
            Name = p.Name,
            Role = p.Role,
            IconCode = p.IconCode,
            DOB = p.DOB,
        };
    }

    public static Child ToChild(this ChildEntity c)
    {
        return new Child()
        {
            Id = c.Id,
            Name = c.Name,
            Gender = c.Gender,
            IconCode = c.IconCode,
            PointsEarned = c.PointsEarned,
            DOB = c.DOB,
        };
    }
}