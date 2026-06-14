// WMS.Application/Features/Departments/Commands/CreateDepartmentCommand.cs
using MediatR;
using WMS.Application.Features.Departments.DTOs;

namespace WMS.Application.Features.Departments.Commands;

public record CreateDepartmentCommand(CreateDepartmentDto Dto) : IRequest<DepartmentDto>;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateDepartmentCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow; _mapper = mapper;
    }

    public async Task<DepartmentDto> Handle(CreateDepartmentCommand request, CancellationToken ct)
    {
        var exists = await _uow.Departments.ExistsAsync(
            d => d.DepartmentName == request.Dto.DepartmentName, ct);
        if (exists)
            throw new ConflictException($"Department '{request.Dto.DepartmentName}' already exists.");

        var dept = _mapper.Map<Department>(request.Dto);
        await _uow.Departments.AddAsync(dept, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<DepartmentDto>(dept);
    }
}