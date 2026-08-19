using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

public class Employee : BaseEntity
{
    public string EmployeeCode { get; set; } = "";

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string Email { get; set; } = "";

    public string Phone { get; set; } = "";

    public DateTime DOB { get; set; }

    public string Gender { get; set; } = "";

    public string Address { get; set; } = "";

    public decimal Salary { get; set; }

    public Guid DepartmentId { get; set; }

    public Guid DesignationId { get; set; }

    public bool IsActive { get; set; }

    public DateTime JoiningDate { get; set; }

    public string? ImageUrl { get; set; }

    public Department? Department { get; set; }

    public Designation? Designation { get; set; }
}