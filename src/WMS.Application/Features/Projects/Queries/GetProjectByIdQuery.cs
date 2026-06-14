using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Exceptions;
using WMS.Application.Features.Projects.DTOs;

public record GetProjectByIdQuery(int ProjectId)
    : IRequest<ProjectDto>;

public class GetProjectByIdQueryHandler
    : IRequestHandler<GetProjectByIdQuery, ProjectDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetProjectByIdQueryHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ProjectDto> Handle(
        GetProjectByIdQuery request,
        CancellationToken ct)
    {
        var project = await _uow.Projects.Query()
            .Include(p => p.Client)
            .Include(p => p.Allocations)
            .FirstOrDefaultAsync(
                p => p.ProjectId == request.ProjectId,
                ct);

        if (project == null)
            throw new NotFoundException(
                nameof(Project),
                request.ProjectId);

        return _mapper.Map<ProjectDto>(project);
    }
}