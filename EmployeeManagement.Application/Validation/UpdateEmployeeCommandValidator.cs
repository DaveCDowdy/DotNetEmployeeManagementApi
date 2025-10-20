using FluentValidation;
using EmployeeManagement.Application.Employees.Commands;
using EmployeeManagement.Application.Validation;

namespace EmployeeManagement.Application.Validation
{
    // This validates the Command used by the PUT endpoint
    public class UpdateEmployeeCommandValidator : EmployeeBaseValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            // Apply all common rules (FirstName, LastName, Email, etc.)
            ApplyCommonRules();
            
            // Add the CRITICAL update-specific rule: ID must be present and valid
            RuleFor(command => command.Id)
                .GreaterThan(0).WithMessage("Employee ID is required for update operation.");
        }
    }
}