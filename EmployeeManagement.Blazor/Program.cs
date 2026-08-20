using EmployeeManagement.Blazor.Components;
using EmployeeManagement.Blazor.Services;
using EmployeeManagement.Blazor.Services.Attendance;
using EmployeeManagement.Blazor.Services.Auth;
using EmployeeManagement.Blazor.Services.Department;
using EmployeeManagement.Blazor.Services.Designation;
using EmployeeManagement.Blazor.Services.Employee;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var apiBaseUrl =
    builder.Configuration["ApiSettings:BaseUrl"];

if (string.IsNullOrWhiteSpace(apiBaseUrl))
{
    throw new InvalidOperationException(
        "ApiSettings:BaseUrl is not configured.");
}
builder.Services.AddAuthorizationCore();

// Authentication
builder.Services.AddScoped<
    JwtAuthenticationStateProvider>();

builder.Services.AddScoped<
    AuthenticationStateProvider>(sp =>
        sp.GetRequiredService<
            JwtAuthenticationStateProvider>());
// Token storage
builder.Services.AddScoped<
    ITokenStorageService,
    TokenStorageService>();

// Authentication service
builder.Services.AddScoped<
    IAuthService,
    AuthService>();

// Employee service
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<JwtAuthorizationHandler>();

// API client
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress =
        new Uri(apiBaseUrl);
});

// Employee API client
builder.Services.AddHttpClient("EmployeeAPI", client =>
{
    client.BaseAddress =
        new Uri(apiBaseUrl);
});
//.AddHttpMessageHandler<
//       JwtAuthorizationHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/Error",
        createScopeForErrors: true);

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();