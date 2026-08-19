namespace EmployeeManagement.Blazor.Services
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequireAuthenticationAttribute : Attribute
    {
    }
}
