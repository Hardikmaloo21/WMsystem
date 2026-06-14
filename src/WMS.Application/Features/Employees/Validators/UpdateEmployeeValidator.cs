using FluentValidation;
using WMS.Application.Features.Employees.Commands;

namespace WMS.Application.Features.Employees.Validators;

public sealed class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeValidator()
    {
    }
}
