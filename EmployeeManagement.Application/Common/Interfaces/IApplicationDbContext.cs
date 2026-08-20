using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Employee> Employees { get; }

    DbSet<Department> Departments { get; }

    DbSet<Designation> Designations { get; }

    DbSet<Attendance> Attendances { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}