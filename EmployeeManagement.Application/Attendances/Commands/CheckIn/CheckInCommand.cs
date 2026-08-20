using MediatR;

namespace EmployeeManagement.Application.Attendances.Commands.CheckIn;

public class CheckInCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; set; }
}