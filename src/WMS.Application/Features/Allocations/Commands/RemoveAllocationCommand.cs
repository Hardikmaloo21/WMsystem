// File: WMS.Application/Features/Allocations/Commands/RemoveAllocationCommand.cs
using MediatR;
using WMS.Domain.Common;

namespace WMS.Application.Features.Allocations.Commands;


public record RemoveAllocationCommand(int AllocationId) : IRequest<Unit>;

public class RemoveAllocationCommandHandler
    : IRequestHandler<RemoveAllocationCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public RemoveAllocationCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(
        RemoveAllocationCommand request, CancellationToken ct)
    {
        var allocation = await _uow.Allocations.GetByIdAsync(request.AllocationId, ct)
            ?? throw new NotFoundException(
                nameof(EmployeeProjectAllocation), request.AllocationId);

        // Soft-remove — mark inactive
        allocation.Status = false;
        allocation.UpdatedDate = DateTime.UtcNow;
        await _uow.Allocations.UpdateAsync(allocation, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}