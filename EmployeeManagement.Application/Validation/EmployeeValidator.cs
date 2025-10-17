using FluentValidation;
using EmployeeManagement.Domain;

namespace EmployeeManagement.Application.Validation
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(employee => employee.FirstName)
                .NotEmpty().WithMessage("First name is required.");
            
            RuleFor(employee => employee.LastName)
                .NotEmpty().WithMessage("Last name is required.");
            
            RuleFor(employee => employee.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address is required.");
            
            RuleFor(employee => employee.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches("^[0-9]{10}$").WithMessage("Phone number must be exactly 10 digits and contain no symbols or spaces.");
            
            RuleFor(employee => employee.Position)
                .NotEmpty().WithMessage("Position is required.")
                .MaximumLength(100).WithMessage("Position cannot exceed 100 characters.");
        }
    }
}