using MediatR;

namespace WMS.Application.Features.Leaves.Commands;

public sealed record CancelLeaveCommand(
    int LeaveId
) : IRequest;