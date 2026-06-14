using AutoMapper;
using MediatR;
using WMS.Application.Features.Clients.DTOs;
using WMS.Domain.Common;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Clients.Queries;

public sealed record GetClientsQuery(
    string? Search,
    int PageNumber,
    int PageSize
) : IRequest<PagedResult<ClientDto>>;

public sealed class GetClientsQueryHandler
    : IRequestHandler<GetClientsQuery, PagedResult<ClientDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetClientsQueryHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<ClientDto>> Handle(
        GetClientsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _uow.Clients.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            string.IsNullOrWhiteSpace(request.Search)
                ? null
                : x => x.ClientName.Contains(request.Search),
            q => q.OrderBy(x => x.ClientName),
            cancellationToken);

        return new PagedResult<ClientDto>
        {
            Items = _mapper.Map<IReadOnlyList<ClientDto>>(result.Items),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}