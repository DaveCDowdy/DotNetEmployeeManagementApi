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
        var employeeToDelete = await employeeRepository.GetByIdAsync(request.Id);
        
        if (employeeToDelete == null)
        {
            throw new KeyNotFoundException($"Employee with ID {request.Id} not found for deletion.");
        }
        
        await employeeRepository.DeleteAsync(request.Id); 
            
        return Unit.Value;
    }
}