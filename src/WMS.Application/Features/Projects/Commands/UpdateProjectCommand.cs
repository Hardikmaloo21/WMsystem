



using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Projects.DTOs;
using WMS.Domain.Common;


namespace WMS.Application.Features.Projects.Commands;

// UpdateProjectCommand.cs
public record UpdateProjectCommand(UpdateProjectDto Dto) : IRequest<ProjectDto>;


public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateProjectCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.Dto.ProjectId, ct)
            ?? throw new NotFoundException(nameof(Project), request.Dto.ProjectId);

        _mapper.Map(request.Dto, project);
        project.UpdatedOn = DateTime.UtcNow;
        await _uow.Projects.UpdateAsync(project, ct);
        await _uow.SaveChangesAsync(ct);

        var updated = await _uow.Projects.Query()
            .Include(p => p.Client)
            .Include(p => p.Allocations)
            .FirstOrDefaultAsync(p => p.ProjectId == project.ProjectId, ct);

        return _mapper.Map<ProjectDto>(updated);
    }
}
