using WMS.Domain.Entities;

namespace WMS.Infrastructure.Persistence.Seeders;

public static class RoleSeeder
{
    public static IReadOnlyList<Role> Seed() => new[]
    {
        new Role { RoleId = 1, RoleName = "Admin", Description = "System administrator" },
        new Role { RoleId = 2, RoleName = "Manager", Description = "Team manager" },
        new Role { RoleId = 3, RoleName = "Employee", Description = "Standard employee" }
    };
}
