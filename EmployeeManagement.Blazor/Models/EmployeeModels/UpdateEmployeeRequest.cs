using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Blazor.Models.EmployeeModels;

public class UpdateEmployeeRequest
{
    [Required(ErrorMessage = "Employee code is required.")]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must contain 10 digits.")]
    public string Phone { get; set; } = string.Empty;

    public Guid DepartmentId { get; set; }

    public Guid DesignationId { get; set; }

    public decimal Salary { get; set; }

    public DateTime JoiningDate { get; set; }

    public bool IsActive { get; set; }
}