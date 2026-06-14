// WMS.Infrastructure/Persistence/Configurations/LeaveConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Persistence.Configurations;

public class LeaveConfiguration : IEntityTypeConfiguration<Leave>
{
    public void Configure(EntityTypeBuilder<Leave> builder)
    {
        builder.HasIndex(l => l.EmpId);
        builder.HasIndex(l => l.Status);

        builder.HasOne(l => l.Employee)
            .WithMany(e => e.Leaves)
            .HasForeignKey(l => l.EmpId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Approver)
            .WithMany()
            .HasForeignKey(l => l.ApprovedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasCheckConstraint("CK_Leave_Dates", "[FromDate] <= [ToDate]");
    }
}