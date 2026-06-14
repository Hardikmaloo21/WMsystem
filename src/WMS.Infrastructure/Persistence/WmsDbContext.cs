// WMS.Infrastructure/Persistence/WmsDbContext.cs
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Persistence;

public class WmsDbContext : DbContext
{
    public WmsDbContext(DbContextOptions<WmsDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Leave> Leaves => Set<Leave>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<EmployeeProjectAllocation> EmployeeProjectAllocations => Set<EmployeeProjectAllocation>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<UserLogin> UserLogins => Set<UserLogin>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all IEntityTypeConfiguration classes from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Seed Data
        SeedRoles(modelBuilder);
        SeedDepartments(modelBuilder);
    }

    private static void SeedRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = "Admin", Description = "System Administrator", CreatedOn = new DateTime(2024, 1, 1) },
            new Role { RoleId = 2, RoleName = "Manager", Description = "Department Manager", CreatedOn = new DateTime(2024, 1, 1) },
            new Role { RoleId = 3, RoleName = "Employee", Description = "Regular Employee", CreatedOn = new DateTime(2024, 1, 1) }
        );
    }

    private static void SeedDepartments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>().HasData(
            new Department { DepartmentId = 1, DepartmentName = "HR", Description = "Human Resources", CreatedOn = new DateTime(2024, 1, 1) },
            new Department { DepartmentId = 2, DepartmentName = "IT", Description = "Information Technology", CreatedOn = new DateTime(2024, 1, 1) },
            new Department { DepartmentId = 3, DepartmentName = "Finance", Description = "Finance & Accounts", CreatedOn = new DateTime(2024, 1, 1) },
            new Department { DepartmentId = 4, DepartmentName = "Operations", Description = "Business Operations", CreatedOn = new DateTime(2024, 1, 1) }
        );
    }
}