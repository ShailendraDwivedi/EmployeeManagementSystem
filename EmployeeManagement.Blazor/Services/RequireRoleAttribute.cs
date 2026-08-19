namespace EmployeeManagement.Blazor.Services;

[AttributeUsage(AttributeTargets.Class,
    AllowMultiple = false)]
public sealed class RequireRoleAttribute : Attribute
{
    public string Role { get; }

    public RequireRoleAttribute(string role)
    {
        Role = role;
    }
}