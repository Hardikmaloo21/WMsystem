using AutoMapper;
using MediatR;
using WMS.Application.Features.Leaves.DTOs;
using WMS.Domain.Common;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Leaves.Queries;

public sealed class GetLeavesByEmployeeQueryHandler
    : IRequestHandler<
        GetLeavesByEmployeeQuery,
        PagedResult<LeaveDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetLeavesByEmployeeQueryHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<LeaveDto>> Handle(
        GetLeavesByEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        var result =
            await _uow.Leaves.GetByEmployeeAsync(
                request.EmployeeId,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

        return new PagedResult<LeaveDto>
        {
            Items = _mapper.Map<IReadOnlyList<LeaveDto>>(
                result.Items
            ),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}