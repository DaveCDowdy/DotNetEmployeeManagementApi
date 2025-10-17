using MediatR;
using EmployeeManagement.Domain;
using EmployeeManagement.Application.Interfaces;

namespace EmployeeManagement.Application.Employees.Commands
{
    public record CreateEmployeeCommand : IRequest<Employee>, IEmployeeCommand
    {
        // These properties will eventually be DTOs, but we use Domain entities for now.
        public string FirstName { get; init; } = "";
        public string LastName { get; init; } = "";
        public string Email { get; init; } = "";
        public string Phone { get; init; } = "";
        public string Position { get; init; } = "";
    }
    
    public class CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        : IRequestHandler<CreateEmployeeCommand, Employee>
    {
        public async Task<Employee> Handle(
            CreateEmployeeCommand request, 
            CancellationToken cancellationToken)
        {
            // Map Command data to Domain Entity
            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Position = request.Position
            };
            
            return await employeeRepository.AddAsync(employee);
        }
    }
}