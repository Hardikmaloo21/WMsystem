// WMS.Application/Features/Leaves/Commands/ApproveLeaveCommand.cs
using MediatR;
using WMS.Application.Features.Leaves.DTOs;

namespace WMS.Application.Features.Leaves.Commands;

public record ApproveLeaveCommand(ApproveRejectLeaveDto Dto) : IRequest<LeaveDto>;

public class ApproveLeaveCommandHandler : IRequestHandler<ApproveLeaveCommand, LeaveDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ApproveLeaveCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow; _mapper = mapper;
    }

    public async Task<LeaveDto> Handle(
    ApproveLeaveCommand request,
    CancellationToken ct)
{
    var leave =
        await _uow.Leaves.GetByIdAsync(
            request.Dto.LeaveId,
            ct);

    if (leave == null)
        throw new NotFoundException(
            nameof(Leave),
            request.Dto.LeaveId);

    if (leave.Status != "Pending")
        throw new ConflictException(
            "Only pending leaves can be approved or rejected.");

    if (request.Dto.Action == "Approve")
    {
        leave.Status = "Approved";
    }
    else if (request.Dto.Action == "Reject")
    {
        leave.Status = "Rejected";
    }
    else
    {
        throw new Exception("Invalid action.");
    }

    leave.ApprovedBy =
        request.Dto.ApproverId;

    leave.ApprovedOn =
        DateTime.UtcNow;

    await _uow.Leaves.UpdateAsync(
        leave,
        ct);

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
        AppliedOn = leave.AppliedOn,
        ApprovedBy = leave.ApprovedBy,
        ApprovedOn = leave.ApprovedOn
    };
}
}