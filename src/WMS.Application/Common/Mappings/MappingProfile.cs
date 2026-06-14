// WMS.Application/Common/Mappings/MappingProfile.cs
using AutoMapper;
using WMS.Domain.Entities;
using WMS.Application.Features.Employees.DTOs;
using WMS.Application.Features.Departments.DTOs;
using WMS.Application.Features.Attendance.DTOs;
using WMS.Application.Features.Leaves.DTOs;
using WMS.Application.Features.Clients.DTOs;
using WMS.Application.Features.Projects.DTOs;
using WMS.Application.Features.Allocations.DTOs;
using WMS.Application.Features.Announcements.DTOs;
namespace WMS.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Employee
        CreateMap<Employee, EmployeeDto>()
            .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department.DepartmentName))
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.RoleName))
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()));

        CreateMap<CreateEmployeeDto, Employee>()
            .ForMember(d => d.Gender, o => o.MapFrom(s =>
                Enum.Parse<Domain.Enums.Gender>(s.Gender)));

        CreateMap<UpdateEmployeeDto, Employee>()
            .ForMember(d => d.Gender, o => o.MapFrom(s =>
                Enum.Parse<Domain.Enums.Gender>(s.Gender)))
            .ForAllMembers(o => o.Condition((_, _, srcMember) => srcMember != null));

        // Department
        CreateMap<Department, DepartmentDto>()
    .ForMember(
        d => d.EmployeeCount,
        o => o.MapFrom(s => s.Employees.Count));


        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();

        // Attendance
        CreateMap<Attendance, AttendanceDto>()
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee.FirstName + " " + s.Employee.LastName));

        // Leave
        CreateMap<Leave, LeaveDto>()
    .ForMember(
        d => d.EmployeeName,
        o => o.MapFrom(s =>
            s.Employee != null
                ? s.Employee.FirstName + " " + s.Employee.LastName
                : string.Empty))

    .ForMember(
        d => d.ApproverName,
        o => o.MapFrom(s =>
            s.Approver != null
                ? s.Approver.FirstName + " " + s.Approver.LastName
                : null));

CreateMap<ApplyLeaveDto, Leave>();

CreateMap<Client, ClientDto>();

CreateMap<CreateClientDto, Client>();

CreateMap<UpdateClientDto, Client>();

CreateMap<Project, ProjectDto>()
    .ForMember(
        d => d.ClientName,
        o => o.MapFrom(s =>
            s.Client != null
                ? s.Client.ClientName
                : null))
    .ForMember(
        d => d.AllocatedEmployeeCount,
        o => o.MapFrom(s =>
            s.Allocations.Count));

CreateMap<CreateProjectDto, Project>();

CreateMap<UpdateProjectDto, Project>();

CreateMap<EmployeeProjectAllocation, AllocationDto>()
    .ForMember(d => d.EmployeeName,
        o => o.MapFrom(s =>
            s.Employee.FirstName + " " + s.Employee.LastName))
    .ForMember(d => d.DepartmentName,
        o => o.MapFrom(s =>
            s.Employee.Department.DepartmentName))
    .ForMember(d => d.ProjectName,
        o => o.MapFrom(s =>
            s.Project.ProjectName));

            CreateMap<AssignEmployeeDto, EmployeeProjectAllocation>();

CreateMap<UpdateAllocationDto, EmployeeProjectAllocation>();
CreateMap<CreateAnnouncementDto, Announcement>();

CreateMap<UpdateAnnouncementDto, Announcement>();
CreateMap<Announcement, AnnouncementDto>()
    .ForMember(
        d => d.CreatedByName,
        o => o.MapFrom(s =>
            s.CreatedByEmployee.FirstName + " " +
            s.CreatedByEmployee.LastName));    }
    
    
}