using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Blazor.Models.EmployeeModels
{
    public class CreateEmployeeRequest
    {
        [Required(ErrorMessage = "Employee code is required.")]
        [StringLength(20)]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(
        @"^\d{10}$",
        ErrorMessage = "Phone number must contain 10 digits.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Salary must be greater than 0.")]
        [Range(0, 999999999)]
        public decimal? Salary { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public Guid DepartmentId { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        public Guid DesignationId { get; set; }
        [Required(ErrorMessage = "Joining date is required.")]
        public DateTime JoiningDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
