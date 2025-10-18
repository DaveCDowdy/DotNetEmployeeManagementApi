using MediatR;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.DTOs;
using AutoMapper;

namespace EmployeeManagement.Application.Employees.Queries
{
    public record GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeResponse>>;

    public class GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeResponse>>
    {
        public async Task<IEnumerable<EmployeeResponse>> Handle(
            GetAllEmployeesQuery request, 
            CancellationToken cancellationToken)
        {
            var employees = await employeeRepository.GetAllAsync();
            
            return mapper.Map<IEnumerable<EmployeeResponse>>(employees);
        }
    }
}