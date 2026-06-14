// File: WMS.Application/Features/Allocations/Commands/UpdateAllocationCommand.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Allocations.DTOs;
using WMS.Domain.Common;


namespace WMS.Application.Features.Allocations.Commands;


public record UpdateAllocationCommand(UpdateAllocationDto Dto) : IRequest<AllocationDto>;

public class UpdateAllocationCommandHandler
    : IRequestHandler<UpdateAllocationCommand, AllocationDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateAllocationCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<AllocationDto> Handle(
        UpdateAllocationCommand request, CancellationToken ct)
    {
        var allocation = await _uow.Allocations.GetByIdAsync(
            request.Dto.AllocationId, ct)
            ?? throw new NotFoundException(
                nameof(EmployeeProjectAllocation), request.Dto.AllocationId);

        allocation.Status = request.Dto.Status;
        allocation.UpdatedBy = request.Dto.UpdatedBy;
        allocation.UpdatedDate = DateTime.UtcNow;

        await _uow.Allocations.UpdateAsync(allocation, ct);
        await _uow.SaveChangesAsync(ct);

        var updated = await _uow.Allocations.Query()
            .Include(a => a.Employee).ThenInclude(e => e.Department)
            .Include(a => a.Project)
            .FirstOrDefaultAsync(a => a.AllocationId == allocation.AllocationId, ct);

        return _mapper.Map<AllocationDto>(updated);
    }
}