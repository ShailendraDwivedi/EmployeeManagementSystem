using MediatR;

namespace EmployeeManagement.Application.Attendances.Commands.CheckOut;

public class CheckOutCommand : IRequest<bool>
{
    public Guid AttendanceId { get; set; }
}