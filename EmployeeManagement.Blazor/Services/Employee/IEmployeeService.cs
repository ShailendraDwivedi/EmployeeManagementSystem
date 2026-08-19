using EmployeeManagement.Blazor.Models;
using EmployeeManagement.Blazor.Models.EmployeeModels;

namespace EmployeeManagement.Blazor.Services.Employee
{
    public interface IEmployeeService
    {
        Task<PagedResult<EmployeeDto>> GetEmployeesAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? search = null);

        Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id);
        Task<bool> CreateEmployeeAsync(CreateEmployeeRequest request);
        Task<bool> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequest request);
        Task<bool> DeleteEmployeeAsync(Guid id);
    }
}
