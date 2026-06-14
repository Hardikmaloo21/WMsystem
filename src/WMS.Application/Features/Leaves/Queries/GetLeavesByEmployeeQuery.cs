using MediatR;
using WMS.Application.Features.Leaves.DTOs;
using WMS.Domain.Common;

namespace WMS.Application.Features.Leaves.Queries;

public sealed record GetLeavesByEmployeeQuery(
    int EmployeeId,
    int PageNumber,
    int PageSize
) : IRequest<PagedResult<LeaveDto>>;