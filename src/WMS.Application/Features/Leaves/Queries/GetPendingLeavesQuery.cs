using MediatR;
using WMS.Application.Features.Leaves.DTOs;

namespace WMS.Application.Features.Leaves.Queries;

public sealed record GetPendingLeavesQuery()
    : IRequest<IReadOnlyList<LeaveDto>>;