using EmployeeManagement.API.Middleware;
using EmployeeManagement.Application.Common.Behaviors;
using EmployeeManagement.Application.Common.Mappings;
using EmployeeManagement.Application.Departments.Commands.CreateDepartment;
using EmployeeManagement.Application.Designations.Commands.CreateDesignation;
using EmployeeManagement.Application.Employees.Commands.CreateEmployee;
using EmployeeManagement.Infrastructure;
using EmployeeManagement.Infrastructure.Seed;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// ==========================================
// Infrastructure
// ==========================================

builder.Services.AddInfrastructure(
    builder.Configuration);

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
// ==========================================
// Authentication / JWT
// ==========================================
var jwtSettings =
    builder.Configuration.GetSection("JwtSettings");

var secretKey =
    jwtSettings["SecretKey"];

if (string.IsNullOrWhiteSpace(secretKey))
{
    throw new InvalidOperationException(
        "JWT SecretKey is not configured.");
}
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer =
                      jwtSettings["Issuer"],

                ValidAudience =
                    jwtSettings["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey)),

                ClockSkew = TimeSpan.Zero
            };
    });
builder.Services.AddAuthorization();

// ==========================================
// FluentValidation
// ==========================================

builder.Services.AddValidatorsFromAssembly(
    typeof(CreateEmployeeCommandValidator).Assembly);


// ==========================================
// MediatR
// ==========================================

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateEmployeeCommand).Assembly);

    cfg.AddOpenBehavior(
        typeof(ValidationBehavior<,>));
});
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateDepartmentCommand).Assembly);

    cfg.AddOpenBehavior(
        typeof(ValidationBehavior<,>));
});
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateDesignationCommand).Assembly);

    cfg.AddOpenBehavior(
        typeof(ValidationBehavior<,>));
});
// ==========================================
// Controllers
// ==========================================

builder.Services.AddControllers();


// ==========================================
// Swagger
// ==========================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
                "Enter JWT token. Example: Bearer {token}"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type =
                                ReferenceType.SecurityScheme,

                            Id = "Bearer"
                        }
                },

                Array.Empty<string>()
            }
        });
});


// ==========================================
// Build
// ==========================================

var app = builder.Build();


// ==========================================
// Identity Seeder
// ==========================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeeder.SeedAsync(services);
}


// ==========================================
// Swagger
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


// ==========================================
// Exception Middleware
// ==========================================

app.UseMiddleware<ExceptionHandlingMiddleware>();


// ==========================================
// HTTPS
// ==========================================

app.UseHttpsRedirection();


// ==========================================
// Authentication
// ==========================================

app.UseAuthentication();


// ==========================================
// Authorization
// ==========================================

app.UseAuthorization();


// ==========================================
// Controllers
// ==========================================

app.MapControllers();


// ==========================================
// Run
// ==========================================

app.Run();