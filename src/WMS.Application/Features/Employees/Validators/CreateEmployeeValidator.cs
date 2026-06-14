// WMS.Application/Features/Employees/Validators/CreateEmployeeValidator.cs
using FluentValidation;
using WMS.Application.Features.Employees.Commands;

namespace WMS.Application.Features.Employees.Validators;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.Dto.FirstName)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.Dto.LastName)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.Dto.Email)
            .NotEmpty().EmailAddress().MaximumLength(80);

        RuleFor(x => x.Dto.PhoneNumber)
            .NotEmpty().MaximumLength(15)
            .Matches(@"^\+?[0-9\s\-\(\)]{7,15}$")
            .WithMessage("Invalid phone number format.");

        RuleFor(x => x.Dto.Gender)
            .NotEmpty().Must(g => new[] { "Male", "Female", "Other" }.Contains(g))
            .WithMessage("Gender must be Male, Female, or Other.");

        RuleFor(x => x.Dto.DOB)
            .NotEmpty()
            .Must(dob => DateTime.Today.Year - dob.Year >= 18)
            .WithMessage("Employee must be at least 18 years old.");

        RuleFor(x => x.Dto.DOJ)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Date of joining cannot be in the future.");

        RuleFor(x => x.Dto.DepartmentId).GreaterThan(0);
        RuleFor(x => x.Dto.RoleId).GreaterThan(0);

        RuleFor(x => x.Dto.Username)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.Dto.Password)
            .NotEmpty().MinimumLength(8);
    }
}