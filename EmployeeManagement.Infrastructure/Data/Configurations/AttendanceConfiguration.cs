using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.HasKey(x => x.AttendanceId);

        builder.Property(x => x.CheckIn)
            .IsRequired();

        builder.Property(x => x.CheckOut)
            .IsRequired(false);

        builder.Property(x => x.WorkingHours)
            .HasPrecision(5, 2);

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.Attendances)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}