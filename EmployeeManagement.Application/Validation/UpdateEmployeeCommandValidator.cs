using FluentValidation;
using EmployeeManagement.Application.Employees.Commands;

namespace EmployeeManagement.Application.Validation
{
    public class UpdateEmployeeCommandValidator : EmployeeBaseValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            ApplyCommonRules();
            
            RuleFor(command => command.Id)
                .GreaterThan(0).WithMessage("Employee ID is required for update operation.");
        }
    }
}