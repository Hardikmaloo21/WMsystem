// WMS.Application/Features/Employees/Commands/UpdateEmployeeCommand.cs
using MediatR;
using WMS.Application.Features.Employees.DTOs;

namespace WMS.Application.Features.Employees.Commands;

public record UpdateEmployeeCommand(UpdateEmployeeDto Dto) : IRequest<EmployeeDto>;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateEmployeeCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow; _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken ct)
    {
        var employee = await _uow.Employees.GetByIdAsync(request.Dto.EmployeeId, ct)
            ?? throw new NotFoundException(nameof(Employee), request.Dto.EmployeeId);

        _mapper.Map(request.Dto, employee);
        employee.UpdatedOn = DateTime.UtcNow;
        await _uow.Employees.UpdateAsync(employee, ct);
        await _uow.SaveChangesAsync(ct);

        var updated = await _uow.Employees.GetWithDetailsAsync(employee.EmployeeId, ct);
        return _mapper.Map<EmployeeDto>(updated);
    }
}