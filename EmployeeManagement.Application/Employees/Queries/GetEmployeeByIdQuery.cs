using MediatR;
using EmployeeManagement.Domain;
using EmployeeManagement.Application.Interfaces;

namespace EmployeeManagement.Application.Employees.Queries
{
    public record GetEmployeeByIdQuery(int Id) : IRequest<Employee?>;
    
    public class GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
        : IRequestHandler<GetEmployeeByIdQuery, Employee?>
    {
        public async Task<Employee?> Handle(
            GetEmployeeByIdQuery request, 
            CancellationToken cancellationToken)
        {
            return await employeeRepository.GetByIdAsync(request.Id);
        }
    }
}