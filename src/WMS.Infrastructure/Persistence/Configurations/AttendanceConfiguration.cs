// WMS.Infrastructure/Persistence/Configurations/AttendanceConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Persistence.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.HasIndex(a => new { a.EmpId, a.AttendanceDate }).IsUnique();
        builder.HasIndex(a => a.AttendanceDate);

        builder.HasOne(a => a.Employee)
            .WithMany(e => e.Attendances)
            .HasForeignKey(a => a.EmpId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}