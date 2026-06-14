﻿// File: WMS.Application/Features/Allocations/Commands/AssignEmployeeCommand.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Allocations.DTOs;
using WMS.Domain.Common;


namespace WMS.Application.Features.Allocations.Commands;


public record AssignEmployeeCommand(AssignEmployeeDto Dto) : IRequest<AllocationDto>;

public class AssignEmployeeCommandHandler
    : IRequestHandler<AssignEmployeeCommand, AllocationDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AssignEmployeeCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<AllocationDto> Handle(
        AssignEmployeeCommand request, CancellationToken ct)
    {
        // Validate employee exists and is active
        var employee = await _uow.Employees.GetByIdAsync(request.Dto.EmpId, ct)
            ?? throw new NotFoundException(nameof(Employee), request.Dto.EmpId);

        if (employee.Status != "Active")
            throw new ConflictException("Cannot assign an inactive employee to a project.");

        // Validate project exists and is active
        var project = await _uow.Projects.GetByIdAsync(request.Dto.ProjectId, ct)
            ?? throw new NotFoundException(nameof(Project), request.Dto.ProjectId);

        if (project.Status != "Active")
            throw new ConflictException("Cannot assign employees to a completed project.");

        // Check for duplicate active allocation
        var alreadyAllocated = await _uow.Allocations.ExistsAsync(
            a => a.EmpId == request.Dto.EmpId
              && a.ProjectId == request.Dto.ProjectId
              && a.Status, ct);

        if (alreadyAllocated)
            throw new ConflictException(
                "Employee is already actively allocated to this project.");

        var allocation = _mapper.Map<EmployeeProjectAllocation>(request.Dto);
        allocation.CreateDate = DateTime.UtcNow;
        allocation.Status = true;

        await _uow.Allocations.AddAsync(allocation, ct);
        await _uow.SaveChangesAsync(ct);

        var created = await _uow.Allocations.Query()
            .Include(a => a.Employee).ThenInclude(e => e.Department)
            .Include(a => a.Project)
            .FirstOrDefaultAsync(a => a.AllocationId == allocation.AllocationId, ct);

        return _mapper.Map<AllocationDto>(created);
    }
}