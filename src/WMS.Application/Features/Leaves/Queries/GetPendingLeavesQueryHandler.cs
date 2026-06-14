using AutoMapper;
using MediatR;
using WMS.Application.Features.Leaves.DTOs;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Leaves.Queries;

public sealed class GetPendingLeavesQueryHandler
    : IRequestHandler<GetPendingLeavesQuery, IReadOnlyList<LeaveDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetPendingLeavesQueryHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<LeaveDto>> Handle(
        GetPendingLeavesQuery request,
        CancellationToken cancellationToken)
    {
        var leaves =
            await _uow.Leaves.GetPendingAsync(
                cancellationToken);

        return _mapper.Map<
            IReadOnlyList<LeaveDto>
        >(leaves);
    }
}