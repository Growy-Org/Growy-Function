using Growy.Function.Models.Dtos;
using Growy.Function.Entities;

namespace Growy.Function.Models;

public static class ModelConversionExtension
{
    public static void HydrateParents(this Home h, List<Parent> parents)
    {
        h.Parents.AddRange(parents);
    }

    public static void HydrateChildren(this Home h, List<Child> children)
    {
        h.Children.AddRange(children);
    }

    public static AppUserEntity ToAppUserEntity(this AppUser u)
    {
        return new AppUserEntity()
        {
            Id = u.Id,
            Email = u.Email,
            IdentityProvider = u.IdentityProvider,
            DisplayName = u.DisplayName,
            IdpId = u.IdpId,
            Sku = u.Sku.ToString(),
        };
    }

    public static void SetSteps(this Assignment h, List<Step> steps)
    {
        h.Steps.AddRange(steps);
    }

    public static void SetAssigner(this Assignment h, List<Parent> parents)
    {
        parents.ForEach(p =>
        {
            if (h.Assigner.Id == p.Id)
            {
                h.Assigner = p;
            }
        });
    }

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

    public static ChildEntity ToChildEntity(this EditChildRequest r)
    {
        return new ChildEntity()
        {
            Id = r.ChildId,
            Name = r.ChildName,
            DOB = r.ChildDoB,
            Gender = r.ChildGender.ToString(),
            IconCode = r.ChildIconCode,
        };
    }

    public static ParentEntity ToParentEntity(this Parent p, Guid homeId)
    {
        return new ParentEntity
        {
            Name = p.Name,
            IconCode = p.IconCode ?? 5, // Default Child Avatar
            DOB = p.DOB,
            Role = p.Role.ToString(),
            HomeId = homeId,
        };
    }

    public static ParentEntity ToParentEntity(this EditParentRequest r)
    {
        return new ParentEntity()
        {
            Id = r.ParentId,
            Name = r.ParentName,
            DOB = r.ParentDoB,
            Role = r.ParentRole.ToString(),
            IconCode = r.ParentIconCode,
        };
    }

    public static HomeEntity ToHomeEntity(this CreateHomeRequest r)
    {
        return new HomeEntity()
        {
            Name = r.HomeName,
            Address = r.HomeAddress,
            AppUserId = r.AppUserId
        };
    }

    public static HomeEntity ToHomeEntity(this EditHomeRequest r)
    {
        return new HomeEntity()
        {
            Id = r.HomeId,
            Name = r.HomeName,
            Address = r.HomeAddress,
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
            DueDateUtc = r.DueDateUtc,
            AssigneeId = r.ChildId,
            AssignerId = r.ParentId
        };
    }

    public static AssignmentEntity ToAssignmentEntity(this EditAssignmentRequest r)
    {
        return new AssignmentEntity
        {
            Id = r.AssignmentId,
            Name = r.AssignmentName,
            Description = r.AssignmentDescription,
            IconCode = r.AssignmentIconCode,
            Points = r.Points,
            RepeatAfter = r.RepeatAfter,
            DueDateUtc = r.DueDateUtc,
            AssigneeId = r.ChildId,
            AssignerId = r.ParentId
        };
    }

    public static StepEntity ToStepEntity(this CreateStepRequest r)
    {
        return new StepEntity()
        {
            StepOrder = r.StepOrder,
            AssignmentId = r.AssignmentId,
            Description = r.StepDescription,
        };
    }

    public static StepEntity ToStepEntity(this EditStepRequest r)
    {
        return new StepEntity()
        {
            Id = r.StepId,
            Description = r.StepDescription,
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

    public static WishEntity ToWishEntity(this EditWishRequest r)
    {
        return new WishEntity
        {
            Id = r.WishId,
            GenieId = r.ParentId,
            WisherId = r.ChildId,
            Name = r.WishName,
            Description = r.WishDescription,
            IconCode = r.WishIconCode,
            PointsCost = r.WishCost
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

    public static AchievementEntity ToAchievementEntity(this EditAchievementRequest r)
    {
        return new AchievementEntity()
        {
            Id = r.AchievementId,
            VisionaryId = r.ParentId,
            AchieverId = r.ChildId,
            Name = r.AchievementName,
            Description = r.AchievementDescription,
            IconCode = r.AchievementIconCode,
            PointsGranted = r.AchievementPointsGranted,
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

    public static PenaltyEntity ToPenaltyEntity(this EditPenaltyRequest r)
    {
        return new PenaltyEntity()
        {
            Id = r.PenaltyId,
            EnforcerId = r.ParentId,
            ViolatorId = r.ChildId,
            Name = r.PenaltyName,
            Reason = r.PenaltyReason,
            IconCode = r.PenaltyIconCode,
            PointsDeducted = r.PenaltyPointsDeducted
        };
    }

    public static DevelopmentQuotientResultEntity ToDqAssessmentEntity(this SubmitDevelopmentReportRequest r)
    {
        return new DevelopmentQuotientResultEntity
        {
            HomeId = r.HomeId,
            CandidateId = r.CandidateId,
            ExaminerId = r.ExaminerId,
            Answer = r.Answers,
            TotalScore = r.TotalScore,
            DqResult = r.DqResult,
            CandidateMonth = r.CandidateMonth,
        };
    }
}