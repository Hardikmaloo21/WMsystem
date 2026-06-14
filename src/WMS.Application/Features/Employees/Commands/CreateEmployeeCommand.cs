// WMS.Application/Features/Employees/Commands/CreateEmployeeCommand.cs
using MediatR;
using WMS.Application.Features.Employees.DTOs;

namespace WMS.Application.Features.Employees.Commands;

public record CreateEmployeeCommand(CreateEmployeeDto Dto) : IRequest<EmployeeDto>;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateEmployeeCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken ct)
    {
        // Check duplicate email
        var exists = await _uow.Employees.ExistsAsync(
            e => e.Email == request.Dto.Email, ct);
        if (exists)
            throw new ConflictException($"An employee with email '{request.Dto.Email}' already exists.");

        // Age validation (>= 18)
        var age = DateTime.Today.Year - request.Dto.DOB.Year;
        if (request.Dto.DOB.Date > DateTime.Today.AddYears(-age)) age--;
        if (age < 18)
            throw new ConflictException("Employee email already exists");

        var employee = _mapper.Map<Employee>(request.Dto);
        await _uow.Employees.AddAsync(employee, ct);
        await _uow.SaveChangesAsync(ct);

        // Create UserLogin for the new employee
        var userLogin = new UserLogin
        {
            Username = request.Dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Dto.Password, 12),
            RoleId = request.Dto.RoleId,
            EmployeeId = employee.EmployeeId
        };
        await _uow.UserLogins.AddAsync(userLogin, ct);
        await _uow.SaveChangesAsync(ct);

        // Reload with navigation
        var created = await _uow.Employees.GetWithDetailsAsync(employee.EmployeeId, ct);
        return _mapper.Map<EmployeeDto>(created);
    }
}