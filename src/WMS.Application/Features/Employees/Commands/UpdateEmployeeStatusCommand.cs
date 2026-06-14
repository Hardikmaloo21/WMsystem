using MediatR;
using WMS.Application.Features.Employees.DTOs;

namespace WMS.Application.Features.Employees.Commands;

public record UpdateEmployeeStatusCommand(
    int EmployeeId,
    string Status
) : IRequest<Unit>;

public class UpdateEmployeeStatusCommandHandler
    : IRequestHandler<UpdateEmployeeStatusCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public UpdateEmployeeStatusCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(
        UpdateEmployeeStatusCommand request,
        CancellationToken ct)
    {
        var employee = await _uow.Employees.GetByIdAsync(
            request.EmployeeId,
            ct);

        if (employee == null)
            throw new NotFoundException(
                nameof(Employee),
                request.EmployeeId);

        employee.Status = request.Status;
        employee.UpdatedOn = DateTime.UtcNow;

        await _uow.Employees.UpdateAsync(employee, ct);
        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}