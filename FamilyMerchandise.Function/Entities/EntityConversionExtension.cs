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

    public static Assignment ToAssignment(this AssignmentEntity a)
    {
        return new Assignment
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            IconCode = a.IconCode,
            Points = a.Points,
            RepeatAfter = a.RepeatAfter,
            DueDate = a.DueDate,
            Assignee = new Child()
            {
                Id = a.AssigneeId,
            },
            Assigner = new Parent()
            {
                Id = a.AssignerId,
            },
            CreatedDateUtc = a.CreatedDateUtc,
            UpdatedDateUtc = a.UpdatedDateUtc,
            CompletedDateUtc = a.UpdatedDateUtc,
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
}