using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Data.Configurations;

public class DesignationConfiguration
    : IEntityTypeConfiguration<Designation>
{
    public void Configure(
        EntityTypeBuilder<Designation> builder)
    {
        builder.ToTable("Designations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .IsRequired();

        builder.Property(x => x.UpdatedDate)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}