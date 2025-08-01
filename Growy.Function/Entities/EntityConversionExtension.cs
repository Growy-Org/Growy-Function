using Growy.Function.Models;

namespace Growy.Function.Entities;

public static class EntityConversionExtension
{
    public static AppUser ToAppUser(this AppUserEntity a)
    {
        return new AppUser()
        {
            Id = a.Id,
            IdentityProvider = a.IdentityProvider,
            Email = a.Email,
            Sku = (AppSku)Enum.Parse(typeof(AppSku), a.Sku),
            IdpId = a.IdpId
        };
    }

    public static Home ToHome(this HomeEntity a)
    {
        return new Home
        {
            Id = a.Id,
            Name = a.Name,
            Address = a.Address,
        };
    }

    public static Parent ToParent(this ParentEntity p)
    {
        return new Parent
        {
            Id = p.Id,
            Name = p.Name,
            Role = (ParentRole)Enum.Parse(typeof(ParentRole), p.Role),
            DOB = p.DOB,
        };
    }

    public static Child ToChild(this ChildEntity c)
    {
        return new Child()
        {
            Id = c.Id,
            Name = c.Name,
            Gender = (ChildGender)Enum.Parse(typeof(ChildGender), c.Gender),
            PointsEarned = c.PointsEarned,
            DOB = c.DOB,
        };
    }

    public static Assignment ToAssignment(this AssignmentEntity a)
    {
        return new Assignment
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            Points = a.Points,
            DueDateUtc = a.DueDateUtc,
            CreatedDateUtc = a.CreatedDateUtc,
            UpdatedDateUtc = a.UpdatedDateUtc,
            CompletedDateUtc = a.CompletedDateUtc,
        };
    }

    public static Step ToStep(this StepEntity s)
    {
        return new Step()
        {
            Id = s.Id,
            StepOrder = s.StepOrder,
            Description = s.Description,
            IsCompleted = s.CompletedDateUtc != null,
        };
    }

    public static Wish ToWish(this WishEntity w)
    {
        return new Wish
        {
            Id = w.Id,
            Name = w.Name,
            Description = w.Description,
            PointsCost = w.PointsCost,
            CreatedDateUtc = w.CreatedDateUtc,
            UpdatedDateUtc = w.UpdatedDateUtc,
            FulFilledDateUtc = w.FulFilledDateUtc,
        };
    }

    public static Achievement ToAchievement(this AchievementEntity a)
    {
        return new Achievement
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            PointsGranted = a.PointsGranted,
            CreatedDateUtc = a.CreatedDateUtc,
            UpdatedDateUtc = a.UpdatedDateUtc,
            AchievedDateUtc = a.AchievedDateUtc,
        };
    }

    public static Penalty ToPenalty(this PenaltyEntity p)
    {
        return new Penalty
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            PointsDeducted = p.PointsDeducted,
            CreatedDateUtc = p.CreatedDateUtc,
            UpdatedDateUtc = p.UpdatedDateUtc,
        };
    }

    public static DevelopmentQuotientResult ToDevelopmentQuotientResult(this DevelopmentQuotientResultEntity d)
    {
        return new DevelopmentQuotientResult
        {
            Id = d.Id,
            Answers = new List<int>(d.Answers),
            TotalMentalAge = d.TotalMentalAge,
            DqResult = d.DqResult,
            CandidateMonth = d.CandidateMonth,
            CreatedDateUtc = d.CreatedDateUtc,
        };
    }
}