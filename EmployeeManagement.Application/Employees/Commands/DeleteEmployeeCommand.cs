using MediatR;
using EmployeeManagement.Application.Interfaces;

namespace EmployeeManagement.Application.Employees.Commands;

public record DeleteEmployeeCommand(int Id) : IRequest<Unit>;
    
public class DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    : IRequestHandler<DeleteEmployeeCommand, Unit>
{
    public async Task<Unit> Handle(
        DeleteEmployeeCommand request, 
        CancellationToken cancellationToken)
    {
        await employeeRepository.DeleteAsync(request.Id);
            
        return Unit.Value;
    }
}