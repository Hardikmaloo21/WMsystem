// WMS.Application/Features/Leaves/Validators/ApplyLeaveValidator.cs
using FluentValidation;
using WMS.Application.Features.Leaves.Commands;

namespace WMS.Application.Features.Leaves.Validators;

public class ApplyLeaveValidator : AbstractValidator<ApplyLeaveCommand>
{
    public ApplyLeaveValidator()
    {
        RuleFor(x => x.Dto.EmpId).GreaterThan(0);

        RuleFor(x => x.Dto.LeaveType)
            .NotEmpty()
            .Must(t => new[] { "Sick", "Casual", "Earned" }.Contains(t))
            .WithMessage("Leave type must be Sick, Casual, or Earned.");

        RuleFor(x => x.Dto.FromDate)
    .GreaterThanOrEqualTo(DateTime.Today.AddDays(-1))
            .WithMessage("From date cannot be in the past.");

        RuleFor(x => x.Dto.ToDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.Dto.FromDate)
            .WithMessage("To date must be on or after from date.");
    }
}