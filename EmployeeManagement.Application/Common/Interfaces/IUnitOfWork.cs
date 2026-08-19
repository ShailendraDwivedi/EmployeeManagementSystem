using EmployeeManagement.Domain.Entities;


namespace EmployeeManagement.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IRepository<Employee> Employees { get; }

    IRepository<Department> Departments { get; }

    IRepository<Designation> Designations { get; }

    IRepository<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}