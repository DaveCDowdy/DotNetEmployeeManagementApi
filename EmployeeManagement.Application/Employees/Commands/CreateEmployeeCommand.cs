using MediatR;
using EmployeeManagement.Domain;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.DTOs;
using AutoMapper;

namespace EmployeeManagement.Application.Employees.Commands
{
    public record CreateEmployeeCommand : IRequest<EmployeeResponse>, IEmployeeCommand
    {
        public string FirstName { get; init; } = "";
        public string LastName { get; init; } = "";
        public string Email { get; init; } = "";
        public string Phone { get; init; } = "";
        public string Position { get; init; } = "";
    }
    
    public class CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        : IRequestHandler<CreateEmployeeCommand, EmployeeResponse>
    {
        public async Task<EmployeeResponse> Handle(
            CreateEmployeeCommand request, 
            CancellationToken cancellationToken)
        {
            var employee = mapper.Map<Employee>(request);
            
            var newEmployee = await employeeRepository.AddAsync(employee);
            
            return mapper.Map<EmployeeResponse>(newEmployee);
        }
    }
}