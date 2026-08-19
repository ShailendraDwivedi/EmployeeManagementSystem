using EmployeeManagement.Blazor.Models;
using EmployeeManagement.Blazor.Models.DesignationModels;

namespace EmployeeManagement.Blazor.Services.Designation
{
    public interface IDesignationService
    {
        Task<PagedResult<DesignationDto>> GetDesignationsAsync(
      int pageNumber = 1,
      int pageSize = 10,
      string? search = null);

        Task<DesignationDto?> GetDesignationByIdAsync(Guid id);
        Task<bool> CreateDesignationAsync(CreateDesignationRequest request);
        Task<bool> UpdateDesignationAsync(Guid id, UpdateDesignationRequest request);
        Task<bool> DeleteDesignationAsync(Guid id);
    }
}
