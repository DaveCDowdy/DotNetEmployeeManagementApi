using MediatR;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.DTOs;
using AutoMapper;

namespace EmployeeManagement.Application.Employees.Queries
{
    public record GetEmployeeByIdQuery(int Id) : IRequest<EmployeeResponse?>;

    public class GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        : IRequestHandler<GetEmployeeByIdQuery, EmployeeResponse?>
    {
        public async Task<EmployeeResponse?> Handle(
            GetEmployeeByIdQuery request, 
            CancellationToken cancellationToken)
        {
            var employee = await employeeRepository.GetByIdAsync(request.Id);
            
            return mapper.Map<EmployeeResponse>(employee);
        }
    }
}