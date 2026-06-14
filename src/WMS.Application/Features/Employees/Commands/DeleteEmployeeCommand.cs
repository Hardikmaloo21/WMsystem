// WMS.Application/Features/Employees/Commands/DeleteEmployeeCommand.cs
using MediatR;

namespace WMS.Application.Features.Employees.Commands;

public record DeleteEmployeeCommand(int EmployeeId) : IRequest<Unit>;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public DeleteEmployeeCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken ct)
    {
        var employee = await _uow.Employees.GetByIdAsync(request.EmployeeId, ct)
            ?? throw new NotFoundException(nameof(Employee), request.EmployeeId);

        // Soft delete — set Status to Inactive
        employee.Status = "Inactive";
        employee.UpdatedOn = DateTime.UtcNow;
        await _uow.Employees.UpdateAsync(employee, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}