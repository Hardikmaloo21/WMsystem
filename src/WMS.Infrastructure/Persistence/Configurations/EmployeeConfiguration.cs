// WMS.Infrastructure/Persistence/Configurations/EmployeeConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.DepartmentId);
        builder.HasIndex(e => e.RoleId);
        builder.HasIndex(e => e.Status);

        builder.Property(e => e.Gender)
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(e => e.Status)
            .HasDefaultValue("Active");

        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Role)
            .WithMany(r => r.Employees)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasCheckConstraint("CK_Employee_DOB",
            "DATEDIFF(year, DOB, GETDATE()) >= 18");
    }
}