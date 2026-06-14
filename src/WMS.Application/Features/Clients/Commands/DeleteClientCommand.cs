using MediatR;
using WMS.Application.Common.Exceptions;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Clients.Commands;

public sealed record DeleteClientCommand(int ClientId) : IRequest;

public sealed class DeleteClientCommandHandler
    : IRequestHandler<DeleteClientCommand>
{
    private readonly IUnitOfWork _uow;

    public DeleteClientCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(
        DeleteClientCommand request,
        CancellationToken cancellationToken)
    {
        var client = await _uow.Clients.GetByIdAsync(
            request.ClientId,
            cancellationToken);

        if (client == null)
            throw new NotFoundException(
                nameof(Client),
                request.ClientId);

        var hasProjects =
            _uow.Projects.Query()
                .Any(p => p.ClientId == request.ClientId);

        if (hasProjects)
            throw new ConflictException(
                "Cannot delete client because projects are assigned to it.");

        await _uow.Clients.DeleteAsync(
            client,
            cancellationToken);

        await _uow.SaveChangesAsync(
            cancellationToken);
    }
}