# Employee Management System

A full-stack **Employee Management System** built using **ASP.NET Core Web API** and **Blazor**, following **Clean Architecture**, Repository Pattern, CQRS/MediatR, Entity Framework Core, JWT Authentication, and SQL Server.

The application provides a complete solution for managing employees, departments, designations, authentication, employee status, searching, filtering, sorting, pagination, and employee details.

---

## 🚀 Project Overview

The Employee Management System is designed to demonstrate how to build a modern enterprise-level application using ASP.NET Core Web API as the backend and Blazor as the frontend.

### Main Components

* **ASP.NET Core Web API** – Backend REST APIs
* **Blazor** – Frontend user interface
* **SQL Server** – Database
* **Entity Framework Core** – ORM
* **JWT** – Authentication and authorization
* **MediatR** – CQRS implementation
* **FluentValidation** – Request validation
* **AutoMapper** – Object mapping
* **Repository Pattern** – Data access abstraction
* **Clean Architecture** – Separation of concerns

---

# 🏗️ Architecture

The project follows **Clean Architecture** principles.

```text
EmployeeManagement
│
├── EmployeeManagement.API
│   ├── Controllers
│   ├── Middleware
│   ├── Program.cs
│   └── appsettings.json
│
├── EmployeeManagement.Application
│   ├── Employees
│   │   ├── Commands
│   │   └── Queries
│   ├── Departments
│   ├── Designations
│   ├── Authentication
│   ├── DTOs
│   ├── Interfaces
│   ├── Validators
│   └── Common
│
├── EmployeeManagement.Domain
│   ├── Entities
│   ├── Enums
│   └── Common
│
├── EmployeeManagement.Infrastructure
│   ├── Data
│   ├── Repositories
│   ├── Identity
│   ├── Services
│   └── Migrations
│
└── EmployeeManagement.Blazor
    ├── Pages
    ├── Components
    ├── Services
    ├── Models
    ├── Layout
    └── wwwroot
```

### Architecture Flow

```text
Blazor UI
    │
    ▼
HTTP Client
    │
    ▼
ASP.NET Core Web API
    │
    ▼
Controllers
    │
    ▼
MediatR / CQRS
    │
    ▼
Application Layer
    │
    ▼
Repository
    │
    ▼
Entity Framework Core
    │
    ▼
SQL Server
```

---

# 🛠️ Technologies Used

## Backend

* ASP.NET Core Web API
* C#
* .NET 8
* Entity Framework Core
* SQL Server
* MediatR
* CQRS
* AutoMapper
* FluentValidation
* JWT Authentication
* ASP.NET Core Identity
* Repository Pattern
* Unit of Work
* Clean Architecture
* Swagger / OpenAPI

## Frontend

* Blazor
* Razor Components
* HTML5
* CSS3
* Bootstrap
* C#
* HttpClient

## Development Tools

* Visual Studio
* SQL Server Management Studio
* Git
* GitHub
* Swagger UI

---

# ✨ Features

## Authentication

* User Registration
* User Login
* JWT Authentication
* Role-based Authorization
* Password Hashing
* Refresh Token support
* Logout

## Employee Management

* Create Employee
* Update Employee
* Delete Employee
* Employee Details
* Employee List
* Employee Status
* Activate Employee
* Deactivate Employee

## Employee List

The Employee List provides:

* Search
* Pagination
* Sorting
* Department Filter
* Designation Filter
* Employee Status
* Employee Details navigation

Example:

```text
Employee List

---------------------------------------------------------------
Search Employee: [________________]  Department: [All ▼]
Designation: [All ▼]                  Status: [All ▼]
---------------------------------------------------------------

Name          Email              Department    Designation
---------------------------------------------------------------
John Smith    john@test.com      IT            Developer
David Brown   david@test.com     HR            Manager
---------------------------------------------------------------

        < Previous   1  2  3  Next >
```

---

# 👨‍💼 Employee Module

Employee information can include:

* Employee Id
* First Name
* Last Name
* Email
* Phone Number
* Date of Birth
* Date of Joining
* Department
* Designation
* Salary
* Status
* Created Date
* Modified Date

---

# 🏢 Department Module

The Department module provides:

* Create Department
* Update Department
* Delete Department
* Department List
* Department filtering

Example departments:

```text
IT
HR
Finance
Administration
Sales
Marketing
```

---

# 💼 Designation Module

The Designation module provides:

* Create Designation
* Update Designation
* Delete Designation
* Designation List
* Designation filtering

Example:

```text
Software Developer
Senior Software Developer
Team Lead
Project Manager
Architect
HR Manager
```

---

# 🔐 Authentication Flow

The application uses JWT authentication.

```text
User
 │
 ▼
Login Page
 │
 ▼
POST /api/auth/login
 │
 ▼
Validate User
 │
 ▼
Generate JWT Token
 │
 ▼
Return Access Token
 │
 ▼
Blazor stores authentication information
 │
 ▼
Token added to API requests
 │
 ▼
Authorized API
```

Example JWT request:

```http
Authorization: Bearer <JWT_TOKEN>
```

---

# 📡 Web API

Swagger is available for testing the APIs.

Typical API structure:

```text
/api/auth
/api/employees
/api/departments
/api/designations
```

## Employee APIs

### Get Employees

```http
GET /api/employees
```

Supports:

* Search
* Pagination
* Sorting
* Department filtering
* Designation filtering
* Status filtering

### Get Employee

```http
GET /api/employees/{id}
```

### Create Employee

```http
POST /api/employees
```

### Update Employee

```http
PUT /api/employees/{id}
```

### Delete Employee

```http
DELETE /api/employees/{id}
```

### Change Employee Status

```http
PATCH /api/employees/{id}/status
```

> Update the endpoint names above if your controllers use different routes.

---

# 🗄️ Database

The application uses **Microsoft SQL Server**.

Main tables include:

```text
AspNetUsers
AspNetRoles
AspNetUserRoles

Employees
Departments
Designations
```

Relationship:

```text
Department
     │
     │ 1
     │
     │ *
Employee
     │
     │ *
     │
     │ 1
Designation
```

---

# ⚙️ Configuration

Update the connection string in:

```text
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EmployeeManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

For SQL Server authentication:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EmployeeManagementDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

---

# 🔑 JWT Configuration

Example configuration:

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyHere",
    "Issuer": "EmployeeManagementAPI",
    "Audience": "EmployeeManagementClient",
    "ExpiresInMinutes": 60
  }
}
```

> For production, never commit real JWT keys or passwords to GitHub. Use environment variables, Azure Key Vault, or another secure secret store.

---

# 📦 Installation

## Prerequisites

Install:

* .NET 8 SDK
* SQL Server
* Visual Studio 2022 or later
* Git
* SQL Server Management Studio

Verify .NET:

```bash
dotnet --version
```

---

# 📥 Clone the Repository

```bash
git clone https://github.com/<your-username>/EmployeeManagement.git
```

Navigate to the project:

```bash
cd EmployeeManagement
```

---

# 🗄️ Database Setup

Update the connection string in `appsettings.json`.

Then run Entity Framework migrations.

```bash
dotnet ef database update
```

If migrations are not available, create one:

```bash
dotnet ef migrations add InitialCreate
```

Then:

```bash
dotnet ef database update
```

---

# ▶️ Run the Web API

Navigate to the API project:

```bash
cd EmployeeManagement.API
```

Run:

```bash
dotnet run
```

Swagger will normally be available at:

```text
https://localhost:<port>/swagger
```

---

# ▶️ Run the Blazor Application

Navigate to the Blazor project:

```bash
cd EmployeeManagement.Blazor
```

Run:

```bash
dotnet run
```

Open the URL displayed by the application.

---

# 🔄 Application Flow

```text
                ┌───────────────────────┐
                │       Blazor UI       │
                └───────────┬───────────┘
                            │
                            │ HTTP / JSON
                            ▼
                ┌───────────────────────┐
                │    ASP.NET Core API   │
                └───────────┬───────────┘
                            │
                            ▼
                ┌───────────────────────┐
                │    MediatR / CQRS     │
                └───────────┬───────────┘
                            │
                            ▼
                ┌───────────────────────┐
                │ Application Layer     │
                └───────────┬───────────┘
                            │
                            ▼
                ┌───────────────────────┐
                │ Repository / EF Core  │
                └───────────┬───────────┘
                            │
                            ▼
                ┌───────────────────────┐
                │      SQL Server       │
                └───────────────────────┘
```

---

# 🧩 CQRS Structure

The application separates commands and queries.

Example:

```text
Employees
│
├── Commands
│   ├── CreateEmployee
│   ├── UpdateEmployee
│   ├── DeleteEmployee
│   └── UpdateEmployeeStatus
│
└── Queries
    ├── GetEmployees
    └── GetEmployeeById
```

### Command

Used to modify data.

```text
Create
Update
Delete
Activate
Deactivate
```

### Query

Used to retrieve data.

```text
Get Employee
Get Employees
Search Employees
Filter Employees
```

---

# ✅ Validation

FluentValidation is used to validate requests before saving data.

Example validation:

```text
First Name       → Required
Last Name        → Required
Email            → Required + Valid Email
Department       → Required
Designation      → Required
Salary           → Must be greater than 0
```

---

# 📄 Employee List Capabilities

The Employee List supports server-side or application-level:

```text
Search
   +
Pagination
   +
Sorting
   +
Department Filter
   +
Designation Filter
   +
Status Filter
```

This allows users to efficiently locate employees even when the employee database becomes large.

---

# 🖥️ Blazor Pages

Typical pages include:

```text
/login
/register

/employees
/employees/create
/employees/edit/{id}
/employees/details/{id}

/departments
/departments/create
/departments/edit/{id}

/designations
/designations/create
/designations/edit/{id}
```

> Update these routes to match your actual Blazor routes.

---

# 🔒 Security

The application implements:

* JWT authentication
* Authorization
* Password hashing
* Role-based access
* Protected API endpoints
* Input validation
* Secure configuration

Sensitive configuration should not be committed to source control.

---

# 🧪 Testing

API endpoints can be tested using Swagger.

Example:

```text
1. Register user
2. Login
3. Copy JWT token
4. Authorize in Swagger
5. Create Department
6. Create Designation
7. Create Employee
8. Search Employee
9. Apply filters
10. Test pagination
11. Test sorting
12. Open Employee Details
13. Activate/Deactivate Employee
```

---

# 📌 Future Enhancements

Possible future improvements:

* Dashboard
* Employee profile photo
* Attendance management
* Leave management
* Payroll
* Audit logging
* Email notifications
* Export employees to Excel
* Import employees from Excel
* Redis caching
* Docker support
* Azure deployment
* CI/CD pipeline
* Unit testing
* Integration testing
* Role and permission management

---

# 🐳 Docker Support

Docker can be added to containerize:

```text
Blazor Application
       │
       ▼
ASP.NET Core API
       │
       ▼
SQL Server
```

Example commands:

```bash
docker build -t employee-management-api .
```

Run:

```bash
docker run -p 8080:8080 employee-management-api
```

---

# ☁️ Azure Deployment

The application can be deployed to Azure using:

```text
Azure App Service
       │
       ├── ASP.NET Core Web API
       │
       └── Blazor Application

Azure SQL Database
       │
       ▼
Employee Management Database
```

Recommended Azure services:

* Azure App Service
* Azure SQL Database
* Azure Key Vault
* Azure Storage
* Azure DevOps
* Application Insights

---

# 📁 Git Workflow

Recommended branching strategy:

```text
main
 │
 ├── develop
 │    │
 │    ├── feature/employee-crud
 │    ├── feature/employee-search
 │    ├── feature/employee-filter
 │    └── feature/authentication
 │
 └── release
```

---

# 🤝 Contribution

1. Create a feature branch.

```bash
git checkout -b feature/employee-dashboard
```

2. Make your changes.

3. Commit the changes.

```bash
git add .
git commit -m "Add employee dashboard"
```

4. Push the branch.

```bash
git push origin feature/employee-dashboard
```

5. Create a Pull Request.

---

# 👨‍💻 Author

**Shailendra Kumar Dwivedi**

Senior .NET Developer

### Technologies

```text
ASP.NET Core
C#
Web API
Blazor
Angular
Microservices
Clean Architecture
CQRS
MediatR
Entity Framework Core
SQL Server
Azure
Docker
RabbitMQ
Redis
```

---

# ⭐ Project Highlights

This project demonstrates practical implementation of:

* Clean Architecture
* RESTful Web API
* Blazor frontend
* CQRS
* MediatR
* Repository Pattern
* Entity Framework Core
* SQL Server
* JWT Authentication
* FluentValidation
* AutoMapper
* CRUD operations
* Search
* Sorting
* Pagination
* Department filtering
* Designation filtering
* Employee status management
* Employee details
* API/Blazor integration

---

## 📜 License

This project is created for learning, demonstration, and professional portfolio purposes.

# Attendance Management

## Overview

The **Attendance Management** module is a part of the Employee Management System built using **ASP.NET Core Web API, Clean Architecture, CQRS/MediatR, Entity Framework Core, SQL Server, and Blazor**.

It provides functionality to manage employee attendance records, including:

* Attendance creation
* Attendance listing
* Attendance details
* Employee-wise attendance
* Attendance status
* Date-based filtering
* Search and pagination
* Validation
* RESTful APIs
* Blazor UI integration

---

## Technology Stack

### Backend

* ASP.NET Core Web API
* .NET 8
* C#
* Entity Framework Core
* SQL Server
* MediatR
* CQRS
* Clean Architecture
* FluentValidation
* Repository Pattern
* Dependency Injection
* REST APIs

### Frontend

* Blazor
* Razor Components
* Bootstrap
* HttpClient
* JSON API integration

### Development Tools

* Visual Studio / Visual Studio Code
* SQL Server / SQL Server Management Studio
* Swagger / OpenAPI
* Git
* GitHub
* Docker

---

## Architecture

The Attendance Management module follows **Clean Architecture** principles.

```text
EmployeeManagement
│
├── EmployeeManagement.API
│   ├── Controllers
│   ├── Middleware
│   └── Program.cs
│
├── EmployeeManagement.Application
│   ├── Attendances
│   │   ├── Commands
│   │   ├── Queries
│   │   ├── DTOs
│   │   └── Validators
│   │
│   ├── Employees
│   ├── Departments
│   └── Common
│
├── EmployeeManagement.Domain
│   ├── Entities
│   ├── Enums
│   └── Interfaces
│
├── EmployeeManagement.Infrastructure
│   ├── Data
│   ├── Repositories
│   ├── Identity
│   └── Services
│
└── EmployeeManagement.Blazor
    ├── Pages
    ├── Services
    ├── Models
    └── Shared
```

---

# Attendance Features

## 1. Attendance List

The Attendance List page displays attendance records with:

* Employee name
* Attendance date
* Check-in time
* Check-out time
* Attendance status
* Total working hours
* Pagination
* Search
* Employee filtering
* Date filtering

Example:

```text
---------------------------------------------------------------
Attendance
---------------------------------------------------------------
Search Employee: [ John              ]

From Date: [01/08/2026]   To Date: [21/08/2026]

---------------------------------------------------------------
Employee       Date          Check In   Check Out   Status
---------------------------------------------------------------
John Smith     21/08/2026    09:05 AM   06:10 PM   Present
David Brown    21/08/2026    09:20 AM   06:00 PM   Late
Robert Lee     21/08/2026    -          -          Absent
---------------------------------------------------------------
```

---

# 2. Attendance Details

Each attendance record can be opened using the **Details** button.

The details page displays:

* Employee information
* Attendance date
* Check-in time
* Check-out time
* Attendance status
* Working hours
* Remarks

Example route:

```text
/attendance/details/{id}
```

---

# Attendance Status

The attendance status is represented using an enum.

```csharp
public enum AttendanceStatus
{
    Present = 1,
    Absent = 2,
    Late = 3,
    HalfDay = 4,
    Leave = 5
}
```

The model can expose the status as:

```csharp
public AttendanceStatus Status { get; set; }
```

---

# Attendance DTO

Example Attendance DTO:

```csharp
public class AttendanceDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public DateTime AttendanceDate { get; set; }

    public TimeSpan? CheckInTime { get; set; }

    public TimeSpan? CheckOutTime { get; set; }

    public AttendanceStatus Status { get; set; }

    public decimal? WorkingHours { get; set; }

    public string? Remarks { get; set; }
}
```

---

# API Endpoints

## Get Attendance List

```http
GET /api/attendance
```

Example:

```http
GET /api/attendance?pageNumber=1&pageSize=10
```

With search:

```http
GET /api/attendance?pageNumber=1&pageSize=10&search=John
```

With employee filter:

```http
GET /api/attendance?pageNumber=1&pageSize=10&employeeId={employeeId}
```

With date filter:

```http
GET /api/attendance?pageNumber=1&pageSize=10&fromDate=2026-08-01&toDate=2026-08-21
```

---

## Get Attendance By ID

```http
GET /api/attendance/{id}
```

Example:

```http
GET /api/attendance/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
```

Response:

```json
{
  "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "employeeId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "employeeName": "John Smith",
  "attendanceDate": "2026-08-21",
  "checkInTime": "09:05:00",
  "checkOutTime": "18:10:00",
  "status": "Present",
  "workingHours": 9.08,
  "remarks": null
}
```

---

# Application Layer

The Attendance module follows the CQRS pattern.

## Queries

Example:

```text
Attendances
│
└── Queries
    └── GetAttendances
        ├── GetAttendancesQuery.cs
        └── GetAttendancesQueryHandler.cs
```

The query supports:

```csharp
public class GetAttendancesQuery 
    : IRequest<PagedResult<AttendanceListDto>>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public string? Search { get; }
    public Guid? EmployeeId { get; }
    public DateTime? FromDate { get; }
    public DateTime? ToDate { get; }
}
```

---

# Blazor Attendance Service

The Blazor application communicates with the Web API using `HttpClient`.

Example:

```csharp
public async Task<AttendanceDto?> GetAttendanceByIdAsync(Guid id)
{
    return await _httpClient.GetFromJsonAsync<AttendanceDto>(
        $"api/attendance/{id}");
}
```

---

# Attendance Details Page

The Blazor details page uses the attendance ID from the route.

```razor
@page "/attendance/details/{Id:guid}"

@inject IAttendanceService AttendanceService
@inject NavigationManager Navigation
```

The attendance record is loaded using:

```csharp
protected override async Task OnInitializedAsync()
{
    Attendance = await AttendanceService.GetAttendanceByIdAsync(Id);
}
```

---

# Pagination

Attendance records are returned using a paged result.

Example:

```text
Page 1 of 10

[Previous]  1  2  3  4  5  [Next]
```

Pagination helps prevent loading a large number of attendance records at once.

---

# Search and Filtering

The Attendance module supports multiple filters.

### Employee Search

```text
Search: John
```

### Employee Filter

```text
Employee: John Smith
```

### Date Filter

```text
From Date: 01/08/2026
To Date:   21/08/2026
```

### Status Filter

```text
Status:
- Present
- Absent
- Late
- Half Day
- Leave
```

---

# Database

The Attendance table contains attendance information related to an employee.

Example structure:

```text
Attendance
------------------------------------------------
Id
EmployeeId
AttendanceDate
CheckInTime
CheckOutTime
Status
WorkingHours
Remarks
CreatedDate
ModifiedDate
IsDeleted
------------------------------------------------
```

Relationship:

```text
Department
     │
     └── Employee
            │
            └── Attendance
```

An employee can have multiple attendance records.

```text
Employee 1 ──────────── * Attendance
```

---

# Entity Framework Core

Example relationship:

```csharp
public class Attendance
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public DateTime AttendanceDate { get; set; }

    public TimeSpan? CheckInTime { get; set; }

    public TimeSpan? CheckOutTime { get; set; }

    public AttendanceStatus Status { get; set; }

    public decimal? WorkingHours { get; set; }

    public string? Remarks { get; set; }
}
```

---

# Clean Architecture Flow

The request flows through the application as follows:

```text
Blazor UI
    │
    ▼
HttpClient
    │
    ▼
ASP.NET Core Controller
    │
    ▼
MediatR
    │
    ▼
Query / Command
    │
    ▼
Handler
    │
    ▼
Repository / DbContext
    │
    ▼
SQL Server
```

Response:

```text
SQL Server
    │
    ▼
Repository
    │
    ▼
Handler
    │
    ▼
DTO
    │
    ▼
Controller
    │
    ▼
HttpClient
    │
    ▼
Blazor UI
```

---

# Validation

Attendance input should be validated before saving.

Typical validation rules:

* Employee ID is required.
* Attendance date is required.
* Check-out should not be earlier than check-in.
* Employee must exist.
* Duplicate attendance for the same employee and date should be prevented where applicable.
* Attendance status must be valid.

FluentValidation can be used for application-level validation.

---

# Error Handling

The API uses centralized error handling middleware.

Typical HTTP responses:

```text
200 OK
201 Created
400 Bad Request
401 Unauthorized
403 Forbidden
404 Not Found
500 Internal Server Error
```

Example error response:

```json
{
  "message": "Attendance record was not found."
}
```

---

# Security

The application can use JWT authentication and authorization.

Protected API example:

```http
Authorization: Bearer {access_token}
```

Authorization can be applied to attendance endpoints using:

```csharp
[Authorize]
```

Role-based authorization can also be implemented where required.

---

# Swagger

Swagger/OpenAPI can be used to test Attendance APIs.

After starting the API, open the Swagger UI configured by the application.

From Swagger you can test:

```text
GET    /api/attendance
GET    /api/attendance/{id}
POST   /api/attendance
PUT    /api/attendance/{id}
DELETE /api/attendance/{id}
```

---

# Running the Project

## 1. Clone the Repository

```bash
git clone <repository-url>
```

## 2. Open the Solution

Open:

```text
EmployeeManagement.sln
```

## 3. Configure SQL Server

Update the connection string in the API configuration.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=EmployeeManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

## 4. Apply Database Migration

```bash
dotnet ef database update
```

## 5. Run the API

```bash
dotnet run
```

## 6. Run the Blazor Application

Start the Blazor project from Visual Studio or using:

```bash
dotnet run
```

---

# Testing

Attendance APIs can be tested using:

* Swagger
* Postman
* Browser for GET APIs
* Blazor UI
* Unit tests
* Integration tests

Recommended test scenarios:

```text
✓ Get attendance list
✓ Get attendance by ID
✓ Search attendance
✓ Employee filtering
✓ Date filtering
✓ Pagination
✓ Valid attendance creation
✓ Invalid employee ID
✓ Invalid attendance date
✓ Invalid check-in/check-out time
✓ Duplicate attendance
✓ Unauthorized request
✓ Attendance not found
```

---

# Future Enhancements

The Attendance module can be extended with:

* Attendance dashboard
* Monthly attendance summary
* Employee attendance calendar
* Attendance percentage
* Late arrival report
* Absent employee report
* Monthly payroll integration
* Export to Excel
* Export to PDF
* Attendance charts
* Shift management
* Holiday management
* Leave integration
* Bulk attendance upload
* QR-code attendance
* Biometric integration
* Notification system

---

# Project Goals

The main goals of the Attendance Management module are:

1. Provide a centralized attendance management system.
2. Maintain accurate employee attendance records.
3. Provide fast search and filtering.
4. Follow Clean Architecture principles.
5. Separate commands and queries using CQRS.
6. Provide reusable REST APIs.
7. Provide a responsive Blazor user interface.
8. Maintain secure and scalable application architecture.

---

# Author

**Shailendra Kumar Dwivedi**

Senior .NET Developer

Technologies:

```text
ASP.NET Core
C#
.NET
Web API
Blazor
Clean Architecture
CQRS
MediatR
Entity Framework Core
SQL Server
Azure
Docker
RabbitMQ
Redis
```



