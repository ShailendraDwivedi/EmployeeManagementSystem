namespace EmployeeManagement.Application.Authentication.DTOs;

public class JwtUserDto
{
    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; }

    public bool IsActive { get; set; }

    public IList<string> Roles { get; set; } = new List<string>();
}