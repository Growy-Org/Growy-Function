using FamilyMerchandise.Function.Models.Dtos;
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
            Gender = c.Gender,
            HomeId = homeId,
            PointsEarned = c.PointsEarned ?? 0,
        };
    }

    public static ParentEntity ToParentEntity(this Parent p, Guid homeId)
    {
        return new ParentEntity
        {
            Name = p.Name,
            IconCode = p.IconCode ?? 5, // Default Child Avatar
            DOB = p.DOB,
            Role = p.Role,
            HomeId = homeId,
        };
    }

    public static HomeEntity ToHomeEntity(this Home h)
    {
        return new HomeEntity()
        {
            Name = h.Name
        };
    }

    public static AssignmentEntity ToAssignmentEntity(this CreateAssignmentRequest r)
    {
        return new AssignmentEntity
        {
            Name = r.AssignmentName,
            Description = r.AssignmentDescription,
            HomeId = r.HomeId,
            IconCode = r.AssignmentIconCode,
            Points = r.Points,
            RepeatAfter = r.RepeatAfter,
            DueDate = r.DueDate,
            AssigneeId = r.ChildId,
            AssignerId = r.ParentId
        };
    }

    public static WishEntity ToWishEntity(this CreateWishRequest r)
    {
        return new WishEntity
        {
            HomeId = r.HomeId,
            GenieId = r.ParentId,
            WisherId = r.ChildId,
            Name = r.WishName,
            Description = r.WishDescription,
            IconCode = r.WishIconCode
        };
    }

    public static AchievementEntity ToAchievementEntity(this CreateAchievementRequest r)
    {
        return new AchievementEntity()
        {
            HomeId = r.HomeId,
            VisionaryId = r.ParentId,
            AchieverId = r.ChildId,
            Name = r.AchievementName,
            Description = r.AchievementDescription,
            IconCode = r.AchievementIconCode,
            PointsGranted = r.AchievementPointsGranted
        };
    }
    
    public static PenaltyEntity ToPenaltyEntity(this CreatePenaltyRequest r)
    {
        return new PenaltyEntity()
        {
            HomeId = r.HomeId,
            EnforcerId = r.ParentId,
            ViolatorId = r.ChildId,
            Name = r.PenaltyName,
            Reason = r.PenaltyReason,
            IconCode = r.PenaltyIconCode,
            PointsDeducted = r.PenaltyPointsDeducted
        };
    }
}