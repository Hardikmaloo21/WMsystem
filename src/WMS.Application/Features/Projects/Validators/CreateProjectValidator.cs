using FluentValidation;
using WMS.Application.Features.Projects.Commands;

public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectValidator()

    {
        RuleFor(x => x.Dto.ProjectName)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(100).WithMessage("Project name cannot exceed 100 characters.");

        RuleFor(x => x.Dto.EndDate)
            .GreaterThanOrEqualTo(x => x.Dto.StartDate)
            .When(x => x.Dto.StartDate.HasValue && x.Dto.EndDate.HasValue)
            .WithMessage("End date must be on or after start date.");
    }
}