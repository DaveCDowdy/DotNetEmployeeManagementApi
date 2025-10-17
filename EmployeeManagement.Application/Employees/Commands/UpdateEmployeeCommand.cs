using MediatR;
using EmployeeManagement.Domain;
using EmployeeManagement.Application.Interfaces;

namespace EmployeeManagement.Application.Employees.Commands
{
    public record UpdateEmployeeCommand : IRequest<Unit>, IEmployeeCommand
    {
        public int Id { get; init; }
        public string FirstName { get; init; } = "";
        public string LastName { get; init; } = "";
        public string Email { get; init; } = "";
        public string Phone { get; init; } = "";
        public string Position { get; init; } = "";
    }
    
    public class UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        : IRequestHandler<UpdateEmployeeCommand, Unit>
    {
        public async Task<Unit> Handle(
            UpdateEmployeeCommand request, 
            CancellationToken cancellationToken)
        {

            var employeeToUpdate = new Employee
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Position = request.Position
            };
            
            await employeeRepository.UpdateAsync(employeeToUpdate);
            
            return Unit.Value;
        }
    }
}