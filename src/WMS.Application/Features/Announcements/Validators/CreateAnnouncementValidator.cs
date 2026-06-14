using FluentValidation;
using WMS.Application.Features.Announcements.Commands;
using WMS.Application.Features.Announcements.DTOs;

public class CreateAnnouncementValidator
    : AbstractValidator<CreateAnnouncementCommand>
{
    public CreateAnnouncementValidator()

    {
        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Dto.Message)
            .NotEmpty().WithMessage("Message is required.");

        RuleFor(x => x.Dto.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be a valid employee ID.");
    }
}