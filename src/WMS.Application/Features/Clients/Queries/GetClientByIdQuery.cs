using AutoMapper;
using MediatR;
using WMS.Application.Common.Exceptions;
using WMS.Application.Features.Clients.DTOs;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Clients.Queries;

public sealed record GetClientByIdQuery(int Id)
    : IRequest<ClientDto>;

public sealed class GetClientByIdQueryHandler
    : IRequestHandler<GetClientByIdQuery, ClientDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetClientByIdQueryHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ClientDto> Handle(
        GetClientByIdQuery request,
        CancellationToken cancellationToken)
    {
        var client =
            await _uow.Clients.GetByIdAsync(
                request.Id,
                cancellationToken);

        if (client == null)
            throw new NotFoundException(
                nameof(Client),
                request.Id);

        return _mapper.Map<ClientDto>(client);
    }
}