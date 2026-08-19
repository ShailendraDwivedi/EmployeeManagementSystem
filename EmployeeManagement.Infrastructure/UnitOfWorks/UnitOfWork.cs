using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Infrastructure.Repositories;
using EmployeeManagement.Application.Common.Interfaces;


namespace EmployeeManagement.Infrastructure.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private IRepository<Employee>? _employees;
    private IRepository<Department>? _departments;
    private IRepository<Designation>? _designations;
    private IRepository<RefreshToken>? _refreshTokens;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IRepository<Employee> Employees =>
        _employees ??= new Repository<Employee>(_context);

    public IRepository<Department> Departments =>
        _departments ??= new Repository<Department>(_context);

    public IRepository<Designation> Designations =>
        _designations ??= new Repository<Designation>(_context);

    public IRepository<RefreshToken> RefreshTokens =>
        _refreshTokens ??= new Repository<RefreshToken>(_context);

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(
            cancellationToken);
    }
}