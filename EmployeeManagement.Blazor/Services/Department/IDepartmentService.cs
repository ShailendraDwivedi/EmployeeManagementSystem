using EmployeeManagement.Blazor.Models;
using EmployeeManagement.Blazor.Models.DepartmentModels;

namespace EmployeeManagement.Blazor.Services.Department
{
    public interface IDepartmentService
    {
        Task<PagedResult<DepartmentDto>> GetDepartmentsAsync(
      int pageNumber = 1,
      int pageSize = 10,
      string? search = null);

        Task<DepartmentDto?> GetDepartmentByIdAsync(Guid id);
        Task<bool> CreateDepartmentAsync(CreateDepartmentRequest request);
        Task<bool> UpdateDepartmentAsync(Guid id, UpdateDepartmentRequest request);
        Task<bool> DeleteDepartmentAsync(Guid id);
    }
}
