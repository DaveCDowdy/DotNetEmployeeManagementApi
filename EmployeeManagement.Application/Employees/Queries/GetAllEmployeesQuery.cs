using MediatR;
using EmployeeManagement.Domain;
using EmployeeManagement.Application.Interfaces;

namespace EmployeeManagement.Application.Employees.Queries
{
    public record GetAllEmployeesQuery : IRequest<IEnumerable<Employee>>;
    
    public class GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository)
        : IRequestHandler<GetAllEmployeesQuery, IEnumerable<Employee>>
    {
        public async Task<IEnumerable<Employee>> Handle(
            GetAllEmployeesQuery request, 
            CancellationToken cancellationToken)
        {
            return await employeeRepository.GetAllAsync();
        }
    }
}