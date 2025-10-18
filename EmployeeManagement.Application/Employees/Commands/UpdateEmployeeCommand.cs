using MediatR;
using EmployeeManagement.Domain;
using EmployeeManagement.Application.Interfaces;
using AutoMapper;

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
    
    public class UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        : IRequestHandler<UpdateEmployeeCommand, Unit>
    {
        public async Task<Unit> Handle(
            UpdateEmployeeCommand request, 
            CancellationToken cancellationToken)
        {
            var employeeToUpdate = mapper.Map<Employee>(request);
            
            await employeeRepository.UpdateAsync(employeeToUpdate);
            
            return Unit.Value;
        }
    }
}