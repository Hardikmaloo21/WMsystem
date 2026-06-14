using AutoMapper;
using MediatR;
using WMS.Application.Common.Exceptions;
using WMS.Application.Features.Clients.DTOs;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Clients.Commands;

public sealed record UpdateClientCommand(
    UpdateClientDto Client
) : IRequest<ClientDto>;

public sealed class UpdateClientCommandHandler
    : IRequestHandler<UpdateClientCommand, ClientDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateClientCommandHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ClientDto> Handle(
        UpdateClientCommand request,
        CancellationToken cancellationToken)
    {
        var client = await _uow.Clients.GetByIdAsync(
            request.Client.ClientId,
            cancellationToken);

        if (client == null)
            throw new NotFoundException(
                nameof(Client),
                request.Client.ClientId);

        client.ClientName = request.Client.ClientName;
        client.ClientAddress = request.Client.ClientAddress;
        client.ClientPhoneNumber = request.Client.ClientPhoneNumber;
        client.ClientLocation = request.Client.ClientLocation;
        client.Status = request.Client.Status;

        await _uow.Clients.UpdateAsync(
            client,
            cancellationToken);

        await _uow.SaveChangesAsync(
            cancellationToken);

        return _mapper.Map<ClientDto>(client);
    }
}