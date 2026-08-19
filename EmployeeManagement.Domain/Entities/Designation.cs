using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

public class Designation : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Employee> Employees
        = new List<Employee>();
}