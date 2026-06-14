using WMS.Domain.Entities;

namespace WMS.Infrastructure.Persistence.Seeders;

public static class DepartmentSeeder
{
    public static IReadOnlyList<Department> Seed() => new[]
    {
        new Department { DepartmentId = 1, DepartmentName = "Human Resources" },
        new Department { DepartmentId = 2, DepartmentName = "Engineering" },
        new Department { DepartmentId = 3, DepartmentName = "Operations" }
    };
}
