// WMS.Application/Features/Leaves/Commands/ApplyLeaveCommand.cs
using MediatR;
using WMS.Application.Features.Leaves.DTOs;

namespace WMS.Application.Features.Leaves.Commands;

public record ApplyLeaveCommand(ApplyLeaveDto Dto) : IRequest<LeaveDto>;

public class ApplyLeaveCommandHandler : IRequestHandler<ApplyLeaveCommand, LeaveDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ApplyLeaveCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow; _mapper = mapper;
    }

    public async Task<LeaveDto> Handle(ApplyLeaveCommand request, CancellationToken ct)
    {
        // Check for overlapping leave
        var overlapping = await _uow.Leaves.ExistsAsync(l =>
            l.EmpId == request.Dto.EmpId &&
            l.Status != "Rejected" &&
            l.Status != "Cancelled" &&
            l.FromDate <= request.Dto.ToDate &&
            l.ToDate >= request.Dto.FromDate, ct);

        if (overlapping)
            throw new ConflictException("An overlapping leave request already exists for this period.");

        var leave = _mapper.Map<Leave>(request.Dto);
        leave.Status = "Pending";
        leave.AppliedOn = DateTime.UtcNow;

        await _uow.Leaves.AddAsync(leave, ct);
        await _uow.SaveChangesAsync(ct);
        return new LeaveDto
{
    LeaveId = leave.LeaveId,
    EmpId = leave.EmpId,
    LeaveType = leave.LeaveType,
    Reason = leave.Reason,
    FromDate = leave.FromDate,
    ToDate = leave.ToDate,
    Status = leave.Status,
    AppliedOn = leave.AppliedOn
};
    }
}