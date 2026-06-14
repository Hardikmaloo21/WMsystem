

using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Projects.DTOs;
using WMS.Domain.Common;


namespace WMS.Application.Features.Projects.Commands;

public record CreateProjectCommand(CreateProjectDto Dto) : IRequest<ProjectDto>;


public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateProjectCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken ct)
    {
        var exists = await _uow.Projects.ExistsAsync(
            p => p.ProjectName == request.Dto.ProjectName, ct);

        if (exists)
            throw new ConflictException(
                $"Project '{request.Dto.ProjectName}' already exists.");

        if (request.Dto.ClientId.HasValue)
        {
            var clientExists = await _uow.Clients.ExistsAsync(
                c => c.ClientId == request.Dto.ClientId.Value && c.Status, ct);
            if (!clientExists)
                throw new NotFoundException("Client", request.Dto.ClientId.Value);
        }

        var project = _mapper.Map<Project>(request.Dto);
        await _uow.Projects.AddAsync(project, ct);
        await _uow.SaveChangesAsync(ct);

        var created = await _uow.Projects.Query()
            .Include(p => p.Client)
            .Include(p => p.Allocations)
            .FirstOrDefaultAsync(p => p.ProjectId == project.ProjectId, ct);

        return _mapper.Map<ProjectDto>(created);
    }
}
