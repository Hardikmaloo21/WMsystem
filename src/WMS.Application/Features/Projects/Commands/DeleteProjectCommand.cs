

using MediatR;
using WMS.Domain.Common;

namespace WMS.Application.Features.Projects.Commands;

// DeleteProjectCommand.cs
public record DeleteProjectCommand(int ProjectId) : IRequest<Unit>;


public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public DeleteProjectCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException(nameof(Project), request.ProjectId);

        var hasActiveAllocations = await _uow.Allocations.ExistsAsync(
            a => a.ProjectId == request.ProjectId && a.Status, ct);

        if (hasActiveAllocations)
            throw new ConflictException(
                "Cannot delete a project with active employee allocations.");

        await _uow.Projects.DeleteAsync(project, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
