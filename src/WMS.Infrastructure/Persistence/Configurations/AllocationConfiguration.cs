using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Persistence.Configurations;

public sealed class AllocationConfiguration : IEntityTypeConfiguration<EmployeeProjectAllocation>
{
    public void Configure(EntityTypeBuilder<EmployeeProjectAllocation> builder)
    {
        builder.HasKey(x => x.AllocationId);
    }
}
