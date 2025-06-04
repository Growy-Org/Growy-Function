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

    public static ChildEntity ToChildEntity(this ChildRequest c)
    {
        return new ChildEntity
        {
            Name = c.Name,
            DOB = c.DOB,
            Gender = c.Gender.ToString(),
            PointsEarned = c.PointsEarned,
        };
    }

    public static ParentEntity ToParentEntity(this ParentRequest p)
    {
        return new ParentEntity
        {
            Name = p.Name,
            DOB = p.DOB,
            Role = p.Role.ToString(),
        };
    }

    public static HomeEntity ToHomeEntity(this HomeRequest h)
    {
        return new HomeEntity()
        {
            Name = h.Name,
            Address = h.Address,
        };
    }

    public static AssignmentEntity ToAssignmentEntity(this CreateAssignmentRequest r)
    {
        return new AssignmentEntity
        {
            Name = r.AssignmentName,
            Description = r.AssignmentDescription,
            HomeId = r.HomeId,
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

    public static WishEntity ToWishEntity(this WishRequest r)
    {
        return new WishEntity
        {
            GenieId = r.ParentId,
            WisherId = r.ChildId,
            Name = r.Name,
            Description = r.Description,
            PointsCost = r.PointsCost
        };
    }

    public static AchievementEntity ToAchievementEntity(this AchievementRequest r)
    {
        return new AchievementEntity()
        {
            VisionaryId = r.ParentId,
            AchieverId = r.ChildId,
            Name = r.Name,
            Description = r.Description,
            PointsGranted = r.PointsGranted
        };
    }

    public static PenaltyEntity ToPenaltyEntity(this PenaltyRequest r)
    {
        return new PenaltyEntity()
        {
            EnforcerId = r.ParentId,
            ViolatorId = r.ChildId,
            Name = r.Name,
            Reason = r.Reason,
            PointsDeducted = r.PointsDeducted
        };
    }

    public static DevelopmentQuotientResultEntity ToDqAssessmentEntity(this DevelopmentReportRequest r)
    {
        return new DevelopmentQuotientResultEntity
        {
            CandidateId = r.ChildId,
            ExaminerId = r.ParentId,
            Answer = r.Answers,
            TotalScore = r.TotalScore,
            DqResult = r.DqResult,
            CandidateMonth = r.CandidateMonth,
        };
    }
}