using AutoMapper;
using MediatR;
using WMS.Application.Features.Clients.DTOs;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Clients.Commands;

public sealed record CreateClientCommand(
    CreateClientDto Client)
    : IRequest<ClientDto>;

public sealed class CreateClientCommandHandler
    : IRequestHandler<CreateClientCommand, ClientDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateClientCommandHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ClientDto> Handle(
        CreateClientCommand request,
        CancellationToken cancellationToken)
    {
        var client =
            _mapper.Map<Client>(request.Client);

        await _uow.Clients.AddAsync(
            client,
            cancellationToken);

        await _uow.SaveChangesAsync(
            cancellationToken);

        return _mapper.Map<ClientDto>(client);
    }
}