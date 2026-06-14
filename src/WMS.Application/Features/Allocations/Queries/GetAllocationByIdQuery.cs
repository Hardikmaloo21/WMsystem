using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Exceptions;
using WMS.Application.Features.Allocations.DTOs;

namespace WMS.Application.Features.Allocations.Queries;

public record GetAllocationByIdQuery(int AllocationId)
    : IRequest<AllocationDto>;

public class GetAllocationByIdQueryHandler
    : IRequestHandler<GetAllocationByIdQuery, AllocationDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllocationByIdQueryHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<AllocationDto> Handle(
        GetAllocationByIdQuery request,
        CancellationToken ct)
    {
        var allocation = await _uow.Allocations.Query()
            .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
            .Include(a => a.Project)
            .FirstOrDefaultAsync(
                a => a.AllocationId == request.AllocationId,
                ct);

        if (allocation == null)
            throw new NotFoundException(
                nameof(EmployeeProjectAllocation),
                request.AllocationId);

        return _mapper.Map<AllocationDto>(allocation);
    }
}